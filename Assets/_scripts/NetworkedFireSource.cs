using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class NetworkedFireSource : FireSource
{
    //-------------------------------------------------
    void Start()
    {
        if (startActive)
        {
            StartBurning();
        }
    }
    //-------------------------------------------------
    protected new void StartBurning()
    {
        if (isDisabled)
        {
            return;
        }

        isBurning = true;
        ignitionTime = Time.time;

        // Play the fire ignition sound if there is one
        if (ignitionSound != null)
        {
            ignitionSound.Play();
        }

        if (customParticles != null)
        {
            customParticles.Play();
        }
        else
        {
            if (fireParticlePrefab != null)
            {
                if(PhotonNetwork.InRoom || PhotonNetwork.InLobby)
                {
                    fireObject = PhotonNetwork.Instantiate(fireParticlePrefab.name, transform.position, transform.rotation);
                    Debug.Log("Networked");
                }
                else
                {
                    fireObject = Instantiate(fireParticlePrefab, transform.position, transform.rotation) as GameObject;
                    Debug.Log("Local");
                }
                //fireObject = Instantiate(fireParticlePrefab, transform.position, transform.rotation) as GameObject;
                fireObject.transform.parent = transform;
            }
        }
    }
}
