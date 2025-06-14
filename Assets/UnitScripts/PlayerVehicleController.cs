using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerVehicleController : MonoBehaviour
{
    [Header("Drive Settings")]
    public float driveForce = 500f;
    public float turnSpeed = 30f;  // 토크 대신 회전 속도로 변경

    [Header("Turret Settings")]
    [Tooltip("자동으로 자식 중 이름이 'FirePoint'인 오브젝트를 피봇으로 할당")]
    public Transform turretPivot;
    public float turretRotateSpeed = 5f;

    private Rigidbody rb;
    private bool canControl = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // X, Z 회전은 고정하고 Y축만 풀어둡니다
        rb.constraints = RigidbodyConstraints.FreezeRotationX
                       | RigidbodyConstraints.FreezeRotationZ;

        // turretPivot 자동 할당
        if (turretPivot == null)
        {
            turretPivot = GetComponentsInChildren<Transform>()
                          .FirstOrDefault(t => t.name == "FirePoint");
            if (turretPivot == null)
                Debug.LogWarning($"[{name}] 자식에 'FirePoint'가 없어 turretPivot 할당 실패");
        }

        // 초기 씬 제어 상태
        SetControlState(SceneManager.GetActiveScene().name);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetControlState(scene.name);
    }

    private void SetControlState(string sceneName)
    {
        bool isTraining = sceneName == "TrainingCenter";
        rb.useGravity = isTraining;
        canControl = isTraining;
    }

    void FixedUpdate()
    {
        if (!canControl) return;

        float v = Input.GetAxis("Vertical");   // W/S
        float h = Input.GetAxis("Horizontal"); // A/D

        // 전진·후진: Rigidbody로 물리 힘 적용
        rb.AddRelativeForce(Vector3.forward * v * driveForce * Time.fixedDeltaTime,
                            ForceMode.Acceleration);

        // 좌우 회전: Transform을 직접 회전
        float yaw = h * turnSpeed * Time.fixedDeltaTime;
        transform.Rotate(0f, yaw, 0f, Space.Self);
    }

    void Update()
    {
        if (!canControl || turretPivot == null) return;

        float mx = Input.GetAxis("Mouse X");
        turretPivot.Rotate(0f, mx * turretRotateSpeed, 0f, Space.Self);
    }
}
