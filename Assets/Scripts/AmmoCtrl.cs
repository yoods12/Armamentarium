using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCtrl : MonoBehaviour
{
    //float damage = 10f;
    float speed = 1000f;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
