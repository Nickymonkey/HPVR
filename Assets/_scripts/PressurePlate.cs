using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public bool RequiresPerson = false;
    public bool triggered = false;
    private bool collisionStay = false;
    public int numOfCollisions = 0;

    [Tooltip("If pressure plate triggered")]
    public UnityEvent onTrigger;

    [Tooltip("If pressure plate untriggered")]
    public UnityEvent offTrigger;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
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

    private void OnCollisionExit(Collision collision)
    {
        deactivated();
    }

    void activated()
    {
        numOfCollisions++;
        //triggered = true;
        if (!collisionStay)
        {
            collisionStay = true;
            this.gameObject.GetComponent<Renderer>().material.color = Color.green;
            onTrigger.Invoke();
        }
    }

    void deactivated()
    {
        numOfCollisions--;
        if (numOfCollisions <= 0 && collisionStay)
        {
            collisionStay = false;
            this.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
            //triggered = false;
            offTrigger.Invoke();
        }
    }
}
