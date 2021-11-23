using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector2 offset;
    public Rigidbody2D followPoint;
    public float speed = 1f;

    void LateUpdate()
    {
        Vector3 goal = new Vector3(
            followPoint.position.x,
            followPoint.position.y,
            transform.position.z
        ) + (Vector3) offset;
        float distance = (goal - transform.position).magnitude;

        Vector3 newPos = Vector3.Lerp(transform.position, goal, Time.deltaTime * speed * distance);
        newPos.z = transform.position.z;
        transform.position = newPos;
    }
}
