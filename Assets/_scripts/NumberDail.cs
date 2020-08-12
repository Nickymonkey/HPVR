﻿using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NumberDail : MonoBehaviour
{
    public int correctNum = 0;
    public int currentNum = 1;
    public UnityEvent onCorrectNum;
    public UnityEvent onIncorrectNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        try
        {
            int newNum = Int32.Parse(other.gameObject.name);
            if(currentNum != newNum)
            {
                if (GameObject.Find("[NetworkedCo-OpGameManager](Clone)"))
                {
                    GameObject.Find("[NetworkedCo-OpGameManager](Clone)").GetComponent<PhotonView>().RequestOwnership();
                }

                if (currentNum == correctNum)
                {
                    onIncorrectNum.Invoke();
                }

                currentNum = newNum;

                if (currentNum == correctNum)
                {
                    onCorrectNum.Invoke();
                }
            }
        }
        catch (FormatException e)
        {
            Console.WriteLine(e.Message);
        }
    }

}