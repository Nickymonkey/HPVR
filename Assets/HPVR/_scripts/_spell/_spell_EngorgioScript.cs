using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class _spell_EngorgioScript : MonoBehaviour
    {

        public string spellName = "_spell_EngorgioScript";
        public float shake_speed = 0.50f;
  public bool isShaking = true;
        public float ScalingFactor = 2.0f;
  public float TimeScale = 5.0f;
  private Vector3 originPosition;
        private Vector3 InitialScale;
        private Vector3 FinalScale;

        void Start()
        {
            if (gameObject.GetComponent<NetworkedObject>())
            {
                gameObject.GetComponent<NetworkedObject>().requestThenTransfer();
                gameObject.GetComponent<NetworkedObject>().currentSpell = spellName;
                //gameObject.GetComponent<Interactable>().attachedToHand.DetachObject(this.gameObject, false);
                //this.interactable.attachedToHand.DetachObject(gameObject, restoreOriginalParent);
            }
            //gameObject.GetComponent<Interactable>().attachedToHand.DetachObject(this.gameObject, false);
            InitialScale = transform.localScale;
            FinalScale = new Vector3(InitialScale.x * ScalingFactor,
             InitialScale.y * ScalingFactor,
             InitialScale.z * ScalingFactor);
            originPosition = transform.position;
            StartCoroutine(Shake());
        }

        void Update()
        {
            if (isShaking)
            {
                float step = shake_speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, originPosition + Random.insideUnitSphere, step);
            }
        }

        IEnumerator Shake()
        {
            //first thing
            isShaking = true;
            StartCoroutine(LerpUp());
            yield
            return new WaitForSeconds(0.25f);
            //second thing
            isShaking = false;
            //StartCoroutine(LerpUp());
            Destroy(this);
        }

        IEnumerator LerpUp()
        {
            float progress = 0.0f;
            //float currentMass = gameObject.GetComponent<Rigidbody>().mass;
            //gameObject.GetComponent<Rigidbody>().mass = currentMass * ScalingFactor;
            while (progress <= 1)
            {
                transform.localScale = Vector3.Lerp(InitialScale, FinalScale, progress);
                progress += Time.deltaTime * TimeScale;
                yield
                return null;
            }
            transform.localScale = FinalScale;
            //Destroy(this);
        }
    }
}