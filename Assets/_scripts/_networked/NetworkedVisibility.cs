using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class NetworkedVisibility : MonoBehaviourPunCallbacks, IPunObservable
    {
        public bool handVisible = true;
        private GameObject handRepresentation;
        void Start()
        {
            if(GetComponent<NetworkedHand>() != null)
            {
                handRepresentation = GetComponent<NetworkedHand>().NetworkedHandRepresentation;
            }
            if (!isMineOrLocal())
            {
                this.gameObject.GetComponent<HandPhysics>().enabled = false;
                this.gameObject.GetComponent<SteamVR_Behaviour_Pose>().enabled = false;
                handRepresentation.SetActive(true);
            }
            else
            {
                this.gameObject.GetComponent<Hand>().enabled = true;
                this.gameObject.GetComponent<HandPhysics>().enabled = true;
                this.gameObject.GetComponent<SteamVR_Behaviour_Pose>().enabled = true;
            }
        }

        void Update()
        {
            if (isMineOrLocal())
            {
                if (this.gameObject.GetComponent<Hand>().AttachedObjects.Count > 0)
                {
                    handVisible = false;
                }
                else
                {
                    handVisible = true;
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(handVisible);
            }
            else
            {
                // Network player, receive data
                handVisible = (bool)stream.ReceiveNext();
                updateHand();
            }
        }

        void updateHand()
        {
            if(handRepresentation != null)
            {
                 handRepresentation.GetComponent<MeshRenderer>().enabled = handVisible;               
            }
        }

        bool isMineOrLocal()
        {
            bool photonViewIsMine = GetComponent<PhotonView>().IsMine;
            return photonViewIsMine || PhotonNetwork.InRoom == false;
        }
    }
}