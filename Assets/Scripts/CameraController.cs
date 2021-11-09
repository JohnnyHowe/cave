using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector2 offset;
    public Transform followPoint;
    public float speed = 1f;
    Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 goal = new Vector3(
            followPoint.position.x,
            followPoint.position.y,
            transform.position.z
        ) + (Vector3) offset;
        float distance = (goal - transform.position).magnitude;
        float smoothTime = speed / distance;
        transform.position = Vector3.SmoothDamp(transform.position, goal, ref velocity, smoothTime);
    }
}
