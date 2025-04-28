using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class UnitMovementController : MonoBehaviour
{
    [Header("References")]
    public Rigidbody unitRigidbody;   // PlayerUnit의 Rigidbody (FixedJoint의 connectedBody)

    [Header("Settings")]
    public float horsePower;    // 전/후진용 힘 크기
    public float turnSpeed = 30f;   // 회전 속도 (도/초)

    private Scene currentScene;

    void Awake()
    {
        if (unitRigidbody == null)
            unitRigidbody = GetComponent<Rigidbody>();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        ConfigurePhysics(currentScene);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene;
        ConfigurePhysics(scene);
    }

    void FixedUpdate()
    {
        if (currentScene.name == "Factory") return;

        float forward = Input.GetAxis("Vertical");
        float turn = Input.GetAxis("Horizontal");
        if (forward < 0f) turn = -turn;

        // 전/후진 힘
        unitRigidbody.AddForce(transform.forward * forward * horsePower, ForceMode.Acceleration);
        transform.Rotate(Vector3.up, turnSpeed * turn);
    }


    void ConfigurePhysics(Scene scene)
    {
        bool isFactory = scene.name == "Factory";
        unitRigidbody.isKinematic = isFactory;
        unitRigidbody.useGravity = !isFactory;
    }
}
