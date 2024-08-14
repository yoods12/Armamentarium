using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    public Transform target;
    public float moveDamping = 15f;
    public float rotateDamping = 10f;
    public float distance = 5f;
    public float height = 4f;
    public float targetOffset = 2f;

    float maxVAngle = 30f;
    float maxHAngle = 90f;

    Transform tr;

    void Start()
    {
        tr = GetComponent<Transform>();
    }
    void LateUpdate()
    {
        LookAt();
    }

    void LookAt()
    {
        var camPos = target.position - (target.forward * distance) + (target.up * height);
        tr.position = Vector3.Slerp(tr.position, camPos, moveDamping * Time.deltaTime);
        tr.rotation = Quaternion.Slerp(tr.rotation, target.rotation, rotateDamping * Time.deltaTime);
        tr.LookAt(target.position + (target.up * targetOffset));
    }
}