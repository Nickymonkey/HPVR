﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressurePlate : MonoBehaviour
{
    public bool RequiresPerson = false;
    public bool RequiresStone = false;
    public bool requiresSpecific = false;
    public bool triggered = false;
    public bool collisionStay = false;
    public int numOfCollisions = 0;
    public Collision currCollission;
    public string specificName = "";

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
        if (!collisionStay && currCollission != null)
        {
            if (RequiresPerson)
            {
                if (currCollission.collider.gameObject.layer == 8)
                {
                    activated();
                }
            }
            else if (RequiresStone)
            {
                if (currCollission.transform.gameObject.GetComponent<Rigidbody>() != null)
                {
                    if (currCollission.transform.gameObject.GetComponent<Rigidbody>().mass == 5)
                    {
                        activated();
                    }
                }
            }
            else if (requiresSpecific)
            {
                if (currCollission.gameObject.name.Contains(specificName)){
                    activated();
                }
            }
            else
            {
                activated();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        currCollission = collision;
        if (RequiresPerson)
        {
            if (currCollission.collider.gameObject.layer == 8)
            {
                activated();
            }
        }
        else if (RequiresStone)
        {
            if (currCollission.transform.gameObject.GetComponent<Rigidbody>() != null)
            {
                if (currCollission.transform.gameObject.GetComponent<Rigidbody>().mass == 5)
                {
                    activated();
                }
            }
        }
        else if (requiresSpecific)
        {
            if (currCollission.gameObject.name.Contains(specificName))
            {
                activated();
            }
        }
        else
        {
            activated();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (!collisionStay)
        {
            if (RequiresPerson)
            {
                if (currCollission.collider.gameObject.layer == 8)
                {
                    activated();
                }
            }
            else if (RequiresStone)
            {
                if (currCollission.transform.gameObject.GetComponent<Rigidbody>() != null)
                {
                    if (currCollission.transform.gameObject.GetComponent<Rigidbody>().mass == 5)
                    {
                        activated();
                    }
                }
            }
            else if (requiresSpecific)
            {
                if (currCollission.gameObject.name.Contains(specificName))
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
        deactivated();
        if(currCollission != null)
        {
            if (currCollission.transform == collision.transform)
            {
                currCollission = null;
            }
        }
    }

    private void activated()
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

    private void deactivated()
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

    public void check()
    {
        if (collisionStay)
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.green;
        }
        else
        {
            this.gameObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }
}
