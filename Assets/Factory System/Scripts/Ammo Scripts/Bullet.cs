using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Tooltip("이 총알이 입힐 데미지")]
    public float damage = 20f;

    [Header("Spawn Sound")]
    [Tooltip("총알이 생성될 때 재생할 오디오 클립")]
    public AudioClip spawnSFX;
    [Tooltip("생성 사운드 재생 볼륨")]
    [Range(0f, 1f)]
    public float spawnVolume = 1f;

    void Start()
    {
        // 총알 생성 시점에 사운드 재생
        if (spawnSFX != null)
            AudioSource.PlayClipAtPoint(spawnSFX, transform.position, spawnVolume);
    }

    void OnCollisionEnter(Collision collision)
    {
        // 충돌한 상대에게 Health 컴포넌트가 있으면
        var hp = collision.collider.GetComponentInParent<Health>();
        if (hp != null)
        {
            hp.TakeDamage(damage);
        }

        // 총알 삭제
        Destroy(gameObject);
    }
}
