using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float speed = 100;

    float h = 0f;
    float v = 0f;

    GameObject Collider;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(h, 0f, v);
        rb.AddForce(moveDir.normalized * speed);
    }

    void Update()
    {
        
    }
}
