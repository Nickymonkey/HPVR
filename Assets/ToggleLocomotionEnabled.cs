using HPVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleLocomotionEnabled : MonoBehaviour
{
    public string locomotionType;

    private void OnEnable()
    {
        if (GameState.Instance.locomotion.Contains(locomotionType))
        {
            GetComponent<Toggle>().isOn = true;
        }
        else
        {
            GetComponent<Toggle>().isOn = false;
        }
    }

}
