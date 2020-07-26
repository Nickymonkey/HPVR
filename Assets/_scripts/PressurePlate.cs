using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public bool RequiresPerson = false;
    public bool triggered = false;
    [Tooltip("If pressure plate triggered")]
    public UnityEvent onTrigger;

    [Tooltip("If pressure plate untriggered")]
    public UnityEvent offTrigger;
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
        if(!triggered)
        {
            if (RequiresPerson)
            {
                if (collision.collider.gameObject.layer == 8)
                {
                    activated();
                }
            }
            else
            {
                activated();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (triggered)
        {
            deactivated();
        }
    }

    void activated()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.green;
        triggered = true;
        onTrigger.Invoke();
    }

    void deactivated()
    {
        this.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        triggered = false;
        offTrigger.Invoke();
    }
}
