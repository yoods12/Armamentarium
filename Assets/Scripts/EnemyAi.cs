using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent enemy; //적

    public Transform player; //플레이어

    public LayerMask whatIsGround, whatIsPlayer; // 땅, 플레이어 레이어 확인

    //Patroling
    public Vector3 walkPoint; // Ai 이동지점 설정
    bool walkPointSet; // 이동지점이 설정되었는가
    public float walkPointRange; // 이동지점 설정 범위

    //Attacking
    public float timeBetweenAttacks; // 공격 시간
    bool alreadyAttacked; // 이미 공격이 나가고 있는가

    //States
    public float sightRange, attackRange; // 탐지범위, 공격범위
    bool playerInSightRange, playerInAttackRange; // 플레이어가 탐지범위에 들어왔는가, 플레이어가 공격범위에 들어왔는가

    public GameObject enemyBullet; // 적 총알

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
        enemy = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // 플레이어가 시야범위, 공격범위에 있는지
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling(); // 순찰
        if (playerInSightRange && !playerInAttackRange) ChasePlayer(); // 추적
        if (playerInSightRange && playerInAttackRange) AttackPlayer(); // 공격
    }

    void Patroling()
    {
        if(!walkPointSet) SearchWalkPoint();

        if(walkPointSet)
            enemy.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // walkPoint에 도달하면 walkPoint를 false로 하고 다시 지정
        if(distanceToWalkPoint.magnitude < 5f)
            walkPointSet = false;
    }
    void SearchWalkPoint() // walkPoint 지정
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        
        if(Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }
    void ChasePlayer()
    {
        enemy.SetDestination(player.position);
    }
    void AttackPlayer() // 공격
    {
        enemy.SetDestination(transform.position);
        transform.LookAt(player);
        if(!alreadyAttacked)
        {
            Rigidbody rb = Instantiate(enemyBullet, transform.position, Quaternion.identity).GetComponent<Rigidbody>();

            rb.AddForce(transform.forward * 10f, ForceMode.Impulse);
            rb.AddForce(transform.up * 10f, ForceMode.Impulse);
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
