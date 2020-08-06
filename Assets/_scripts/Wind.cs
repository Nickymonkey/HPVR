using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public enum Axis_t
    {
        XAxis,
        YAxis,
        ZAxis
    };

    public Fan fs;
    public float windStrength = -50f;
    public Axis_t axisOfRotation = Axis_t.XAxis;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("triggered");
        if (other.transform.parent.GetComponent<Rigidbody>() && fs.enabled)
        {
            //Debug.Log("found rigidbody");
            //if(axisOfRotation)
            if (axisOfRotation == Axis_t.XAxis)
            {
                other.transform.parent.GetComponent<Rigidbody>().AddForce(new Vector3(windStrength, 0, 0));
            } else if(axisOfRotation == Axis_t.YAxis)
            {
                other.transform.parent.GetComponent<Rigidbody>().AddForce(new Vector3(0, windStrength, 0));
            } else if(axisOfRotation == Axis_t.ZAxis)
            {
                other.transform.parent.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, windStrength));
            }         
        }
    }
}
