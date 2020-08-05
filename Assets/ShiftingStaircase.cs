using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftingStaircase : MonoBehaviour
{
    private Vector3 startPosition;
    public Vector3 otherPosition;
    public float duration;
    public bool atStart = true;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator LerpPosition(Vector3 targetPosition)
    {
        float time = 0;
        Vector3 currentPosition = transform.position;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(currentPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
    }

    public void shiftPosition()
    {
        Vector3 targetPosition;
        if (atStart)
        {
            atStart = false;
            targetPosition = otherPosition;
        }
        else
        {
            atStart = true;
            targetPosition = startPosition;
        }
        StopAllCoroutines();
        StartCoroutine(LerpPosition(targetPosition));
    }
}
