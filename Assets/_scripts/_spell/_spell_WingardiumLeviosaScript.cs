using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class _spell_WingardiumLeviosaScript : MonoBehaviour
    {

        public string spellName = "_spell_WingardiumLeviosaScript";
        private Vector3 targetPosition;
        private AudioSource currentAudioSource;
        public float speed = 1.0f;
        public float height = 0.25f;
        public bool timerStarted = false;
        // Use this for initialization
        void Start()
        {

            if (GetComponent<Rigidbody>().useGravity == false)
            {
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
                currentAudioSource.spread = 0; // we dont want to reduce our angle of hearing
                currentAudioSource.spatialBlend = 1; // we do want to hear spatialized audio
                AudioClip wingardiumLeviosaSound = Resources.Load("wingardiumLeviosaSound") as AudioClip;
                currentAudioSource.volume = 0.25f;
                currentAudioSource.clip = wingardiumLeviosaSound;
                //currentAudioSource.loop = true;
                currentAudioSource.Play();
            }
            else
            {
                currentAudioSource = gameObject.AddComponent<AudioSource>();
                currentAudioSource.spatialize = true; // we DO want spatialized audio
                currentAudioSource.spread = 0; // we dont want to reduce our angle of hearing
                currentAudioSource.spatialBlend = 1; // we do want to hear spatialized audio
                AudioClip wingardiumLeviosaSound = Resources.Load("wingardiumLeviosaSound") as AudioClip;
                currentAudioSource.volume = 0.25f;
                currentAudioSource.clip = wingardiumLeviosaSound;
                //currentAudioSource.loop = true;
                currentAudioSource.Play();
            }

            GetComponent<Rigidbody>().useGravity = false;

            if (this.transform.parent != null)
            {
                if (this.transform.parent.name.Contains("Hand"))
                {
                    Destroy(this);
                }
            }

            targetPosition = new Vector3(transform.position.x, transform.position.y + height, transform.position.z);
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            StartCoroutine(TimerStart());
        }

        void Update()
        {
            // Move our position a step closer to the target.
            float step = speed * Time.deltaTime; // calculate distance to move
                                                 //transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            transform.position = Vector3.Lerp(transform.position, targetPosition, step);
            // Check if the position of the cube and sphere are approximately equal.
            if (Vector3.Distance(transform.position, targetPosition) < 0.001f) {
                // Destroy this script
                Destroy(this);
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            //if (timerStarted)
            //{
            //    GetComponent<Rigidbody>().useGravity = false;
            //    Destroy(this);
            //}
        }

        public IEnumerator TimerStart()
        {
            timerStarted = true;
            yield return new WaitForSeconds(3.0f);
            Destroy(this);
        }
    }
}