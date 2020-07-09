using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class NetworkedHandPhysics : HandPhysics
    {
        protected virtual void Awake()
        {
            if (!isMineOrLocal())
            {
                this.enabled = false;
            }
        }

        // Start is called before the first frame update
        private void Start()
        {
            hand = GetComponent<Hand>();
            //spawn hand collider and link it to us

            handCollider = ((GameObject)Instantiate(handColliderPrefab.gameObject)).GetComponent<HandCollider>();
            Vector3 localPosition = handCollider.transform.localPosition;
            Quaternion localRotation = handCollider.transform.localRotation;

            handCollider.transform.parent = Player_VR.instance.transform;
            handCollider.transform.localPosition = localPosition;
            handCollider.transform.localRotation = localRotation;
            handCollider.hand = this;

            GetComponent<SteamVR_Behaviour_Pose>().onTransformUpdated.AddListener(UpdateHand);
        }

        // Update is called once per frame
        void Update()
        {

        }

        bool isMineOrLocal()
        {
            bool photonViewIsMine = false;
            if (GetComponent<PhotonView>() != null)
            {
                photonViewIsMine = GetComponent<PhotonView>().IsMine;
            }
            return photonViewIsMine || (PhotonNetwork.InRoom == false && PhotonNetwork.InLobby == false);
        }
    }
}