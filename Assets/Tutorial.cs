using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public int IsFirst;

    void Start()
    {
        IsFirst = PlayerPrefs.GetInt("IsFirst");
        if (IsFirst == 0)
        {
            //Do stuff on the first time
            Debug.Log("first run");
            PlayerPrefs.SetInt("IsFirst", 1);
        }
        else
        {
            //Do stuff other times
            Debug.Log("welcome again!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
