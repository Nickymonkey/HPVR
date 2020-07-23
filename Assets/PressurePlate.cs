using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
    public bool RequiresPerson = false;
    public bool triggered = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay(Collision collision)
    {
        if (RequiresPerson)
        {
            if(collision.collider.gameObject.layer == 8)
            {
                activated();
            }
        }
        else
        {
            activated();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        deactivated();
    }

    void activated()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;
        triggered = true;
    }

    void deactivated()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        triggered = false;
    }
}
