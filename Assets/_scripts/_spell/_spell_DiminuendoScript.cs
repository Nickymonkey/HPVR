using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HPVR
{
    public class _spell_DiminuendoScript : MonoBehaviour
    {

        public string spellName = "_spell_DiminuendoScript";
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
            }
            InitialScale = transform.localScale;
            FinalScale = new Vector3(InitialScale.x * (1 / ScalingFactor),
             InitialScale.y * (1 / ScalingFactor),
             InitialScale.z * (1 / ScalingFactor));
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
            StartCoroutine(LerpDown());
            yield
            return new WaitForSeconds(0.25f);
            //second thing
            isShaking = false;
            Destroy(this);
        }

        IEnumerator LerpDown()
        {
            float progress = 0.0f;
            // float currentMass = gameObject.GetComponent<Rigidbody>().mass;
            //gameObject.GetComponent<Rigidbody>().mass = currentMass * (1 / ScalingFactor);
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