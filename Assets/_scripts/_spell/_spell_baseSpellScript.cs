using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class _spell_baseSpellScript : MonoBehaviour
    {
        public int healthDamage = 0;
        public int shieldDamage = 0;
        public float knockbackStrength = 0f;
        public float knockbackRadius = 0f;
        public bool hasExplosion = false;
        public bool spellActivated = false;
        public string SpellExplosion = "Flash";
        public string SpellExplosionSoundClip = "";
        private Color spellColor;
        private string spellColorName;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        public void setColor(Color color, string colorName)
        {
            spellColor = color;
            spellColorName = colorName;
            GetComponent<Renderer>().material.SetColor("_Color", spellColor);
            if(GetComponentInChildren<ParticleSystem>() != null)
            {
                ParticleSystem.MainModule ma = GetComponentInChildren<ParticleSystem>().main;
                ma.startColor = color;
            }
            if(GetComponentInChildren<Light>() != null)
            {
                GetComponentInChildren<Light>().color = color;
            }
            //if (SpellExplosion.Contains("SpellExplosion"))
            //{
            //    SpellExplosion = SpellExplosion + spellColorName;
            //}
            //SpellExplosion = SpellExplosion+ spellColorName;
        }

        private void OnTriggerEnter(Collider other)
        {

            if (other.name != "Wand(Clone)" && spellActivated)
            {
                spellActivated = false;
                GetComponent<Renderer>().enabled = false;

                if (GetComponent<FireSource>() != null)
                {
                    //if (GetComponent<FireSource>().isBurning && GetComponent<FireSource>().canSpreadFromThisSource)
                    //{
                    //    other.SendMessageUpwards("FireExposure", SendMessageOptions.DontRequireReceiver);
                    //}

                    for (int i = 0; i < gameObject.transform.childCount; i++)
                    {
                        GameObject child = gameObject.transform.GetChild(i).gameObject;
                        if (child != null)
                        {
                            child.SetActive(false);
                            if(child.GetComponent<PhotonView>() != null)
                            {
                                if(PhotonNetwork.InRoom || PhotonNetwork.InLobby)
                                {
                                    PhotonNetwork.Destroy(child);
                                }
                            }
                        }
                    }
                }

                if (PhotonNetwork.InRoom || PhotonNetwork.InLobby)
                {
                    if (other.gameObject.tag == "hittable" && healthDamage > 0)
                    {
                        NetworkedGameManager.Instance.projectileHitPlayer(healthDamage);
                    }

                    if (other.gameObject.tag == "shield" && shieldDamage > 0)
                    {
                        NetworkedGameManager.Instance.projectileHitShield(shieldDamage);
                    }
                }

                if (other.gameObject.tag == "mob" && healthDamage > 0)
                {
                    other.gameObject.SendMessageUpwards("TookDamage", healthDamage);
                }

                if(other.gameObject.GetComponent<Breakable>() != null)
                {
                    other.gameObject.GetComponent<Breakable>().BreakFunction();
                }

                if (hasExplosion)
                {
                    Knockback(other);
                    StartCoroutine(SpellExplosionSound());
                }
                else
                {
                    DestroyThis();
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void activate()
        {
            spellActivated = true;
            StartCoroutine(SpellLifeTimer(5.0f));
        }

        public void DestroyThis()
        {
            if (GetComponent<PhotonView>() != null)
            {
                PhotonNetwork.Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        bool isMineOrLocal()
        {
            bool photonViewIsMine = GetComponent<PhotonView>().IsMine;
            return photonViewIsMine || (PhotonNetwork.InRoom == false && PhotonNetwork.InLobby == false);
        }

        private IEnumerator SpellLifeTimer(float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            DestroyThis();
        }

        private IEnumerator SpellExplosionSound()
        {
            AudioSource source = GetComponent<AudioSource>();
            source.PlayOneShot(Resources.Load(SpellExplosionSoundClip) as AudioClip, 1.0f);
            yield return new WaitWhile(() => source.isPlaying);
            DestroyThis();
        }

        void Knockback(Collider other)
        {
            //Debug.Log("knockback");

            if (PhotonNetwork.InRoom || PhotonNetwork.InLobby)
            {
                PhotonNetwork.Instantiate(SpellExplosion, transform.position, transform.rotation, 0);
            }
            else
            {
                Instantiate(Resources.Load(SpellExplosion), transform.position, transform.rotation);
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position, knockbackRadius);

            foreach (Collider hit in colliders)
            {
                if (hit.gameObject.transform.parent)
                {
                    if (hit.gameObject.transform.parent.GetComponent<Rigidbody>() != null)
                    {
                        if (hit.gameObject.transform.parent.gameObject.GetComponent<NetworkedObject>())
                        {
                            hit.gameObject.transform.parent.gameObject.GetComponent<NetworkedObject>().requestThenTransfer();
                        }

                        Rigidbody rb = hit.gameObject.transform.parent.GetComponent<Rigidbody>();

                        if (rb != null)
                        {
                            rb.AddExplosionForce(knockbackStrength, transform.position, knockbackRadius, 0f, ForceMode.Impulse);
                        }
                    }
                    else
                    {
                        if (hit.gameObject.GetComponent<NetworkedObject>())
                        {
                            hit.gameObject.GetComponent<NetworkedObject>().requestThenTransfer();
                        }

                        Rigidbody rb = hit.gameObject.GetComponent<Rigidbody>();

                        if (rb != null)
                        {
                            rb.AddExplosionForce(knockbackStrength, transform.position, knockbackRadius, 0f, ForceMode.Impulse);
                        }
                    }
                }
                else
                {
                    if (hit.gameObject.GetComponent<NetworkedObject>())
                    {
                        hit.gameObject.GetComponent<NetworkedObject>().requestThenTransfer();
                    }

                    Rigidbody rb = hit.gameObject.GetComponent<Rigidbody>();

                    if (rb != null)
                    {
                        rb.AddExplosionForce(knockbackStrength, transform.position, knockbackRadius, 0f, ForceMode.Impulse);
                    }
                }
            }
        }
    }
}