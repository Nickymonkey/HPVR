using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Indicator : MonoBehaviour
{
    public Material OnMaterial;
    public Material OffMaterial;
    public bool isOn = false;
    public UnityEvent switchOn;
    public UnityEvent switchOff;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchOn()
    {
        switchOn.Invoke();
        GetComponent<Light>().color = Color.green;
        GetComponent<Renderer>().sharedMaterial = OnMaterial;
        isOn = true;
        //GetComponent<L>
    }

    public void SwitchOff()
    {
        switchOff.Invoke();
        GetComponent<Light>().color = Color.red;
        GetComponent<Renderer>().sharedMaterial = OffMaterial;
        isOn = false;
    }

    public void checkSwitch()
    {
        if (isOn)
        {
            SwitchOn();
        }
        else
        {
            SwitchOff();
        }
    }

    public void flip()
    {
        if (GameObject.Find("[NetworkedCo-OpGameManager](Clone)"))
        {
            GameObject.Find("[NetworkedCo-OpGameManager](Clone)").GetComponent<PhotonView>().RequestOwnership();
        }

        if (isOn)
        {
            SwitchOff();
        }
        else
        {
            SwitchOn();
        }
    }
}
