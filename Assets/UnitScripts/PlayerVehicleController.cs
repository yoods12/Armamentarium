using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class PlayerVehicleController : MonoBehaviour
{
    [Header("Drive Settings")]
    public float driveForce = 500f;
    public float turnTorque = 200f;

    [Header("Turret Settings")]
    [Tooltip("자동으로 자식 중 이름이 'FirePoint'인 오브젝트를 피봇으로 할당")]
    public Transform turretPivot;
    public float turretRotateSpeed = 5f;

    private Rigidbody rb;
    private bool canControl = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // turretPivot 자동 할당: 자식 중 이름이 "FirePoint"인 Transform
        if (turretPivot == null)
        {
            turretPivot = GetComponentsInChildren<Transform>()
                          .FirstOrDefault(t => t.name == "FirePoint");
            if (turretPivot == null)
                Debug.LogWarning($"[{name}] 자식에 'FirePoint'가 없어 turretPivot 할당 실패");
        }

        // 처음 씬 로드 시 상태 설정
        SetControlState(SceneManager.GetActiveScene().name);

        // 씬 전환 콜백
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

        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        rb.AddRelativeForce(Vector3.forward * v * driveForce * Time.fixedDeltaTime);
        rb.AddTorque(Vector3.up * h * turnTorque * Time.fixedDeltaTime);
    }

    void Update()
    {
        if (!canControl || turretPivot == null) return;

        float mouseX = Input.GetAxis("Mouse X");
        turretPivot.Rotate(0f, mouseX * turretRotateSpeed, 0f, Space.Self);
    }
}
