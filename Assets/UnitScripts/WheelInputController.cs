using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WheelInputController : MonoBehaviour
{
    [Header("Tag Settings")]
    [Tooltip("플레이어 유닛에 붙어있는 태그")]
    public string playerTag = "Player";
    [Tooltip("휠 오브젝트에 붙어있는 태그")]
    public string wheelTag = "Tire";

    [Header("Rotation Settings")]
    [Tooltip("휠 Spin 속도 (도/초)")]
    public float rotationSpeed = 180f;
    [Tooltip("Y축 Steer 최대 각도 (±값)")]
    public float maxSteerAngle = 30f;

    private Transform playerTransform;
    private Transform[] wheels;

    // 현재 스티어 목표값과 이전값
    private float currentSteerOffset = 0f;
    private float previousSteerOffset = 0f;

    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        AssignWheels();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        AssignWheels();
    }

    private void AssignWheels()
    {
        GameObject player = GameObject.FindWithTag(playerTag);
        if (player == null)
        {
            Debug.LogWarning($"[{name}] 태그 '{playerTag}' 오브젝트를 찾을 수 없습니다.");
            wheels = new Transform[0];
            playerTransform = null;
            return;
        }

        playerTransform = player.transform;

        var list = new List<Transform>();
        foreach (Transform t in player.GetComponentsInChildren<Transform>())
            if (t.CompareTag(wheelTag))
                list.Add(t);

        wheels = list.ToArray();
        Debug.Log($"[{name}] {wheels.Length}개의 휠 할당 완료 (부모: {player.name}).");

        // 스티어 오프셋 초기화
        currentSteerOffset = previousSteerOffset = 0f;
    }

    void Update()
    {
        if (wheels == null || wheels.Length == 0 || playerTransform == null)
            return;

        float spinDelta = rotationSpeed * Time.deltaTime;

        // 1) Spin: 부모 로컬 X축 기준 굴림
        if (Input.GetKey(KeyCode.W)) RotateSpin(+spinDelta);
        else if (Input.GetKey(KeyCode.S)) RotateSpin(-spinDelta);

        previousSteerOffset = currentSteerOffset;
    }

    private void RotateSpin(float angle)
    {
        // 부모의 로컬 X축을 World 축으로 변환
        Vector3 axis = playerTransform.TransformDirection(Vector3.right);
        foreach (var w in wheels)
            w.Rotate(axis, angle, Space.World);
    }

}
