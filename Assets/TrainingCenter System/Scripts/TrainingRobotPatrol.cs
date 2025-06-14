using UnityEngine;

public class TrainingRobotPatrol : MonoBehaviour
{
    [Header("Patrol Settings")]
    [Tooltip("시작 지점에서 순찰할 거리")]
    public float patrolDistance = 10f;
    [Tooltip("이동 속도 (유닛/초)")]
    public float patrolSpeed = 3f;
    [Tooltip("양 끝점 도착 후 대기 시간 (초)")]
    public float waitTime = 0.5f;

    private Vector3 pointA;
    private Vector3 pointB;
    private bool goingToB = true;
    private float waitTimer = 0f;

    void Start()
    {
        // 순찰 시작 위치 저장
        pointA = transform.position;
        // pointA 기준 로컬 forward 방향으로 patrolDistance 만큼 떨어진 지점 계산
        pointB = pointA + transform.forward * patrolDistance;
    }

    void Update()
    {
        // 대기 중이라면 타이머만 감소
        if (waitTimer > 0f)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        // 현재 목표 지점 선택
        Vector3 target = goingToB ? pointB : pointA;

        // 1) 위치 이동
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            patrolSpeed * Time.deltaTime
        );

        // 2) 바라보는 방향 회전 (부드럽게)
        Vector3 dir = (target - transform.position).normalized;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion lookRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                lookRot,
                Time.deltaTime * 5f
            );
        }

        // 3) 목표 도착 체크
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            // 반대 지점으로 전환 및 대기
            goingToB = !goingToB;
            waitTimer = waitTime;
        }
    }

    // 씬 전환 등으로 위치가 초기화될 때 순찰 지점도 다시 계산하고 싶다면, 
    // OnEnable()/OnDisable() 등에 AssignPointA/B 코드를 넣어 주시면 됩니다.
}
