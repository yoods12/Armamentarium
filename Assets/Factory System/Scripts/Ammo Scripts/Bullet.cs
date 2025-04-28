using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 로컬 Z+ (transform.forward) 방향으로 속도 설정
            rb.velocity = transform.forward * speed;
        }
    }
}
