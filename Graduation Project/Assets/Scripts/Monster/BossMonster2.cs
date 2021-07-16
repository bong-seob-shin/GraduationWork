using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossMonster2 : MonsterManager
{
    public GameObject boss2_Wheel;
    public GameObject boss2_Door;
    public GameObject boss2_Head;

    public GameObject enemyPrefab;
    
    // 몬스터 소환 위치
    public Transform spawnPoint;
    
    // 레일의 가운데
    public Transform centerPoint;
    
    public float boss2_speed;
    public bool isOperate;
    public int phase = 0;
    public int randomPattern;
    public float dead_timer = 0;
    public bool isRotate;
    public bool isSpawn;

    // 공격 텀
    public float attackTime;
    public float currentAttackTime;
    
    // 전깃 줄 공격
    public GameObject electricWire;
    public GameObject[] electricWirePoints;
    
    // 장판 공격
    public GameObject boss2_ShootPoint;
    public GameObject boss2_BulletTargetPoint;
    public GameObject boss2_floorAttackBullet;
    
    // 총알 공격
    public GameObject boss2_bullet;
    public Collider[] TargetsColls;
    public GameObject Target;

    // 드론 소환
    public List<MonsterManager> spawnList = new List<MonsterManager>();
    
    // Start is called before the first frame update
    void Start()
    {
        isOperate = false;
        isDead = false;
        isRotate = true;

        this.MaxHP = 300;
        this.HP = MaxHP;
        this.armor = 100;
        // 몬스터 공격 시간
        currentAttackTime = attackTime;
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckSpawnMonsterDead();
        if (isOperate)
        {
            if (!isDead)
            {
                CalcPhase();

                if (phase == 1)
                {
                    transform.RotateAround(centerPoint.position, Vector3.up, boss2_speed * Time.deltaTime);
                    
                    attackTime = 10;

                    currentAttackTime -= Time.deltaTime;
                    if (currentAttackTime <= 0.0f)
                    {
                        //phaseFourthPattern();
                        phaseFirstPattern();
                        currentAttackTime = attackTime;
                    }
                }

                if (phase == 2)
                {
                    transform.RotateAround(centerPoint.position, Vector3.up, -boss2_speed * Time.deltaTime);
                    
                    attackTime = 8;
                    currentAttackTime -= Time.deltaTime;
                    if (currentAttackTime <= 0.0f)
                    {
                        randomPattern = Random.Range(0, 2);
                        if (randomPattern == 0)
                        {
                            phaseFirstPattern();
                        }

                        if (randomPattern == 1)
                        {
                            phaseSecondPattern();
                        }

                        currentAttackTime = attackTime;
                    }
                }

                if (phase == 3)
                {
                    transform.RotateAround(centerPoint.position, Vector3.up, boss2_speed * Time.deltaTime);
                    
                    List<int> randList = new List<int>(){0,1,2};
                    attackTime = 6;

                    currentAttackTime -= Time.deltaTime;
                    if (currentAttackTime <= 0.0f)
                    {
                        randomPattern = Random.Range(0, randList.Count);
                        if (randList[randomPattern] == 0)
                        {
                            phaseFirstPattern();
                        }

                        if (randList[randomPattern] == 1)
                        {
                            phaseSecondPattern();
                        }

                        if (randList[randomPattern] == 2)
                        {
                            phaseThirdPattern();
                        }
                        randList.RemoveAt(randomPattern);

                        int pattern2 = Random.Range(0, randList.Count);

                        if (randList[pattern2] == 0)
                        {
                            phaseFirstPattern();
                        }

                        if (randList[pattern2] == 1)
                        {
                            phaseSecondPattern();
                        }

                        if (randList[pattern2] == 2)
                        {
                            phaseThirdPattern();
                        }

                        currentAttackTime = attackTime;
                    }
                }

                if (phase == 4)
                {
                    transform.RotateAround(centerPoint.position, Vector3.up, -boss2_speed * 1.3f * Time.deltaTime);

                    attackTime = 4;
                    List<int> randList = new List<int>() {0, 1, 2, 3};

                    currentAttackTime -= Time.deltaTime;
                    if (currentAttackTime <= 0.0f)
                    {
                        randomPattern = Random.Range(0, randList.Count);
                        if (randList[randomPattern] == 0)
                        {
                            phaseFirstPattern();
                        }

                        if (randList[randomPattern] == 1)
                        {
                            phaseSecondPattern();
                        }

                        if (randList[randomPattern] == 2)
                        {
                            phaseThirdPattern();
                        }

                        if (randList[randomPattern] == 3)
                        {

                            phaseFourthPattern();
                        }

                        randList.RemoveAt(randomPattern);

                        int pattern2 = Random.Range(0, randList.Count);

                        if (randList[pattern2] == 0)
                        {
                            phaseFirstPattern();
                        }

                        if (randList[pattern2] == 1)
                        {
                            phaseSecondPattern();
                        }

                        if (randList[pattern2] == 2)
                        {
                            phaseThirdPattern();
                        }

                        if (randList[pattern2] == 3)
                        {
                            phaseFourthPattern();
                        }

                        currentAttackTime = attackTime;

                    }
                }
            }
            
            //hp가 0% 이하일 때 죽이자.
            if (isDead)
            {
                this.gameObject.AddComponent<Rigidbody>().useGravity = true;
                boss2_Wheel.transform.parent = null;
                boss2_Head.transform.parent = null;
                dead_timer += Time.deltaTime;
                if (dead_timer >= 3.0f)
                {
                    Destroy(this.gameObject);
                    Destroy(boss2_Wheel);
                    Destroy(boss2_Head);
                }
            }
            
        }
    }
    private void phaseFirstPattern()
    {
        if (spawnList.Count <= 4)
        {
            // 몬스터 소환
            DroneMonster droneMonster = Instantiate(enemyPrefab, spawnPoint.transform.position,
                spawnPoint.transform.rotation).GetComponent<DroneMonster>();
            spawnList.Add(droneMonster);
        }
    }

    private void phaseSecondPattern()
    {
        DetectTarget();

        Boss2Bullet boss2Bullet = Instantiate(boss2_bullet, boss2_ShootPoint.transform.position, Quaternion.identity)
            .GetComponent<Boss2Bullet>();
        boss2Bullet.target = Target.transform.position;
    }
    
    private void phaseThirdPattern()
    {
        for (int i = 0; i < electricWirePoints.Length; i++)
        {
            Boss2Laser laser = Instantiate(electricWire, electricWirePoints[i].transform.position, Quaternion.Euler(0.0f,0.0f,90.0f)).GetComponent<Boss2Laser>();
            laser.target = centerPoint.position;
        }
    }

    private void phaseFourthPattern()
    {
        Boss2FloorAttackBullet boss2FloorAttackBullet = Instantiate(boss2_floorAttackBullet, boss2_ShootPoint.transform.position, Quaternion.identity)
            .GetComponent<Boss2FloorAttackBullet>();
        boss2FloorAttackBullet.target = boss2_BulletTargetPoint.transform.position;
    }
    
    private void DetectTarget()
    {
        TargetsColls = Physics.OverlapSphere(transform.position, 100.0f);

        // 여기서 범위 안에 들어온 플레이어를 찾아서 Targets[] 에 append
        for (int i = 0; i < TargetsColls.Length; i++)
        {
            if (TargetsColls[i].tag == "Player")
            {
                Target = TargetsColls[i].gameObject;
            }
        }
    }
    
    private void CalcPhase()
    {
        float hpPer = ((float)((float)this.HP / (float)this.MaxHP) * 100.0f);
        if (hpPer <= 100.0f && hpPer >= 80.0f)
        {
            phase = 1;
        }
        if (hpPer < 80.0f && hpPer >= 50.0f)
        {
            phase = 2;
            
        }
        if (hpPer < 50.0f && hpPer >= 30.0f)
        {
            phase = 3;
        }
        if (hpPer < 30.0f && hpPer > 0.0f)
        {
            phase = 4;
        }
        // hp가 0이하일 때 ----> 죽자.
        if (hpPer <= 0.0f)
        {
            isDead = true;
        }
    }
    
    private void CheckSpawnMonsterDead()
    {
        for(int i = spawnList.Count-1; i>=0; i--)
        {
            if (spawnList[i].isDead)
            {
                Debug.Log("dead");
                spawnList.Remove(spawnList[i]);
            }
        }
    }
    
}
