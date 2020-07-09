using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

namespace HPVR
{
    public class PlayerManager_VR : MonoBehaviourPunCallbacks, IPunObservable
    {
        public static GameObject LocalPlayerInstance;
        public SteamVR_Action_Vector2 input;
        public float speed = 2.5f;

        private CharacterController characterController;
        private Camera camera;
        private AudioListener audioListener;

        // Start is called before the first frame update
        void Start()
        {
            setCamera();
            setAudioListener();
            characterController = GetComponent<CharacterController>();
            if (isConnectedOrLocal())
            {
                LocalPlayerInstance = gameObject;
            }
            DontDestroyOnLoad(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if (isConnectedOrLocal())
            {
                Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(input.axis.x, 0, input.axis.y));
                characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) - new Vector3(0, 9.81f, 0) * Time.deltaTime);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            throw new System.NotImplementedException();
        }

        void setCamera()
        {
            if (GetComponentInChildren<Camera>() != null)
            {
                camera = GetComponentInChildren<Camera>();
                camera.enabled = isConnectedOrLocal();
            }
        }

        void setAudioListener()
        {
            if (GetComponentInChildren<AudioListener>() != null)
            {
                audioListener = GetComponentInChildren<AudioListener>();
                audioListener.enabled = isConnectedOrLocal();
            }
        }

        bool isConnectedOrLocal()
        {
            return photonView.IsMine || PhotonNetwork.InRoom == false;
        }
    }
}
