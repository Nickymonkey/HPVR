using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveJointPole : MonoBehaviour
{
    public Transform BodyTarget;
    public float distanceThreshold = 2f;
    public float speed = .1f;
    public float legOffset = 1.5f;
    //private float upOffset = .5f;
    public bool legGrounded = true;
    public bool legComingDown = false;
    public List<MoveJointPole> oppositeLegs;
    //private Vector3 PositionToMoveTo;
    //private Vector3 JointTargetComponent;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 BodyTargetComponent = new Vector3(BodyTarget.position.x, BodyTarget.position.y + legOffset, BodyTarget.position.z);
        Vector3 JointTargetComponent = new Vector3(transform.position.x, BodyTarget.position.y + legOffset, transform.position.z);
       // Debug.Log(Vector3.Distance(BodyTargetComponent, JointTargetComponent));
        //Debug.Log(takeStep);
        if ((Vector3.Distance(BodyTargetComponent, JointTargetComponent) > distanceThreshold && (legGrounded == true)) && oppositeLegsGrounded())
        {
            legGrounded = false;
        }

        if (!legGrounded)
        {
            if (Vector3.Distance(BodyTargetComponent, JointTargetComponent) > (distanceThreshold / 2.0f))
            {
                Vector3 legUp = new Vector3(BodyTargetComponent.x, (BodyTargetComponent.y + legOffset), BodyTargetComponent.z);
                transform.position = Vector3.MoveTowards(transform.position, legUp, speed);
            } else if (Vector3.Distance(BodyTargetComponent, JointTargetComponent) > 0.1f)
            {
                legComingDown = true;
                transform.position = Vector3.MoveTowards(transform.position, BodyTargetComponent, speed);
            }
            else
            {
                legGrounded = true;
                legComingDown = false;
            }
        }
    }

    bool oppositeLegsGrounded()
    {
        foreach(MoveJointPole leg in oppositeLegs)
        {
            if (!leg.legGrounded && !leg.legComingDown)
            {
                return false;
            }
        }
        return true;
    }
}
