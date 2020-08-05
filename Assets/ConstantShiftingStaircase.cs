using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantShiftingStaircase : MonoBehaviour
{
    public float shiftInterval = 5.0f;
    private Vector3 startAngle;
    public Vector3 otherAngle;
    private Vector3 currentAngle;
    private Vector3 targetAngle;

    // Start is called before the first frame update
    void Start()
    {
        startAngle = transform.rotation.eulerAngles;
        currentAngle = startAngle;
        StartCoroutine(Shift());
    }

    // Update is called once per frame
    void Update()
    {
        currentAngle = new Vector3(
            Mathf.LerpAngle(currentAngle.x, targetAngle.x, Time.deltaTime),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, Time.deltaTime),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, Time.deltaTime));

        transform.eulerAngles = currentAngle;
    }

    private IEnumerator Shift()
    {
        if (targetAngle == startAngle)
        {
            targetAngle = otherAngle;
        }
        else
        {
            targetAngle = startAngle;
        }
        yield return new WaitForSeconds(shiftInterval);
        StartCoroutine(Shift());
    }
}
