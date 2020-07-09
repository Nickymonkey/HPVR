using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

namespace HPVR
{
    public class NetworkedHead : MonoBehaviour
    {
        //public GameObject multiplayerHeadRepresentation;
        // Start is called before the first frame update
        void Start()
        {
            Camera VRCamera = GetComponent<Camera>();
            SteamVR_Fade fadeScript = GetComponent<SteamVR_Fade>();
            FlareLayer flareLayer = GetComponent<FlareLayer>();

            if (!isMineOrLocal())
            {
                VRCamera.enabled = false;
                fadeScript.enabled = false;
                flareLayer.enabled = false;
                //multiplayerHeadRepresentation.SetActive(true);
                this.gameObject.tag = "Untagged";
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        bool isMineOrLocal()
        {
            bool photonViewIsMine = GetComponent<PhotonView>().IsMine;
            return photonViewIsMine || PhotonNetwork.InRoom == false;
        }
    }
}