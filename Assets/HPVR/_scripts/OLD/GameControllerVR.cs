using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerVR : MonoBehaviourPunCallbacks
{
    public static GameControllerVR Instance { get; private set; }
    public GameObject VRPlayerPrefab;
    public GameObject nonVRPlayerPrefab;
    public NetworkControllerVR networkControllerVR;
    public bool useNonVRPrefab = true;

    private GameObject prefabToInstantiate;
    public GameObject CurrentPlayer;

    private void Awake()
    {
        if (useNonVRPrefab)
        {
            prefabToInstantiate = nonVRPlayerPrefab;
        }
        else
        {
            prefabToInstantiate = VRPlayerPrefab;
        }
    }

    private void Start()
    {
        Instance = this;
        BuildPlayer();
    }


    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            StartMultiplayer();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LeaveMultiplayer();
        }
    }

    public void BuildPlayer()
    {
        if(FirstPersonPlayerManager.LocalPlayerInstance == null)
        {
            PhotonNetwork.Instantiate(this.prefabToInstantiate.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        }
        //if (Instance.CurrentPlayer != null)
        //{
        //{
        //    PhotonNetwork.Destroy(Instance.CurrentPlayer);
        //}
        //Instance.CurrentPlayer = PhotonNetwork.Instantiate(this.prefabToInstantiate.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0) as GameObject;
    }

    public void StartMultiplayer()
    {
        networkControllerVR.Connect();
    }

    public void LeaveMultiplayer()
    {
        networkControllerVR.LeaveRoom();
    }

}
