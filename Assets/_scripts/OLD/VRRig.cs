using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VRMap
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;

    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
    }
}
public class VRRig : MonoBehaviour
{
    public VRMap head;
    public VRMap leftHand;
    public VRMap rightHand;
    public float turnSmoothness;
    public Transform headConstraint;
    public Vector3 headBodyOffset;
    public float thresholdAngle;
    private float currentThresholdAngle;
    // Start is called before the first frame update
    void Start()
    {
        headBodyOffset = transform.position - headConstraint.position;
        currentThresholdAngle = thresholdAngle;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = headConstraint.position + headBodyOffset;
        var angle = Vector3.Angle(transform.forward, Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized);
        //Debug.Log(headConstraint.up.y);
        if ((angle > currentThresholdAngle) && headConstraint.up.y >= -0.5f)
        {
            //currentThresholdAngle = 5f;
            transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized, Time.deltaTime * turnSmoothness);
        }
        //else if ((currentThresholdAngle != thresholdAngle) && Math.Abs(headConstraint.localRotation.z) <= 130.0f)
        //{
        //    currentThresholdAngle = thresholdAngle;
        //}
        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}
