using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;


namespace HPVR
{
    public class Player_VR : Player
    {

        public static GameObject LocalPlayerInstance;
        public SteamVR_Action_Vector2 ThumbstickInput;
        public SteamVR_Action_Boolean ButtonInput;
        public float speed = 2.5f;
        public GameObject snapTurn;
        public GameObject inputModule;
        public GameObject steamVRIntializer;

        private CharacterController characterController;
        //-------------------------------------------------
        // Singleton instance of the Player. Only one can exist at a time.
        //-------------------------------------------------
        private static Player_VR _instance;


        //-------------------------------------------------
        private void Awake()
        {
            if (trackingOriginTransform == null)
            {
                trackingOriginTransform = this.transform;
            }
        }

        //-------------------------------------------------
        private IEnumerator Start()
        {
            DontDestroyOnLoad(gameObject);
            characterController = GetComponent<CharacterController>();

            if (isMineOrLocal())
            {
                _instance = this;
                LocalPlayerInstance = gameObject;
            }
            else
            {
                audioListener.gameObject.SetActive(false);
                this.gameObject.GetComponent<CharacterController>().enabled = false;
                if (steamVRIntializer != null)
                {
                    steamVRIntializer.SetActive(false);
                }
                if (snapTurn != null)
                {
                    snapTurn.SetActive(false);
                }
                if (inputModule != null)
                {
                    inputModule.SetActive(false);
                }
                this.enabled = false;
            }

            while (SteamVR.initializedState == SteamVR.InitializedStates.None || SteamVR.initializedState == SteamVR.InitializedStates.Initializing)
                yield return null;

            ActivateRig(rigSteamVR);
                //= ButtonInput.GetStateDown(Hand.);
        }
        //-------------------------------------------------
        private void ActivateRig(GameObject rig)
        {
            rigSteamVR.SetActive(rig == rigSteamVR);

            if (audioListener)
            {
                audioListener.transform.parent = hmdTransform;
                audioListener.transform.localPosition = Vector3.zero;
                audioListener.transform.localRotation = Quaternion.identity;
            }
           
        }

        // Update is called once per frame
        private void Update()
        {
            if (SteamVR.initializedState != SteamVR.InitializedStates.InitializeSuccess)
                return;

            bool buttonPressedDown = false;

            for (int i = 0; i < hands.Length; i++)
            {
                if (!buttonPressedDown)
                {
                    buttonPressedDown = ButtonInput.GetStateDown(hands[i].handType);
                }
            }

            if (buttonPressedDown)
            {
                GameState.Instance.ToggleLocomotionType();
            }

            movementUpdate();
        }

        bool isMineOrLocal()
        {
            bool photonViewIsMine = GetComponent<PhotonView>().IsMine;
            return photonViewIsMine || PhotonNetwork.InRoom == false;
        }

        void movementUpdate()
        {
            characterController.center = new Vector3(hmdTransform.localPosition.x, characterController.center.y, hmdTransform.transform.localPosition.z);
            characterController.Move(new Vector3(0, -9.81f, 0) * Time.deltaTime * 10f);
            if (GameState.Instance.locomotion == "Smooth")
            {
                SmoothLocomotion();
            }
            else if(GameState.Instance.locomotion == "Teleport")
            {
                TeleportLocomotion();
            }
        }

        void SmoothLocomotion()
        {
            if (NetworkedGameManager.Instance != null)
            {
                if (NetworkedGameManager.Instance.matchStarted)
                {
                    if (isMineOrLocal())
                    {
                        Vector3 direction = this.hmdTransform.TransformDirection(new Vector3(ThumbstickInput.axis.x, 0, ThumbstickInput.axis.y));
                        characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up));
                    }
                }
            }
            else
            {
                if (isMineOrLocal())
                {
                    Vector3 direction = this.hmdTransform.TransformDirection(new Vector3(ThumbstickInput.axis.x, 0, ThumbstickInput.axis.y));
                    characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up));
                }
            }
        }

        void TeleportLocomotion()
        {

        }
    }
}
