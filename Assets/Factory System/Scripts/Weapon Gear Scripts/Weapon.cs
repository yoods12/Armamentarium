using UnityEngine;

public class Weapon : MonoBehaviour
{
    // 무기에 따라 다른 발사체 프리팹을 할당할 수 있습니다.
    public GameObject projectilePrefab;
    // 무기의 발사 지점. 예를 들어 총구 위치.
    public Transform firePoint;
    // 발사체의 이동 속도.
    public float projectileSpeed = 20f;
    // 무기의 연사 간격 (초 단위).
    public float fireRate = 0.5f;
}
