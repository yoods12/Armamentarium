using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 총알의 이동 속도 (z축 방향)
    public float speed = 20f;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            // 월드의 z축 방향 (0, 0, 1)에 speed를 곱한 속도로 설정합니다.
            rb.velocity = new Vector3(0f, 0f, speed);
        }
    }
}
