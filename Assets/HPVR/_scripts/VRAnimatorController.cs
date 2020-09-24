using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRAnimatorController : MonoBehaviour
{
    public float speedThreshold = 0.1f;
    [Range(0,1)]
    public float smoothing = 1f;
    private Animator animator;
    private Vector3 previousPos;
    private VRRig vrRig;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        vrRig = GetComponent<VRRig>();
        previousPos = vrRig.head.vrTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(vrRig.head.vrTarget != null)
        {
            Vector3 headsetSpeed = (vrRig.head.vrTarget.position - previousPos) / Time.deltaTime;
            //Debug.Log(headsetSpeed);
            headsetSpeed.y = 0;

            previousPos = vrRig.head.vrTarget.position;
            //set animator values
            animator.SetBool("isMoving", headsetSpeed.magnitude > speedThreshold);

            float previousDirectionX = animator.GetFloat("DirectionX");
            float previousDirectionY = animator.GetFloat("DirectionY");

            if (!float.IsNaN(headsetSpeed.x))
            {
                animator.SetFloat("DirectionX", Mathf.Lerp(previousDirectionX, Mathf.Clamp(headsetSpeed.x, -1, 1), smoothing));
            }

            if (!float.IsNaN(headsetSpeed.z))
            {
                animator.SetFloat("DirectionY", Mathf.Lerp(previousDirectionY, Mathf.Clamp(headsetSpeed.z, -1, 1), smoothing));
            }
        }
    }
}
