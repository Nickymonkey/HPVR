using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBasedOnPoints : MonoBehaviour
{
    public Transform PointA;
    public Transform PointB;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 IsolatedPointA = new Vector3(PointA.position.x, PointA.position.y, 0f);
        Vector3 IsolatedPointB = new Vector3(PointB.position.x, PointB.position.y, 0f);
        transform.rotation = Quaternion.LookRotation(IsolatedPointA - IsolatedPointB, Vector3.forward);

        //Vector3 IsolatedPointA = new Vector3(PointA.position.x, PointA.position.y, 0f);
        //Vector3 IsolatedPointB = new Vector3(PointB.position.x, PointB.position.y, 0f);
        //float angle = Vector3.Angle(IsolatedPointA - IsolatedPointB, Vector3.zero);
        //Quaternion.Angle(transform.rotation, )
        //Quaternion.
        //Debug.Log(angle);
        //transform.rotation = Vector3.
        //transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0f, 0f, 1f));
        //transform.rotation = Quaternion.LookRotation(new Vector3(PointA.position.x, PointA.position.y, 0f), new Vector3(PointB.position.x, PointB.position.y, 0f));
        //transform.RotateTowards(new Vector3(0f, 0f, angle));
    }
}
