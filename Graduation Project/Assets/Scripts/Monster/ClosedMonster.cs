using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class ClosedMonster : MonsterManager
{
    public Animator anim;

    public GameObject livedparts;
    public GameObject deadparts;
    //보류
    [SerializeField] private MeshCollider meshCollider;

    private Vector3 monsterStartPos;

    // 상태값
    public bool isHit = false;
    private bool isAttack = false;

    private float timer;

    private bool alreadyAttacked = false;
    public float attackTerm;
    private float currentAttackTerm;
    public Collider[] colls;
    private Transform target;
    private bool targetOn;

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

        this.MaxHP = 300;
        this.HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (isHit)
        {
            StopTrace();
            Rigidity();
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
                currentAttackTerm -= Time.deltaTime;
                if (Vector3.Distance(target.position, transform.position) <= 20)
                {
                    TraceTarget();
                    isAttack = false;
                }

                if (Vector3.Distance(target.position, transform.position) > 20)
                {
                    StopTrace();
                }

                if (Vector3.Distance(target.position, transform.position) <= 4.0f)
                {
                    if (!isAttack && currentAttackTerm <= 0.0f)
                    {
                        Debug.Log("attack");
                        AttackTarget();
                        currentAttackTerm = attackTerm;
                    }
                }
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
            timer += Time.deltaTime;
            if (timer >= 2.0)
            {
                Destroy(this.gameObject);
            }
        }
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
        if (Vector3.Distance(monsterStartPos, transform.position) <= 0.1f)
        {
            anim.SetBool("Walking", false);
            nav.Stop();
        }
    }

    private void AttackTarget()
    {
        isAttack = true;
        nav.Stop();
        anim.SetTrigger("Attack");
    }

    public void Rigidity()
    {
        anim.SetTrigger("Hit");
        isHit = false;
    }
       
}