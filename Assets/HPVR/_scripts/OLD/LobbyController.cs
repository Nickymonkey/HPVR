using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviour
{
    public GameObject VRPlayerPrefab;
    public GameObject nonVRPlayerPrefab;
    public bool useNonVRPrefab = true;
    public NetworkControllerVR NetworkControllerVR;
    private GameObject prefabToInstantiate;

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
        Instantiate(prefabToInstantiate, new Vector3(0f, 5f, 0f), Quaternion.identity);
    }

    public void StartMultiplayer()
    {
        NetworkControllerVR.Connect();
    }

    public void LeaveMultiplayer()
    {
        NetworkControllerVR.LeaveRoom();
    }
}
