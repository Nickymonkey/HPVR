using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HPVR
{
    public class _spell_AccioScript : MonoBehaviour
    {

        public string spellName = "_spell_AccioScript";
        public float speed = 8.0f;
        public bool flyTowards = false;
        public GameObject hand;
        public Animator currentAnimator;
        public AudioSource currentAudioSource;
        public RuntimeAnimatorController currentController;
        private Vector3 originPosition;
        public bool originalGravity;
        private float lastDistance;
        private bool shaking = false;
        // Use this for initialization
        void Start()
        {
            if (gameObject.GetComponent<NetworkedObject>())
            {
                gameObject.GetComponent<NetworkedObject>().requestThenTransfer();
                gameObject.GetComponent<NetworkedObject>().currentSpell = spellName;
            }
            originPosition = transform.position;
            currentController = (RuntimeAnimatorController)Resources.Load("shakeController");
            if (GetComponent<Animator>() != null)
            {
                DestroyImmediate(gameObject.GetComponent<Animator>());
                currentAnimator = (Animator)gameObject.AddComponent<Animator>();
                currentAnimator.runtimeAnimatorController = currentController;
            }
            else
            {
                currentAnimator = (Animator)gameObject.AddComponent<Animator>();
                currentAnimator.runtimeAnimatorController = currentController;
            }

            if (gameObject.GetComponent<AudioSource>() != null)
            {
                currentAudioSource = gameObject.GetComponent<AudioSource>();
            }
            else
            {
                currentAudioSource = gameObject.AddComponent<AudioSource>();
            }
            currentAudioSource.spatialBlend = 1; // we do want to hear spatialized audio
            StartCoroutine(Shake());
        }

        // Update is called once per frame
        void Update()
        {
            FlyTowards();
        }

        void FlyTowards()
        {
            if (flyTowards)
            {
                // Move our position a step closer to the target.
                float step = speed * Time.deltaTime; // calculate distance to move
                                                     //transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                transform.position = Vector3.MoveTowards(transform.position, hand.transform.position, step);
                float distance = (transform.position - hand.transform.position).magnitude;
                if (Vector3.Distance(transform.position, hand.transform.position) < 0.001f) {
                    KillScript();
                }
                //Debug.Log("Distance= " + distance + ", lastDisance= " + lastDistance);
                if (distance > lastDistance)
                {
                    //KillScript();
                }
                lastDistance = distance;
            }
        }

        IEnumerator Shake()
        {
            flyTowards = false;
            shaking = true;
            currentAnimator.Play("ShakeAnim", 0, 0.0f);
            currentAudioSource.PlayOneShot(Resources.Load("shakeSound") as AudioClip, 0.10F);
            yield
            return new WaitForSeconds(1.0f);
            shaking = false;
            if (GetComponent<Rigidbody>() != null)
            {
                originalGravity = GetComponent<Rigidbody>().useGravity;
                GetComponent<Rigidbody>().useGravity = false;
            }
            currentAnimator.enabled = false;
            Destroy(currentAnimator);
            if (GetComponent<Rigidbody>() != null)
            {
                lastDistance = (transform.position - hand.transform.position).magnitude;
                GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            }
            flyTowards = true;
            AudioClip flyingSound = Resources.Load("flyingSound") as AudioClip;
            currentAudioSource.volume = 1f;
            currentAudioSource.clip = flyingSound;
            currentAudioSource.loop = true;
            currentAudioSource.Play();
        }

        void KillScript()
        {
            currentAudioSource.Stop();
            if (GetComponent<Rigidbody>() != null)
            {
                GetComponent<Rigidbody>().isKinematic = false;
                GetComponent<Rigidbody>().useGravity = originalGravity;
                if (!GetComponent<Rigidbody>().useGravity)
                {
                    AudioClip wingardiumLeviosaSound = Resources.Load("wingardiumLeviosaSound") as AudioClip;
                    currentAudioSource.volume = 0.1f;
                    currentAudioSource.clip = wingardiumLeviosaSound;
                    currentAudioSource.loop = true;
                    currentAudioSource.Play();
                }
                else
                {
                    Destroy(currentAudioSource);
                }
            }
            // Destroy this script
            Destroy(this);
        }

        void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.layer == 10)
            {
                KillScript();
            }
        }

    }

}