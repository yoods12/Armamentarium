using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitCameraController : MonoBehaviour
{
    [Header("타겟 설정")]
    public Transform target;             // 따라갈 대상 (Player)

    [Header("거리(줌) 설정")]
    public float distance = 10f;
    public float minDistance = 5f;
    public float maxDistance = 15f;
    public float zoomSpeed = 2f;

    [Header("회전 속도")]
    public float xSpeed = 120f;
    public float ySpeed = 120f;

    [Header("수직 각도 제한")]
    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    [Header("높이 오프셋")]
    public float heightOffset = 2f;  // 여기서 원하는 높이를 인스펙터에서 조절

    private float x = 0f;
    private float y = 0f;
    private bool isFactory;

    void Awake()
    {
        // 씬 전환마다 Factory 씬 여부 갱신
        SceneManager.sceneLoaded += OnSceneLoaded;
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
    }

    void Start()
    {
        // 초기 타겟 찾기
        if (target == null)
        {
            var p = GameObject.FindWithTag("Core Gear");
            if (p) target = p.transform;
        }
        // 현재 카메라 각도 저장
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        isFactory = (scene.name == "Factory");
        // Factory 씬에선 이동을 멈추고
        if (isFactory)
            return;
        // 아닌 씬으로 넘어가면 타겟이 없을 수도 있으니 다시 찾기
        if (target == null)
        {
            var p = GameObject.FindWithTag("Core Gear");
            if (p) target = p.transform;
        }
    }

    void Update()
    {
        // Factory 씬이면 조작 무시
        if (isFactory || target == null) return;

        // 회전 입력
        x += Input.GetAxis("Mouse X") * xSpeed * Time.deltaTime;
        y -= Input.GetAxis("Mouse Y") * ySpeed * Time.deltaTime;
        y = ClampAngle(y, yMinLimit, yMaxLimit);

        // 줌 입력
        distance = Mathf.Clamp(
            distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed,
            minDistance, maxDistance
        );
    }

    void LateUpdate()
    {
        if (isFactory || target == null) return;

        // 1) 회전
        Quaternion rot = Quaternion.Euler(y, x, 0f);
        // 2) 로컬 오프셋에 높이와 거리(줌) 한 번에 넣기
        Vector3 localOffset = new Vector3(0f, heightOffset, -distance);
        // 3) 월드 위치 계산
        Vector3 desiredPos = target.position + rot * localOffset;

        transform.position = desiredPos;
        transform.rotation = rot;
    }

    // -360~360 범위 안전 클램핑
    static float ClampAngle(float angle, float min, float max)
    {
        while (angle < -360f) angle += 360f;
        while (angle > 360f) angle -= 360f;
        return Mathf.Clamp(angle, min, max);
    }
}
