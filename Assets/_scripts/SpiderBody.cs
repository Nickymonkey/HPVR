using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBody : MonoBehaviour
{
    public float speed = .1f;
    public float bodyOffset = 2.0f;
    public List<Transform> legs;
    //private Vector3 PreviousVector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float averageY = 0f;
        foreach(Transform leg in legs)
        {
            averageY += leg.position.y;
        }
        averageY = averageY / legs.Count;
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, bodyOffset+averageY, transform.position.z), speed);
        //Vector3 IsolatedPointA = new Vector3(legs[0].position.x, legs[0].position.y, 0f);
        //Vector3 IsolatedPointB = new Vector3(legs[1].position.x, legs[1].position.y, 0f);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(IsolatedPointA - IsolatedPointB, Vector3.forward), speed);
        //transform.rotation = Quaternion.LookRotation(IsolatedPointA - IsolatedPointB, Vector3.forward);
        //float currentHeight = transform.position.y;
        //Vector3 SecondVector = legs[0].position - legs[1].position;
        //float angleBetweenThem = Vector3.Angle(new Vector3(0f, legs[0].position.y, 0f), new Vector3(0f, legs[1].position.y, 0f));
        //Debug.Log(angleBetweenThem);
        //transform.rotation = Quaternion.ro
        //transform.rotation = Quaternion.LookRotation(legs[0].position - legs[1].position, Vector3.forward);
    }
}
