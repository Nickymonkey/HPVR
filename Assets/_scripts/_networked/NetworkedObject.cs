using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Photon.Pun;
using Photon.Realtime;
using System;

namespace HPVR
{
    public class NetworkedObject : Throwable, IPunObservable, IPunOwnershipCallbacks
    {
        bool gravityOn = true;
        bool kinematicOn = false;
        float mass = 1;
        public Material defaultMaterial;
        public string currentMaterial;
        public string currentSpell = "";

        // Start is called before the first frame update
        void Start()
        {
            Renderer _renderer = GetComponentInChildren<Renderer>();
            currentMaterial = defaultMaterial.name;
            if (!PhotonNetwork.InLobby && !PhotonNetwork.InRoom)
            {
                if(GetComponent<PhotonTransformView>() != null)
                {
                    Destroy(GetComponent<PhotonTransformView>());
                }

                if (GetComponent<PhotonView>() != null)
                {
                    Destroy(GetComponent<PhotonView>());
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(this.GetComponent<Rigidbody>().useGravity);
                stream.SendNext(this.GetComponent<Rigidbody>().isKinematic);
                stream.SendNext(this.GetComponent<Rigidbody>().mass);
                stream.SendNext(currentMaterial);
            }
            else
            {
                // Network player, receive data
                gravityOn = (bool)stream.ReceiveNext();
                kinematicOn = (bool)stream.ReceiveNext();
                mass = (float)stream.ReceiveNext();
                currentMaterial = (string)stream.ReceiveNext();
                updateObject();
            }
        }

        override protected void HandHoverUpdate(Hand hand)
        {
            GrabTypes startingGrabType = hand.GetGrabStarting();

            if (startingGrabType != GrabTypes.None)
            {
                requestThenTransfer();
                hand.AttachObject(gameObject, startingGrabType, attachmentFlags, attachmentOffset);
                hand.HideGrabHint();
            }
        }

        void updateObject()
        {
            GetComponent<Rigidbody>().useGravity = gravityOn;
            GetComponent<Rigidbody>().isKinematic = kinematicOn;
            GetComponent<Rigidbody>().mass = mass;
            GetComponentInChildren<Renderer>().material = Resources.Load(currentMaterial, typeof(Material)) as Material;
        }

        public void requestThenTransfer()
        {
            if(PhotonNetwork.InLobby || PhotonNetwork.InRoom)
            {
                GetComponent<PhotonView>().RequestOwnership();
                GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
            }
        }

        public void OnOwnershipRequest(PhotonView targetView, Photon.Realtime.Player requestingPlayer)
        {
            if (requestingPlayer.UserId != PhotonNetwork.LocalPlayer.UserId)
            {
                if (GetComponent(Type.GetType(currentSpell)))
                {
                    Destroy(GetComponent(Type.GetType(currentSpell)));
                    currentSpell = "";
                }
                this.interactable.attachedToHand.DetachObject(gameObject, restoreOriginalParent);
            }
        }

        public void OnOwnershipTransfered(PhotonView targetView, Photon.Realtime.Player previousOwner)
        {
            throw new System.NotImplementedException();
        }

    }
}