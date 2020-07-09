using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HPVR
{
    public class _spell_GeminioScript : MonoBehaviour
    {

        public string spellName = "_spell_GeminioScript";
        public bool destroyThis = false;
        public bool doneWaiting = false;
        public Animator currentAnimator;
        public AudioSource currentAudioSource;
        public RuntimeAnimatorController currentController;
        // Use this for initialization
        void Start()
        {

            if (destroyThis)
            {
                currentAudioSource.PlayOneShot(Resources.Load("geminioSound") as AudioClip, 1.0f);
                Destroy(this);
            }

            if (gameObject.GetComponent<NetworkedObject>())
            {
                gameObject.GetComponent<NetworkedObject>().requestThenTransfer();
                gameObject.GetComponent<NetworkedObject>().currentSpell = spellName;
            }

            if (gameObject.GetComponent<AudioSource>() != null)
            {
                currentAudioSource = gameObject.GetComponent<AudioSource>();
                currentAudioSource.spatialize = true; // we DO want spatialized audio
                currentAudioSource.spatialBlend = 1; // we do want to hear spatialized audio
            }
            else
            {
                currentAudioSource = gameObject.AddComponent<AudioSource>();
                currentAudioSource.spatialize = true; // we DO want spatialized audio
                currentAudioSource.spatialBlend = 1; // we do want to hear spatialized audio
            }

            StartCoroutine(Shake());

        }

        // Update is called once per frame
        void Update()
        {
            checkDone();
        }

        IEnumerator Shake()
        {
            yield return new WaitForSeconds(0.1f);
            doneWaiting = true;
        }

        void checkDone()
        {
            if (doneWaiting)
            {
                destroyThis = true;
                doneWaiting = false;
                if (PhotonNetwork.InRoom)
                {
                    PhotonNetwork.Instantiate(gameObject.name.Replace("(Clone)", ""), gameObject.transform.position, gameObject.transform.rotation);
                }
                else
                {
                    //PhotonNetwork.AllocateViewID()
                    //this.gameObject.GetPhotonView().InstantiationId 
                    GameObject clone = Instantiate(this.gameObject, gameObject.transform.position, gameObject.transform.rotation);
                    clone.GetComponent<Rigidbody>().isKinematic = false;
                }
                Destroy(this);
            }
        }
    }
}