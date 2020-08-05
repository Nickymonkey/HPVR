﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portcullis : MonoBehaviour
{
    public float OpenPosition;
    private float ClosePosition;
    //public float StartPosition;
    public float timeToLerp;
    public int triggersNeeded = -1;
    public int currentNumTriggers = 0;

    // Start is called before the first frame update
    void Start()
    {
        ClosePosition = transform.position.y;
        //StartCoroutine(LerpPosition(new Vector3(transform.position.x, transform.position.y, transform.position.z), timeToLerp));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

    public void Open()
    {
        StopAllCoroutines();
        StartCoroutine(LerpPosition(new Vector3(transform.position.x, ClosePosition+OpenPosition, transform.position.z), timeToLerp));
    }

    public void Close()
    {
        StopAllCoroutines();
        StartCoroutine(LerpPosition(new Vector3(transform.position.x, ClosePosition, transform.position.z), timeToLerp));
    }

    public void AddTrigger()
    {
        currentNumTriggers++;
        if (currentNumTriggers == triggersNeeded)
        {
            Open();
        }
    }

    public void RemoveTrigger()
    {
        currentNumTriggers--;        
        Close();
    }
}