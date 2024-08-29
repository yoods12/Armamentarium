using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoCtrl : MonoBehaviour
{
    //int damage = 10;
    float speed = 5000f;

    AudioSource audioSource;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        rb.AddForce(transform.forward * speed);
        Destroy(gameObject, 3f);
        audioSource.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider)
        {
            Destroy(gameObject, 0.5f);
        }
    }

}
