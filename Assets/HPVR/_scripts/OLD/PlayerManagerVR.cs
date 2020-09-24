using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManagerVR : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameObject LocalPlayerInstance;
    public GameObject VRCamera;
    public GameObject FollowHead;

    public void Awake()
    {
        // used in GameManager.cs: we keep track of the localPlayer instance to prevent instanciation when levels are synchronized
        if (instanceMine())
        {
            LocalPlayerInstance = gameObject;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if (!instanceMine())
        {
            VRCamera.SetActive(false);
            FollowHead.GetComponent<AudioListener>().enabled = false;
            return;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }

    public bool instanceMine()
    {
        return !((PhotonNetwork.IsConnected == true && PhotonNetwork.InRoom == true) && photonView.IsMine == false);
    }
}
