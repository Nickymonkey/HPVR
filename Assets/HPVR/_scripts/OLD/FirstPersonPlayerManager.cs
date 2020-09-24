using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public CharacterController controller;
    public float speed = 12f;
    public float mouseSensitivity = 100f;
    public static GameObject LocalPlayerInstance;

    private float xRotation = 0f;
    private Camera camera;
    private AudioListener audioListener;

    public void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        audioListener = GetComponentInChildren<AudioListener>();
        if (!instanceMine())
        {
            camera.enabled = false;
            audioListener.enabled = false;
        }
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instanciation when levels are synchronized

        // #Critical

        if (!instanceMine())
        {
            LocalPlayerInstance = gameObject;
        }
        // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
        DontDestroyOnLoad(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!instanceMine())
        {
            return;
        }
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime - new Vector3(0, 9.81f, 0) * Time.deltaTime);

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        camera.gameObject.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
    {
    }

    public bool instanceMine()
    {
        return !((PhotonNetwork.IsConnected == true && PhotonNetwork.InRoom == true) && photonView.IsMine == false);
    }
}
