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
        private AudioSource source;
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
            source = GetComponent<AudioSource>();
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
            //float y = floored();
            //transform.position = new Vector3(transform.position.x, y, transform.position.z);
            characterController.center = new Vector3(hmdTransform.localPosition.x, characterController.center.y, hmdTransform.localPosition.z);
            characterController.Move(new Vector3(0, -100f, 0) * Time.deltaTime);
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
                        Vector3 thumbstickInput = new Vector3(ThumbstickInput.axis.x, 0, ThumbstickInput.axis.y);
                        if (thumbstickInput != Vector3.zero && !source.isPlaying)
                        {
                            source.Play();
                        }
                        else if (thumbstickInput == Vector3.zero && source.isPlaying)
                        {
                            source.Stop();
                        }
                        characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up));
                    }
                }
            }
            else
            {
                if (isMineOrLocal())
                {
                    Vector3 direction = this.hmdTransform.TransformDirection(new Vector3(ThumbstickInput.axis.x, 0, ThumbstickInput.axis.y));
                    Vector3 thumbstickInput = new Vector3(ThumbstickInput.axis.x, 0, ThumbstickInput.axis.y);
                    if (thumbstickInput != Vector3.zero && !source.isPlaying)
                    {
                        source.Play();
                    }
                    else if(thumbstickInput == Vector3.zero && source.isPlaying)
                    {
                        source.Stop();
                    }
                    characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up));

                    //if(characterController)
                }
            }
        }

        void TeleportLocomotion()
        {
            characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(new Vector3(0, 0, 0), Vector3.up));
            if (source.isPlaying)
            {
                source.Stop();
            }
        }

        private float floored()
        {
            RaycastHit hit;
            float distance = 100f;

            if (Physics.Raycast(characterController.center, Vector3.down, out hit, distance))
            {
                /*
                 * Set the target location to the location of the hit.
                 */
                Vector3 targetLocation = hit.point;
                /*
                 * Modify the target location so that the object is being perfectly aligned with the ground (if it's flat).
                 */
                //targetLocation += new Vector3(0, transform.localScale.y+groundAlignment, 0);
                /*
                 * Move the object to the target location.
                 */
                Debug.Log(targetLocation);
                return targetLocation.y;
            }

            return gameObject.transform.position.y;
        }
    }
}
