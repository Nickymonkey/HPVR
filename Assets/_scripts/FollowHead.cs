using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HPVR
{
    public class FollowHead : MonoBehaviour
    {
        public Transform VRHead;
        public Vector3 offset = new Vector3(0f, 0f, 0f);

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.localPosition = new Vector3(VRHead.transform.localPosition.x+offset.x, offset.y, VRHead.transform.localPosition.z + offset.z);
            //this.gameObject.transform.localPosition.(VRHead.transform.localPosition.x, this.gameObject.transform.localPosition.y, VRHead.transform.localPosition.z);
        }
    }
}
