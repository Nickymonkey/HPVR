using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public Fan fs;
    public float windStrength = -50f;
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
            other.transform.parent.GetComponent<Rigidbody>().AddForce(new Vector3(windStrength, 0, 0));
        }
    }
}
