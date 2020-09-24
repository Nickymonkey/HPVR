using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    [RequireComponent(typeof(Interactable))]
    public class Portkey : MonoBehaviour
    {

        public string location = "";
        public string lobby = "";
        public string level = "";
        private TextMesh generalText;
        private TextMesh hoveringText;
        private Vector3 oldPosition;
        private Quaternion oldRotation;
        public bool networkedConnect = false;
        public bool networkedDisconnect = false;
        public bool localConnect = false;

        private float attachTime;

        private Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);

        private Interactable interactable;

        //-------------------------------------------------
        void Awake()
        {
            var textMeshs = GetComponentsInChildren<TextMesh>();
            generalText = textMeshs[0];
            hoveringText = textMeshs[1];

            generalText.text = "";
            hoveringText.text = "";

            interactable = this.GetComponent<Interactable>();
        }


        //-------------------------------------------------
        // Called when a Hand starts hovering over this object
        //-------------------------------------------------
        private void OnHandHoverBegin(Hand hand)
        {
            generalText.text = "Travel to: " + location;
        }

        //-------------------------------------------------
        // Called when a Hand stops hovering over this object
        //-------------------------------------------------
        private void OnHandHoverEnd(Hand hand)
        {
            generalText.text = "";
        }


        //-------------------------------------------------
        // Called every Update() while a Hand is hovering over this object
        //-------------------------------------------------
        private void HandHoverUpdate(Hand hand)
        {
            GrabTypes startingGrabType = hand.GetGrabStarting();
            bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

            if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
            {
                // Save our position/rotation so that we can restore it when we detach
                oldPosition = transform.position;
                oldRotation = transform.rotation;

                // Call this to continue receiving HandHoverUpdate messages,
                // and prevent the hand from hovering over anything else
                hand.HoverLock(interactable);

                // Attach this object to the hand
                hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
            }
            else if (isGrabEnding)
            {
                // Detach this object from the hand
                hand.DetachObject(gameObject);

                // Call this to undo HoverLock
                hand.HoverUnlock(interactable);

                // Restore position/rotation
                transform.position = oldPosition;
                transform.rotation = oldRotation;
            }
        }


        //-------------------------------------------------
        // Called when this GameObject becomes attached to the hand
        //-------------------------------------------------
        private void OnAttachedToHand(Hand hand)
        {
            if (networkedConnect)
            {
                Destroy(Launcher.LocalPlayerInstance);
                GameState.Instance.lobbyToLoad = lobby;
                GameState.Instance.levelToLoad = level;
                NetworkManager.Instance.Connect();
            } else if (networkedDisconnect)
            {
                PhotonNetwork.Destroy(GameManager.LocalPlayerInstance);
                NetworkManager.Instance.LeaveRoom();
            }
            else if (localConnect)
            {
                Destroy(Launcher.LocalPlayerInstance);
                SceneManager.LoadScene(lobby, LoadSceneMode.Single);
            }
            //generalText.text = string.Format("Attached: {0}", hand.name);
            //attachTime = Time.time;
        }



        //-------------------------------------------------
        // Called when this GameObject is detached from the hand
        //-------------------------------------------------
        private void OnDetachedFromHand(Hand hand)
        {
            //generalText.text = string.Format("Detached: {0}", hand.name);
        }


        //-------------------------------------------------
        // Called every Update() while this GameObject is attached to the hand
        //-------------------------------------------------
        private void HandAttachedUpdate(Hand hand)
        {
            //generalText.text = string.Format("Attached: {0} :: Time: {1:F2}", hand.name, (Time.time - attachTime));
        }

        private bool lastHovering = false;
        private void Update()
        {
            if (interactable.isHovering != lastHovering) //save on the .tostrings a bit
            {
                //hoveringText.text = string.Format("Hovering: {0}", interactable.isHovering);
                //lastHovering = interactable.isHovering;
            }
        }


        //-------------------------------------------------
        // Called when this attached GameObject becomes the primary attached object
        //-------------------------------------------------
        private void OnHandFocusAcquired(Hand hand)
        {
        }


        //-------------------------------------------------
        // Called when another attached GameObject becomes the primary attached object
        //-------------------------------------------------
        private void OnHandFocusLost(Hand hand)
        {
        }    
    }
}