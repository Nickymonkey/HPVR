using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorUnityEventHandler : MonoBehaviour
{

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void setBoolTrue(string boolName){

        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            animator.SetBool(parameter.name, false);
        }

        animator.SetBool(boolName, true);
    }

    public void setBoolFalse(string boolName)
    {
        animator.SetBool(boolName, false);
    }

    public void debug()
    {
        Debug.Log("TEST");
    }
}
