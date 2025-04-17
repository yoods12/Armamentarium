using UnityEngine;
using UnityEngine.SceneManagement;

public class Wheel : MonoBehaviour
{
    [Header("속도 / 인스펙터 창 우선")]
    public float speed;
    public float turnSpeed;

    // 조향에 사용될 바퀴 배열 (PowerGear 태그를 가진 바퀴들)
    public Transform[] steeringWheels;
    // playerUnit은 이 스크립트가 부착된 오브젝트의 부모 (유닛)
    public Transform playerUnit;

    public float maxSteerAngle = 30f;
    public float steerSpeed = 5f;

    void Awake()
    {
        // TrainingCenter 씬이 로드되었을 때 자동 할당을 위해 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // TrainingCenter 씬이 로드되면 playerUnit과 steeringWheels를 자동 할당합니다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "TrainingCenter")
        {
            // playerUnit이 아직 할당되지 않았다면 부모를 할당
            if (playerUnit == null && transform.parent != null)
            {
                playerUnit = transform.parent;
            }

            // steeringWheels가 비어있다면, playerUnit의 자식 중 PowerGear 태그만 할당
            if ((steeringWheels == null || steeringWheels.Length == 0) && playerUnit != null)
            {
                var list = new System.Collections.Generic.List<Transform>();
                foreach (Transform child in playerUnit.GetComponentsInChildren<Transform>())
                {
                    if (child != playerUnit && child.CompareTag("Power Gear"))
                    {
                        list.Add(child);
                    }
                }
                steeringWheels = list.ToArray();
            }
        }
    }

    void Update()
    {
        WheelSteering();
    }

    void WheelSteering()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        // 각 바퀴에 대해 현재 회전 상태에서 증분 변화를 적용합니다.
        foreach (Transform wheel in steeringWheels)
        {
            // 오브젝트의 태그가 "PowerGear"인 경우에만 조향 처리합니다.
            if (!wheel.CompareTag("Power Gear"))
                continue;

            // playerUnit의 로컬 좌표계에서 해당 바퀴의 위치를 계산합니다.
            Vector3 localPos = playerUnit.InverseTransformPoint(wheel.position);

            // 입력에 따른 증분 회전값 계산 (delta는 매 프레임 적용되는 변화량)
            float deltaAngle = horizontalInput * maxSteerAngle * Time.deltaTime * steerSpeed;
            // localPos.z가 음수이면 반대 부호의 증분을 적용 (즉, -z 쪽이면 좌우 회전 반대로)
            if (localPos.z < 0)
                deltaAngle = -deltaAngle;

            // 현재 바퀴의 로컬 y각도를 구하고, -180 ~ 180 범위로 변환합니다.
            float currentY = wheel.localEulerAngles.y;
            if (currentY > 180)
                currentY -= 360;

            // 현재 회전 상태에 증분을 더한 후, 최대 회전 각도로 클램핑합니다.
            float newY = Mathf.Clamp(currentY + deltaAngle, -maxSteerAngle, maxSteerAngle);

            // 기존의 x, z 값은 그대로 두고 y축 회전만 업데이트합니다.
            wheel.localEulerAngles = new Vector3(wheel.localEulerAngles.x, newY, wheel.localEulerAngles.z);
        }
    }
}
