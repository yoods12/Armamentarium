using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct AngleLimit
{
    public float min;  // 최소 각도 (deg)
    public float max;  // 최대 각도 (deg)
}

public class UnitAimManager : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;        // 플레이어가 보는 카메라
    public Transform[] weaponTransforms;   // 조준할 무기(터렛)들의 Transform 배열

    [Header("Settings")]
    public float maxAimDistance = 100f;    // 레이 최대 사거리

    [Header("Angle Limits (local space)")]
    AngleLimit yawLimit = new AngleLimit { min = -90f, max = 90f };  // 좌우
    AngleLimit pitchLimit = new AngleLimit { min = 0f, max = 45f };  // 상하

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        AssignCameraAndWeapons(SceneManager.GetActiveScene());
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignCameraAndWeapons(scene);
    }

    // TrainingCenter 씬이면 Camera.main 과 Weapon 태그 오브젝트들을 할당
    void AssignCameraAndWeapons(Scene scene)
    {
        if (scene.name == "TrainingCenter")
        {
            playerCamera = Camera.main;
            GameObject[] weapons = GameObject.FindGameObjectsWithTag("Weapon");
            weaponTransforms = new Transform[weapons.Length];
            for (int i = 0; i < weapons.Length; i++)
                weaponTransforms[i] = weapons[i].transform;
        }
    }

    void Update()
    {
        if (playerCamera == null || weaponTransforms == null || weaponTransforms.Length == 0)
            return;

        // 화면 중앙에서 Ray 생성
        Ray ray = playerCamera.ScreenPointToRay(
            new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)
        );

        // 타겟 포인트 계산
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out RaycastHit hit, maxAimDistance))
            targetPoint = hit.point;
        else
            targetPoint = ray.origin + ray.direction * maxAimDistance;

        // 각 무기마다 회전 및 각도 제한 적용
        foreach (Transform weapon in weaponTransforms)
        {
            // 1) 월드 공간 목표 회전
            Quaternion worldTarget = Quaternion.LookRotation(targetPoint - weapon.position, Vector3.up);

            // 2) 부모 로컬 공간으로 변환
            Quaternion localTarget = Quaternion.Inverse(weapon.parent.rotation) * worldTarget;
            Vector3 e = localTarget.eulerAngles;

            // 3) Euler -> -180~180 범위로 변환
            float yaw = e.y > 180f ? e.y - 360f : e.y;
            float pitch = e.x > 180f ? e.x - 360f : e.x;

            // 4) 제한값으로 Clamp
            yaw = Mathf.Clamp(yaw, yawLimit.min, yawLimit.max);
            pitch = Mathf.Clamp(pitch, pitchLimit.min, pitchLimit.max);

            // 5) 클램프된 로컬 회전 재구성
            Quaternion clampedLocal = Quaternion.Euler(pitch, yaw, 0f);

            // 6) 로컬 → 월드 회전 변환 후 적용
            weapon.rotation = weapon.parent.rotation * clampedLocal;
        }
    }
}
