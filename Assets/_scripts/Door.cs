using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Door : MonoBehaviour
{
    private Interactable Interactable;
    private AudioSource source;
    private CircularDrive drive;
    // Start is called before the first frame update
    void Start()
    {
        drive = GetComponent<CircularDrive>();
        source = GetComponent<AudioSource>();
        Interactable = GetComponent<Interactable>();
        //teractable.onAttachedToHand += OnAttachedToHand;
        //ive.driving
        //drive.
        //Interactable.on
        //interact.onAttachedToHand += OnAttachedToHand();
        //interact.onAttachedToHand += doorGrabbed();
        //drive.
    }

    //private void OnAttachedToHand(Hand hand)
    //{
    //    Debug.Log("Grabbed door");
    //    if (!drive.rotateGameObject)
    //    {
    //        source.Play();
    //    }
    //}

    //private void OnAttachedToHand(Hand attachedHand)
    //{
    //    Debug.Log("Grabbed door");
    //    if (!drive.rotateGameObject)
    //    {
    //        source.Play();
    //    }
    //}

    //-------------------------------------------------
    //private void OnHandFocusAcquired(Hand hand)
    //{
    //    //gameObject.SetActive(true);
    //    OnAttachedToHand(hand);
    //}

    //private Interactable.OnAttachedToHandDelegate OnAttachedToHand()
    //{
    //    Debug.Log("Grabbed door");
    //    if (!drive.rotateGameObject)
    //    {
    //        source.Play();
    //    }     
    //    //return null;
    //}

    // Update is called once per frame
    void Update()
    {
        
    }

    private GrabTypes grabbedWithType;
    //-------------------------------------------------
    private void HandHoverUpdate(Hand hand)
    {
        GrabTypes startingGrabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabbingWithType(grabbedWithType) == false;

        if ((grabbedWithType == GrabTypes.None && startingGrabType != GrabTypes.None) && !source.isPlaying)
        {
            Debug.Log("Grabbed door");
            if (!drive.rotateGameObject)
            {
                source.Play();
            }
        }
    }
}
