using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixFoot : MonoBehaviour
{
    public float distance = 100f;
    //public float groundAlignment;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down, out hit, distance))
        {
            /*
             * Set the target location to the location of the hit.
             */
            Vector3 targetLocation = hit.point;
            /*
             * Modify the target location so that the object is being perfectly aligned with the ground (if it's flat).
             */
            //targetLocation += new Vector3(0, transform.localScale.y+groundAlignment, 0);
            /*
             * Move the object to the target location.
             */
            transform.position = targetLocation;
        }
    }
}
