using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitAttackController : MonoBehaviour
{
    // 현재 유닛에 장착된 무기. 인스펙터에서 할당하지 않으면 Awake()에서 자식 오브젝트 중 검색
    public Weapon currentWeapon;
    public ParticleSystem muzzleFlash;
    // 마지막 발사 이후 경과 시간 추적 변수
    private float fireTimer = 0f;

    void Awake()
    {
        // 인스펙터에서 currentWeapon이 할당되어 있지 않다면 자식에서 탐색
        if (currentWeapon == null)
        {
            currentWeapon = GetComponentInChildren<Weapon>();
        }

        // TrainingCenter 씬으로 전환될 때를 대비해 씬 로드 이벤트에 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // TrainingCenter 씬으로 전환된 후, 무기가 없으면 다시 자식 오브젝트를 검색하여 할당
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TrainingCenter")
        {
            if (currentWeapon == null)
            {
                currentWeapon = GetComponentInChildren<Weapon>();
                if (currentWeapon == null)
                {
                    Debug.LogWarning("TrainingCenter 씬에서 자식 오브젝트에서 Weapon 컴포넌트를 찾을 수 없습니다.");
                }
            }
        }
    }

    void Update()
    {
        fireTimer += Time.deltaTime;

        // "Fire1" 입력(예: 마우스 좌클릭)과 무기가 존재하며 연사 간격이 경과한 경우 발사 처리
        if (Input.GetButtonDown("Fire1") && currentWeapon != null && fireTimer >= currentWeapon.fireRate)
        {
            FireWeapon();
            fireTimer = 0f;
        }
    }

    // 무기를 발사하는 함수
    public void FireWeapon()
    {
        if (currentWeapon != null)
        {
            // currentWeapon에 지정된 발사 지점(firePoint)에서 projectilePrefab 생성
            GameObject projectile = Instantiate(currentWeapon.projectilePrefab,
                                                currentWeapon.firePoint.position,
                                                currentWeapon.firePoint.rotation);
            //muzzleFlash.Play();
        }
    }

    // 무기 교체 등 외부 호출을 위한 함수
    public void EquipWeapon(Weapon newWeapon)
    {
        currentWeapon = newWeapon;
    }
}
