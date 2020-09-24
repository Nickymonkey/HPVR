using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableSpawn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "hand")
        {
            Debug.Log("OnTriggerEnter");
        }
        //Debug.Log("OnTriggerEnter");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "hand")
        {
            Debug.Log("OnTriggerExit");
        }
        //Debug.Log("OnTriggerExit");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
