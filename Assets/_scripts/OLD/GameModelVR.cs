using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModelVR : MonoBehaviourPunCallbacks
{
    public static GameModelVR Instance { get; private set; }
    public GameObject currentPlayer;
    public GameObject VRPlayerPrefab;
    public GameObject nonVRPlayerPrefab;
    public GameObject prefabToInstantiate;
    public bool useNonVRPrefabInEditor = true;

    private void Awake()
    {
        if (Application.isEditor && useNonVRPrefabInEditor)
        {
            prefabToInstantiate = nonVRPlayerPrefab;
        }
        else
        {
            prefabToInstantiate = VRPlayerPrefab;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BuildPlayerForLobby()
    {

    }

    public void BuildPlayerForMultiplayer()
    {

    }
}
