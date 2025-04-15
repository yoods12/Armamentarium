using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitMovementController : MonoBehaviour
{
    // PlayerUnit 오브젝트는 Rigidbody가 부착되어 있으며, 이 오브젝트에 힘을 가하여 움직임을 구현합니다.
    public GameObject playerUnit;

    // 전진/후진에 가할 힘의 크기 (적절하게 조절할 필요가 있음)
    public float horsePower = 50f;
    // 좌우 회전에 가할 토크의 크기
    public float turnSpeed = 50f;

    private Rigidbody unitRigidbody;

    void Awake()
    {
        if (playerUnit != null)
        {
            // 부모 오브젝트에 부착된 Rigidbody 가져오기
            unitRigidbody = playerUnit.GetComponent<Rigidbody>();
        }
        else
        {
            Debug.LogError("PlayerUnit이 할당되지 않았습니다.");
        }
    }

    void FixedUpdate()
    {
        // 특정 씬에서는 움직임을 막을 수 있습니다. 예를 들어, "Factory" 씬에서는 입력을 무시합니다.
        if (SceneManager.GetActiveScene().name != "Factory")
        {
            ProcessInput();
        }
    }

    // 입력 처리: WASD 또는 화살표 키에 의해 전진/후진과 좌우 회전을 적용합니다.
    void ProcessInput()
    {
        // 수직 입력: 전진(+) / 후진(-)
        float forwardInput = Input.GetAxis("Vertical");
        // 수평 입력: 좌(음수) / 우(양수)
        float horizontalInput = Input.GetAxis("Horizontal");

        // 후진할 때 회전 방향 반전 처리
        if (forwardInput < 0)
        {
            horizontalInput = -horizontalInput;
        }

        unitRigidbody.AddRelativeForce(Vector3.forward * forwardInput * horsePower);
        transform.Rotate(Vector3.up, turnSpeed * horizontalInput * Time.deltaTime);
    }

}
