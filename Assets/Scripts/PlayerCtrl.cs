using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

[Serializable]
public class Whee
{
    public GameObject WheelModel;
    public WheelCollider WheelCollider;
}
public class PlayerCtrl : MonoBehaviour
{
    Rigidbody rb;

    float h = 0f;
    float v = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");

        Vector3 moveDir = new Vector3(h, 0f, v);
        rb.AddForce(moveDir);
    }

    void Update()
    {
        
    }
}
