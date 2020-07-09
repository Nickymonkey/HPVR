using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkedScriptDisabler : MonoBehaviour
{
    public MonoBehaviour[] scripts;
    // Start is called before the first frame update
    void Start()
    {
        if (!isMineOrLocal())
        {
            foreach(MonoBehaviour script in scripts)
            {
                script.enabled = false;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool isMineOrLocal()
    {
        bool photonViewIsMine = GetComponent<PhotonView>().IsMine;
        return photonViewIsMine || PhotonNetwork.InRoom == false;
    }
}
