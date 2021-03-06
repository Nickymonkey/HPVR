﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponentInChildren<ParticleSystem>().IsAlive())
        {
            if (PhotonNetwork.InRoom || PhotonNetwork.InLobby)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}
