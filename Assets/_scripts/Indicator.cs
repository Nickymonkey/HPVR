using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    public Material OnMaterial;
    public Material OffMaterial;

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
        GetComponent<Light>().color = Color.green;
        GetComponent<Renderer>().sharedMaterial = OnMaterial;
        //GetComponent<L>
    }

    public void SwitchOff()
    {
        GetComponent<Light>().color = Color.red;
        GetComponent<Renderer>().sharedMaterial = OffMaterial;
    }
}
