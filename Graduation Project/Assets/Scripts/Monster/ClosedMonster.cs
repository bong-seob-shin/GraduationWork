using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = System.Random;

public class ClosedMonster : MonsterManager
{
    public Animator anim;

    public GameObject livedparts;
    public GameObject deadparts;
    public MeshCollider meshCollider;

    private Vector3 monsterStartPos;

    // 상태값
    private bool isAttack = false;

    private float timer = 2.0f;
    
    public float attackTerm;
    private float currentAttackTerm;
    private float motionDelay = 1.0f;
    public Collider[] colls;
    public Transform target;
    public bool targetOn;
    
    public BoxCollider attackCol;
    
    // 패트롤
    public float range = 10.0f;
    
    public float patrolTime = 3.0f;
    private float currentPatrolTime;
    private float minX, maxX, minZ, maxZ;

    private Vector3 moveSpot;

    private bool isPatrol;
    
    // 맞았을 때 타이머
    private float currentAgroTime;
    private float agroTime = 5.0f;
    public bool agroOn = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        meshCollider = GetComponent<MeshCollider>();
        nav = GetComponent<NavMeshAgent>();

        monsterStartPos = transform.position;
        

        nav.enabled = true;
        
        // 플레이어 타겟 잡는 곳
        targetOn = false;
        currentAttackTerm = attackTerm;
        
        // 패트롤
        isPatrol = true;

        this.MaxHP = 300;
        this.HP = MaxHP;
        this.armor = 50;
        
        GetPatrolRange();
        moveSpot = GetNewPosition();
        currentPatrolTime = patrolTime;

        currentAgroTime = agroTime;
        //bm = GetComponent<BossMonster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit)
        {
            //StopTrace();
            Rigidity();
            agroOn = true;
        }
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
                TraceTarget();
                if (agroOn)
                {
                    currentAgroTime -= Time.deltaTime;
                    if (currentAgroTime <= 0.0f && Vector3.Distance(target.position, transform.position) > 20)
                    {
                        StopTrace();
                        currentAgroTime = agroTime;
                        agroOn = false;
                        targetOn = false;
                    }
                }
                else
                {
                    if (Vector3.Distance(target.position, transform.position) > 20)
                    {
                        StopTrace();
                    }
                }
                // if (Vector3.Distance(target.position, transform.position) <= 20
                //     &&  Vector3.Distance(target.position , transform.position) >= 3.0f)
                // {
                //     attackCol.gameObject.SetActive(false);
                //     TraceTarget();
                //     isAttack = false;
                // }
                //
              
                
                currentAttackTerm -= Time.deltaTime;
                if (Vector3.Distance(target.position, transform.position) <= 3.0f)
                {
                    nav.Stop();
                    if (currentAttackTerm <= 0.0f)
                    {
                        AttackTarget();
                        currentAttackTerm = attackTerm;
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
                livedparts.SetActive(false);
                deadparts.SetActive(true);
               
                Dead();
                

            }
        }

        if (isDead)
        {
            timer -= Time.deltaTime;
            if (timer <= 0.0f)
            {
                Destroy(this.gameObject);
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
        anim.SetBool("Walking", true);
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
        transform.LookAt(target);
        anim.SetBool("Walking", true);
        nav.SetDestination(target.position);
    }

    private void StopTrace()
    {
        nav.SetDestination(monsterStartPos);
        transform.LookAt(monsterStartPos);
        if (Vector3.Distance(monsterStartPos, transform.position) <= 1.0f)
        {
            anim.SetBool("Walking", false);
            nav.Stop();
            targetOn = false;
        }
    }

    private void AttackTarget()
    {
        isAttack = true;
        nav.Stop();
        anim.SetTrigger("Attack");
        attackCol.gameObject.SetActive(true);
    }

    public void Rigidity()
    {
        nav.Stop();
        anim.SetTrigger("Hit");
        isHit = false;
    }
       
}