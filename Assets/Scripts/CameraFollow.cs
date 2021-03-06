using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Vector3 offset;
    void Start()
    {
        offset = transform.position - target.transform.position;
    }

    void LateUpdate()
    {
        Vector3 newPos = new Vector3(offset.x + target.position.x, transform.position.y, offset.z + target.position.z);
        transform.position = newPos;

    }
}
