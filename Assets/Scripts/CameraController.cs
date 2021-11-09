using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector2 offset;
    public Transform followPoint;

    void LateUpdate()
    {
        transform.position = new Vector3(
            followPoint.position.x,
            followPoint.position.y,
            transform.position.z
        ) + (Vector3) offset;
    }
}
