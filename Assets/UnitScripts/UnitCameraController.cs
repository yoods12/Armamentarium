using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitCameraController : MonoBehaviour
{
    // 카메라가 따라다닐 대상(플레이어 유닛)
    public Transform target;

    // 카메라와 대상 사이의 기본 거리
    public float distance = 10.0f;
    // 줌 조절 시 최소/최대 거리
    public float minDistance = 5f;
    public float maxDistance = 15f;

    // 마우스 입력에 따른 회전 속도
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    // 카메라가 회전할 때 제한하는 수직 각도
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    // 줌 속도
    public float zoomSpeed = 2f;

    // 내부 회전 각도 (초기값은 Start에서 카메라의 현재 각도에서 가져옵니다)
    private float x = 0.0f;
    private float y = 0.0f;

    void Start()
    {
        // 현재 카메라의 EulerAngles를 초기 회전 각도로 사용
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        // Rigidbody가 있다면 회전과 관련된 물리 영향이 없도록 설정
        if (GetComponent<Rigidbody>() != null)
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }

        // 씬 전환 시 target을 다시 찾도록 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;

        // target이 할당되어 있지 않다면 태그 "Player"로 검색
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                target = player.transform;
        }
    }

    void FixedUpdate()
    {
        if (target)
        {
            // 마우스 입력을 이용해 회전 각도 업데이트
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            y = ClampAngle(y, yMinLimit, yMaxLimit);

            // 스크롤휠로 줌을 조절 (distance 조절)
            distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minDistance, maxDistance);

            // 계산된 회전 값으로 Quaternion 생성
            Quaternion rotation = Quaternion.Euler(y, x, 0);
            // target의 로컬 좌표계에서 (0,0,-distance) 오프셋을 월드 좌표로 변환
            Vector3 negDistance = new Vector3(0.0f, 5f, -distance);
            Vector3 desiredPosition = rotation * negDistance + target.position;

            // LateUpdate에서 카메라 위치 및 회전 적용
            transform.position = desiredPosition;
            transform.rotation = rotation;
        }
    }

    // 각도를 지정된 범위 내로 클램핑하는 함수
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    // 씬 전환 후 target이 null이면 태그 "Player"를 이용하여 다시 할당
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (target == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                target = player.transform;
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
