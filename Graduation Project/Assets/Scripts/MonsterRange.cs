using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;

public class MonsterRange : MonoBehaviour
{
    
    [SerializeField] private NavMeshAgent nav;

    [SerializeField] private Rigidbody rigid;
    
    //보류
    [SerializeField] private CapsuleCollider meshCollider;

    private Vector3 monsterStartPos;

    private bool alreadyAttacked = false;
    [SerializeField] private float attackTerm;

    public Collider[] colls;
    
    // 상태값
    public bool isDead = false;
    public bool isHit = false;
    private bool isAttack = false;

    private float timer;

    public GameObject projectile;

    private Vector3 bulletStartPos;

    private Transform target;
    private bool targetOn;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        meshCollider = GetComponent<CapsuleCollider>();
        nav = GetComponent<NavMeshAgent>();

        bulletStartPos = GetComponentInChildren<VisualEffect>().transform.position;
        
        monsterStartPos = transform.position;

        nav.enabled = true;
        // 플레이어 타겟 잡는 곳
        targetOn = false;

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
        Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 10f , ForceMode.Impulse);
        rb.AddForce(transform.up * 5f , ForceMode.Impulse);

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
    
    private void Dead()
    {
        isDead = true;
        nav.Stop();
        rigid.velocity = Vector3.zero;
        //anim.SetTrigger("Dead");
    }
}
