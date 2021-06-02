using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class RangedMonster : MonsterManager
{
    //보류
    [SerializeField] private MeshCollider meshCollider;

    public GameObject FirePos;

    private Vector3 monsterStartPos;

    private bool alreadyAttacked = false;
    [SerializeField] private float attackTerm;

    public Collider[] colls;
    
    // 상태값
    public bool isHit = false;
    private bool isAttack = false;

    private float timer = 2.0f;

    public GameObject projectile;

    private Vector3 bulletStartPos;

    private Transform target;
    private bool targetOn;

    public GameObject HeadPart;
    
    // 패트롤
    public float range = 10.0f;
    
    public float patrolTime = 3.0f;
    private float currentPatrolTime;
    private float minX, maxX, minZ, maxZ;

    private Vector3 moveSpot;

    private bool isPatrol;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        meshCollider = GetComponent<MeshCollider>();
        nav = GetComponent<NavMeshAgent>();

        bulletStartPos = FirePos.transform.position;
        
        monsterStartPos = transform.position;

        nav.enabled = true;
        // 플레이어 타겟 잡는 곳
        targetOn = false;

        this.MaxHP = 150;
        this.HP = this.MaxHP;
        this.armor = 30;
        
        GetPatrolRange();
        moveSpot = GetNewPosition();
        currentPatrolTime = patrolTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            colls = Physics.OverlapSphere(transform.position, 20.0f);
            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i].tag == "Player")
                {
                    target = colls[i].transform;
                    targetOn = true;
                    break;
                }
            }

            if (targetOn)
            {
                if (Vector3.Distance(target.position, transform.position) <= 20)
                {
                    TraceTarget();
                }

                if (Vector3.Distance(target.position, transform.position) > 20)
                {
                    StopTrace();
                    targetOn = false;
                }

                if (Vector3.Distance(target.position, transform.position) <= 10.0f)
                {
                    if (!isAttack)
                    {
                        AttackTarget();
                    }
                }
            }
            
            
            if (!targetOn)
            {
                WatchYourStep();
                GetToStepping();
            }
            
            if (this.HP <= 0)
            {
                HeadPart.transform.parent = null;
                HeadPart.AddComponent<Rigidbody>();
                
                Dead();
                
            }
        }
        
        if (isDead)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0)
            {
                Destroy(this.gameObject);
                Destroy(HeadPart);
                timer = 2.0f;
            }

        }
    }
    private void GetPatrolRange()
    {
        minX = monsterStartPos.x - range;
        maxX = monsterStartPos.x + range;
        minZ = monsterStartPos.z - range;
        maxZ = monsterStartPos.z + range;
    }

    Vector3 GetNewPosition()
    {
        float randomX = UnityEngine.Random.Range(minX, maxX);
        float randomZ = UnityEngine.Random.Range(minZ, maxZ);

        Vector3 newPosition = new Vector3(randomX, transform.position.y, randomZ);
        return newPosition;
    }

    private void GetToStepping()
    {
        nav.SetDestination(moveSpot);
        currentPatrolTime -= Time.deltaTime;
        if (currentPatrolTime <= 0.0f || Vector3.Distance(transform.position, moveSpot) <= 0.2f)
        {
            moveSpot = GetNewPosition();
            currentPatrolTime = patrolTime;
        }
    }

    private void WatchYourStep()
    {
        Vector3 targetDirection = moveSpot - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0.3f, 0f);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }
    private void TraceTarget()
    {
        // 플레이어와의 거리가 20보다 가까울 때 
        nav.Resume();
        //transform.LookAt(target);
        Vector3 dirToTarget = target.transform.position - this.transform.position;
        this.transform.forward = dirToTarget.normalized;
        this.transform.rotation = Quaternion.LookRotation(dirToTarget,Vector3.up);
        
        this.transform.LookAt(target);
        
        nav.SetDestination(target.position);
    }
    
    private void StopTrace()
    {
        nav.SetDestination(monsterStartPos);
        if (Vector3.Distance(monsterStartPos , transform.position) <= 0.1f)
        {
            nav.Stop();
        }
    }
    
    private void AttackTarget()
    {
        if (!alreadyAttacked)
        {
            Fire();
        }
        
        nav.Stop();
    }

    private void Fire()
    {
        Rigidbody rb = Instantiate(projectile, FirePos.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 10f , ForceMode.Impulse);
        rb.AddForce(transform.up * 2f , ForceMode.Impulse);

        // Vector3 dirToTarget = target.transform.position - transform.position;
        // transform.forward = -dirToTarget.normalized;

        alreadyAttacked = true;
        Invoke(nameof(ResetAttack),attackTerm);
        //rb.velocity = dirToTarget.normalized * 20;
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
