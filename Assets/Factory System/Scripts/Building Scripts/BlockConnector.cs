using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class BlockConnector : MonoBehaviour
{
    [Header("이웃 연결 설정 (Factory 씬에서만)")]
    public float connectRadius = 0.6f;      // 반경 내 블럭과 연결
    public LayerMask blockLayer;            // 블럭 전용 레이어

    Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        // 씬 전환 시에도 물리 설정을 다시 해 주기 위해 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        // ① Start()에서도 현재 씬이 Factory 이면 물리 설정 + Joint 연결
        var scene = SceneManager.GetActiveScene();
        ConfigurePhysics(scene);
        if (scene.name == "Factory")
            ConnectToNeighbors();
    }

    // 씬이 바뀔 때마다 호출
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // ② 씬 전환 시에도 물리 설정
        ConfigurePhysics(scene);
        // Factory로 전환될 때 Joint 연결
        if (scene.name == "Factory")
            ConnectToNeighbors();
    }

    // 물리 속성만 분리
    void ConfigurePhysics(Scene scene)
    {
        bool isFactory = scene.name == "Factory";
        _rb.isKinematic = isFactory;     // 빌딩 모드면 kinematic
        _rb.useGravity = !isFactory;   // 테스트 모드면 gravity on
    }

    // 반경 내 블록에 FixedJoint 연결
    void ConnectToNeighbors()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, connectRadius, blockLayer);
        foreach (var hit in hits)
        {
            if (hit.gameObject == gameObject)
                continue;

            // 중복 연결 방지
            if (hit.gameObject.GetInstanceID() < gameObject.GetInstanceID())
                continue;

            Rigidbody otherRb = hit.GetComponent<Rigidbody>();
            if (otherRb == null)
                continue;

            var fj = gameObject.AddComponent<FixedJoint>();
            fj.connectedBody = otherRb;
            fj.breakForce = Mathf.Infinity;
            fj.breakTorque = Mathf.Infinity;
            fj.enableCollision = false;
        }
    }

    // Editor 가시화
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, connectRadius);
    }
}
