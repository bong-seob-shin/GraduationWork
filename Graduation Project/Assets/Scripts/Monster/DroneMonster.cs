using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using Random = UnityEngine.Random;

public class DroneMonster : MonsterManager
{
    //보류
    public MeshCollider meshCollider;

    public GameObject Missile;
    public GameObject missilePos;
    private Vector3 missleStartPos;

    public GameObject[] droneWing;
    public int wingRotationSpeed;

    public float droneVelocity;
    public float maximumHeight;

    public Collider[] colls;
    
    private Vector3 monsterStartPos;

    private bool alreadyAttacked = false;
    [SerializeField] private float attackTerm;
    // 상태값
    private bool isAttack = false;
    private float timer = 2.0f;

    public Transform target;
    public bool targetOn;

    // 패트롤
    public float range = 50.0f;
    
    public float patrolTime = 3.0f;
    private float currentPatrolTime;
    private float minX, maxX, minZ, maxZ;

    private Vector3 moveSpot;

    private bool isPatrol;
    
    // 맞았을 때 타이머
    private float currentAgroTime;
    private float agroTime = 5.0f;
    public bool agroOn = false;

    public bool patrolOn = false;


    public Animator anim;
    
    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();

        //missleStartPos = missilePos.transform.position;
        
        monsterStartPos = transform.position;

        maximumHeight = Random.Range(5, 20);
        
        // 플레이어 타겟 잡는 곳
        targetOn = false;

        this.MaxHP = 150;
        this.HP = this.MaxHP;
        this.armor = 30;
        
        GetPatrolRange();
        //moveSpot = GetNewPosition();
        currentPatrolTime = patrolTime;
        
        currentAgroTime = agroTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            anim.SetBool("Idle", true);
            // 날개 네개를 돌리는것이다
            for (int i = 0; i < droneWing.Length; i++)
            {
                droneWing[i].GetComponent<Transform>().Rotate(new Vector3(0, Time.deltaTime * wingRotationSpeed, 0),Space.World);
            }

            RaycastHit hit;
            if (Physics.Raycast(new Vector3(this.transform.position.x,this.transform.position.y,this.transform.position.z),Vector3.down, out hit,100.0f))
            {
                if (hit.collider != null)
                {
                    // if (Mathf.Abs(this.transform.position.y - hit.point.y) != maximumHeight)
                    // {
                    //     transform.position = Vector3.MoveTowards(transform.position,
                    //         new Vector3(transform.position.x, hit.point.y + (float)maximumHeight, transform.position.z),
                    //         droneVelocity * Time.deltaTime);
                    //     // transform.position = new Vector3(transform.position.x,
                    //     //     transform.position.y + droneVelocity * Time.deltaTime,
                    //     //     transform.position.z);
                    //     patrolOn = false;
                    // }
                    transform.position = Vector3.MoveTowards(transform.position,
                        new Vector3(transform.position.x, hit.point.y + (float)maximumHeight, transform.position.z),
                        droneVelocity * Time.deltaTime);
                    // transform.position = new Vector3(transform.position.x,
                    //     transform.position.y + droneVelocity * Time.deltaTime,
                    //     transform.position.z);
                    patrolOn = false;
                    if(Mathf.Abs(this.transform.position.y - hit.point.y) <= maximumHeight || Mathf.Abs(this.transform.position.y - hit.point.y) >= maximumHeight)
                    {
                        patrolOn = true;
                    }
                    
                }
                
                colls = Physics.OverlapSphere(hit.point, 40.0f);
                for (int i = 0; i < colls.Length; i++)
                {
                    if (colls[i].tag == "Player")
                    {
                        target = colls[i].transform;
                        targetOn = true;
                        break;
                    }
                }
            }

            if (targetOn)
            {
                TraceTarget();
                if (agroOn)
                {
                    currentAgroTime -= Time.deltaTime;
                    if (currentAgroTime <= 0.0f && Vector3.Distance(target.position, new Vector3(transform.position.x,hit.point.y,transform.position.z)) > 40)
                    {
                        StopTrace();
                        currentAgroTime = agroTime;
                        agroOn = false;
                        targetOn = false;
                    }
                }
                else
                {
                    if (Vector3.Distance(target.position, new Vector3(transform.position.x,hit.point.y,transform.position.z)) > 40)
                    {
                        StopTrace();
                    }
                }


                if (Vector3.Distance(target.position, new Vector3(transform.position.x,hit.point.y,transform.position.z)) <= 3.0f)
                {
                    if (!isAttack)
                    {
                        AttackTarget();
                    }
                }
            }



            if (!targetOn && patrolOn)
            {
                WatchYourStep();
                GetToStepping();
            }
            
            if (this.HP <= 0)
            {
                Dead();
            }
        }
        
        if (isDead)
        {
            rigid.useGravity = true;
            rigid.AddForce(Vector3.down * 400.0f,ForceMode.Acceleration);
            
            
            timer -= Time.deltaTime;
            if (timer <= 0.0)
            {
                Destroy(this.gameObject);
                timer = 5.0f;
            }

        }
    }


    private void TraceTarget()
    {
        
        Vector3 targetDirection = new Vector3(target.transform.position.x,transform.position.y,target.transform.position.z) - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0.3f, 0f);
        newDirection = new Vector3(newDirection.x, 90.0f, newDirection.z);
        transform.rotation = Quaternion.LookRotation(newDirection);

        transform.position =
            Vector3.MoveTowards(transform.position,
                new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z),
                droneVelocity * Time.deltaTime);
    }

    private void StopTrace()
    {
        targetOn = false;
        patrolOn = true;
        
        Vector3 targetDirection = new Vector3(monsterStartPos.x,transform.position.y,monsterStartPos.z) - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0.3f, 0f);
        newDirection = new Vector3(newDirection.x, 90.0f, newDirection.z);
        transform.rotation = Quaternion.LookRotation(newDirection);
        
        transform.position =
            Vector3.MoveTowards(transform.position,
                new Vector3(monsterStartPos.x, transform.position.y, monsterStartPos.z),
                droneVelocity * Time.deltaTime);
        
        // if (Vector3.Distance(new Vector3(monsterStartPos.x,transform.position.y,monsterStartPos.z) , transform.position) <= 0.1f)
        // {
        //     targetOn = false;
        // }
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
        transform.position =
            Vector3.MoveTowards(transform.position, moveSpot,
                3.0f * Time.deltaTime);
        currentPatrolTime -= Time.deltaTime;
        if (currentPatrolTime <= 0.0f || Vector3.Distance(transform.position, moveSpot) <= 0.2f)
        {
            moveSpot = GetNewPosition();
            currentPatrolTime = patrolTime;
        }
    }

    private void WatchYourStep()
    {
        Vector3 targetDirection = new Vector3(moveSpot.x,transform.position.y,moveSpot.z) - transform.position;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, 0.3f, 0f);
        newDirection = new Vector3(newDirection.x, 90.0f, newDirection.z);
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void AttackTarget()
    {
        if (!alreadyAttacked)
        {
            anim.SetTrigger("Attack");
            Fire();
        }
    }
    
    private void Fire()
    {
        Instantiate(Missile, missilePos.transform.position, Quaternion.identity);
        
        alreadyAttacked = true;
        Invoke(nameof(ResetAttack),attackTerm);
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
