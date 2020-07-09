using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Photon.Pun;
using Valve.VR;
using System;
using System.Collections.ObjectModel;

namespace HPVR
{
    public class NetworkedHand : Hand
    {

        public GameObject NetworkedHandRepresentation;
        public NetworkedVisibility networkedVisibility;

        //bool handVisible = true;

        //-------------------------------------------------
        protected new virtual void Awake()
        {
            //Debug.Log("AWAKE");
            if (!isMineOrLocal())
            {
                //this.gameObject.AddComponent<RemoteHand>();
                if (NetworkedHandRepresentation != null)
                {
                    NetworkedHandRepresentation.SetActive(true);
                }

                //FIGURE THIS OUT BECAUSE IF ITS DISABLED IT CANT SEND MESSAGES
                this.enabled = false;
            }

            inputFocusAction = SteamVR_Events.InputFocusAction(OnInputFocus);

            if (hoverSphereTransform == null)
                hoverSphereTransform = this.transform;

            if (objectAttachmentPoint == null)
                objectAttachmentPoint = this.transform;

            applicationLostFocusObject = new GameObject("_application_lost_focus");
            applicationLostFocusObject.transform.parent = transform;
            applicationLostFocusObject.SetActive(false);

            if (trackedObject == null)
            {
                trackedObject = this.gameObject.GetComponent<SteamVR_Behaviour_Pose>();

                if (trackedObject != null)
                    trackedObject.onTransformUpdatedEvent += OnTransformUpdated;
            }
        }

        //-------------------------------------------------
        private void InitController()
        {

            bool hadOldRendermodel = mainRenderModel != null;
            EVRSkeletalMotionRange oldRM_rom = EVRSkeletalMotionRange.WithController;
            if (hadOldRendermodel)
                oldRM_rom = mainRenderModel.GetSkeletonRangeOfMotion;

            foreach (RenderModel r in renderModels)
            {
                if (r != null)
                    Destroy(r.gameObject);
            }

            renderModels.Clear();
            GameObject renderModelInstance;

            if (GetComponent<PhotonView>().IsMine)
            {
                renderModelInstance = PhotonNetwork.Instantiate(this.renderModelPrefab.name, new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
            }
            else
            {
                renderModelInstance = GameObject.Instantiate(renderModelPrefab);
            }
            renderModelInstance.layer = gameObject.layer;
            renderModelInstance.tag = gameObject.tag;
            renderModelInstance.transform.parent = this.transform;
            renderModelInstance.transform.localPosition = Vector3.zero;
            renderModelInstance.transform.localRotation = Quaternion.identity;
            renderModelInstance.transform.localScale = renderModelPrefab.transform.localScale;

            //TriggerHapticPulse(800);  //pulse on controller init

            int deviceIndex = trackedObject.GetDeviceIndex();

            mainRenderModel = renderModelInstance.GetComponent<RenderModel>();
            renderModels.Add(mainRenderModel);

            if (hadOldRendermodel)
                mainRenderModel.SetSkeletonRangeOfMotion(oldRM_rom);

            this.BroadcastMessage("SetInputSource", handType, SendMessageOptions.DontRequireReceiver); // let child objects know we've initialized
            this.BroadcastMessage("OnHandInitialized", deviceIndex, SendMessageOptions.DontRequireReceiver); // let child objects know we've initialized
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

        public override void SetVisibility(bool visible)
        {
            networkedVisibility.handVisible = visible;
            if (mainRenderModel != null)
                mainRenderModel.SetVisibility(visible);
        }
    }
}