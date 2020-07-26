using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fan : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        transform.Rotate(300 * Time.deltaTime, 0, 0); //rotates 50 degrees per second around z axis
    }
}
