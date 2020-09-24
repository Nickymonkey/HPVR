using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverRotationStaircase : MonoBehaviour
{
    public float shiftInterval = 5.0f;
    public float duration = 1.0f;
    private Vector3 startAngle;
    public Vector3 otherAngle;
    private Vector3 currentAngle;
    //private Vector3 targetAngle;
    public bool atStart = true;

    // Start is called before the first frame update
    void Start()
    {
        startAngle = transform.rotation.eulerAngles;
        currentAngle = startAngle;
    }

    private IEnumerator LerpRotation(Vector3 targetAngle)
    {
        float time = 0;
        Vector3 currentPosition = transform.position;
        while (time < duration)
        {
            currentAngle = new Vector3(
    Mathf.LerpAngle(currentAngle.x, targetAngle.x, time / duration),
    Mathf.LerpAngle(currentAngle.y, targetAngle.y, time / duration),
    Mathf.LerpAngle(currentAngle.z, targetAngle.z, time / duration));

            transform.eulerAngles = currentAngle;
            time += Time.deltaTime;
            yield return null;
        }
    }

    public void shiftRotation()
    {
        StopAllCoroutines();
        Vector3 targetAngle;
        if (atStart)
        {
            atStart = false;
            targetAngle = otherAngle;
        }
        else
        {
            atStart = true;
            targetAngle = startAngle;
        }
        StartCoroutine(LerpRotation(targetAngle));
    }
}
