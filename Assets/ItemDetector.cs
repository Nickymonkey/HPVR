using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HPVR
{
    public class ItemDetector : MonoBehaviour
    {
        public GameObject objectToDetect;
        public bool itemDetected = false;
        public ItemDetector otherItemDetector;

        [Tooltip("If pressure plate triggered")]
        public UnityEvent onTrigger;

        [Tooltip("If pressure plate untriggered")]
        public UnityEvent offTrigger;

        private void OnTriggerStay(Collider other)
        {
            if (!itemDetected)
            {
                if (other.gameObject.name.Contains(objectToDetect.name) || other.transform.parent.gameObject.name.Contains(objectToDetect.name))
                {
                    itemDetected = true;

                    if (otherItemDetector.itemDetected)
                    {
                        onTrigger.Invoke();
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (itemDetected)
            {
                itemDetected = false;
                offTrigger.Invoke();
            }
        }

    }
}
