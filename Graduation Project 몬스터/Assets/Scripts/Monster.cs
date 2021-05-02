using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private NavMeshAgent nav;

    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rigid;
    
    //보류
    [SerializeField] private MeshCollider meshCollider;

    private Vector3 monsterStartPos;
    
    // 상태값
    public bool isDead = false;
    public bool isHit = false;
    private bool isAttack = false;

    private float timer;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        meshCollider = GetComponent<MeshCollider>();
        nav = GetComponent<NavMeshAgent>();

        monsterStartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
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
                if (!isAttack)
                {
                    Debug.Log("attack");
                    AttackTarget();
                }
            }
        }
        
        if (isDead)
        {
            timer += Time.deltaTime;
            if (timer >= 2.0)
            {
                gameObject.SetActive(false);
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
        if (Vector3.Distance(monsterStartPos , transform.position) <= 0.1f)
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
    
    private void Dead()
    {
        isDead = true;
        nav.Stop();
        rigid.velocity = Vector3.zero;
        //anim.SetTrigger("Dead");
    }
}
