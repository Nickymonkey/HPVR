using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HPVR
{
    public class OpenDoor : MonoBehaviour
    {
        public List<ItemDetector> ItemDetectorList;
        public bool triggered = false;

        private void Start()
        {
            while (!triggered)
            {
                for(int i=0; i<ItemDetectorList.Count; i++)
                {
                    if (!ItemDetectorList[i].itemDetected)
                        break;
                    if (i == ItemDetectorList.Count - 1)
                    {
                        triggered = true;
                    }
                }
            }
        }
    }
}
