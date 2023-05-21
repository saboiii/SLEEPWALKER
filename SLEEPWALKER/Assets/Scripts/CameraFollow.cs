using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;

    private void LateUpdate()
    {
        if (target != null) // Check if the target reference is not null
        {
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, target.position, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
