using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HPVR
{
    public class Belt : MonoBehaviour
    {
        public GameObject VRCamera;

        // Start is called before the first frame update
        void Start()
        {
            gameObject.SetActive(isMineOrLocal());
        }

        // Update is called once per frame
        void Update()
        {

        }

        bool isMineOrLocal()
        {
            bool photonViewIsMine = VRCamera.GetComponent<PhotonView>().IsMine;
            return photonViewIsMine || PhotonNetwork.InRoom == false;
        }
    }
}
