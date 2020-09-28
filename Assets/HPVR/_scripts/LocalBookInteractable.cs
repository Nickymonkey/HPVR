using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using UnityEngine.Events;

namespace HPVR
{
    //-------------------------------------------------------------------------
    [RequireComponent(typeof(Interactable))]
    public class LocalBookInteractable : MonoBehaviour
    {

        [Tooltip("When detaching the object, should it return to its original parent?")]
        public bool restoreOriginalParent = false;

        private TextMesh generalText;
        private TextMesh hoveringText;
        private Vector3 oldPosition;
        private Quaternion oldRotation;
        public LayerMask defaultLayerMask;

        private float attachTime;

        public Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags & (~Hand.AttachmentFlags.SnapOnAttach) & (~Hand.AttachmentFlags.DetachOthers) & (~Hand.AttachmentFlags.VelocityMovement);
       
        private Interactable interactable;

        public UnityEvent onPickUp;
        public UnityEvent onDetachFromHand;
        public HandEvent onHeldUpdate;
        public bool attached = false;

        //-------------------------------------------------
        void Awake()
        {
            interactable = this.GetComponent<Interactable>();
        }


        //-------------------------------------------------
        // Called when a Hand starts hovering over this object
        //-------------------------------------------------
        private void OnHandHoverBegin(Hand hand)
        {

        }


        //-------------------------------------------------
        // Called when a Hand stops hovering over this object
        //-------------------------------------------------
        private void OnHandHoverEnd(Hand hand)
        {
        }


        //-------------------------------------------------
        // Called every Update() while a Hand is hovering over this object
        //-------------------------------------------------
        //private void HandHoverUpdate(Hand hand)
        //{

        //    GrabTypes startingGrabType = hand.GetGrabStarting();

        //    if (startingGrabType != GrabTypes.None)
        //    {
        //        hand.AttachObject(gameObject, startingGrabType, attachmentFlags);               
        //        hand.HideGrabHint();
        //    }
        //}


        //-------------------------------------------------
        // Called when this GameObject becomes attached to the hand
        //-------------------------------------------------
        private void OnAttachedToHand(Hand hand)
        {
            //hand = attachedHand;
            //Debug.Log("LOCAL BOOK INTERACTABlE: ATTACHED TO HAND");
            defaultLayerMask = hand.hoverLayerMask;
            string[] masks = new string[2];
            masks[0] = "Pickup";
            //masks[1] = "Page";
            LayerMask mask = LayerMask.GetMask(masks);
            //LayerMask pageMask = 
            mask &= ~(1 << this.gameObject.layer);
            hand.hoverLayerMask = mask;
            //hand.HoverLock(interactable);
            //attached = true;
            //onPickUp.Invoke();
        }



        //-------------------------------------------------
        // Called when this GameObject is detached from the hand
        //-------------------------------------------------
        private void OnDetachedFromHand(Hand hand)
        {
            hand.hoverLayerMask = defaultLayerMask;
            //attached = false;
            //onDetachFromHand.Invoke();

            //m_Recognizer.Dispose();
            //LexiconFocusManager.OnCaptureFocus -= CaptureFocus;
            //Destroy(pointerSphere);
            Destroy(gameObject);
        }


        //-------------------------------------------------
        // Called every Update() while this GameObject is attached to the hand
        //-------------------------------------------------
        //private void HandAttachedUpdate(Hand hand)
        //{
        //    //if(hand.)
        //    if (hand.IsGrabEnding(this.gameObject))
        //    {
        //        //Debug.Log("LOCAL BOOK INTERACTABLE: GRAB IS ENDING");
        //        hand.DetachObject(gameObject, restoreOriginalParent);

        //        // Uncomment to detach ourselves late in the frame.
        //        // This is so that any vehicles the player is attached to
        //        // have a chance to finish updating themselves.
        //        // If we detach now, our position could be behind what it
        //        // will be at the end of the frame, and the object may appear
        //        // to teleport behind the hand when the player releases it.
        //        //StartCoroutine( LateDetach( hand ) );
        //    }

        //    if (onHeldUpdate != null)
        //        onHeldUpdate.Invoke(hand);
        //}

        private bool lastHovering = false;

        private void Update()
        {
            if (interactable.isHovering != lastHovering) //save on the .tostrings a bit
            {
                lastHovering = interactable.isHovering;
            }
        }


        //-------------------------------------------------
        // Called when this attached GameObject becomes the primary attached object
        //-------------------------------------------------
        //-------------------------------------------------
        private void OnHandFocusAcquired(Hand hand)
        {
            gameObject.SetActive(true);
            OnAttachedToHand(hand);
        }


        //-------------------------------------------------
        // Called when another attached GameObject becomes the primary attached object
        //-------------------------------------------------
        private void OnHandFocusLost(Hand hand)
        {
            gameObject.SetActive(false);
        }
    }
}
