using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject boss2_bullet;
    
    

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
                if (isRotate)
                {
                    transform.RotateAround(centerPoint.position, Vector3.up, boss2_speed * Time.deltaTime);
                }

                if (phase == 1)
                {
                    currentAttackTime -= Time.deltaTime;
                    if (currentAttackTime <= 0.0f)
                    {
                        // phaseFirstPattern();
                        //phaseThirdPattern();
                        phaseFourthPattern();
                        currentAttackTime = attackTime;
                    }
                }

                if (phase == 2)
                {
                    
                }
                
            }
            
            //hp가 0% 이하일 때 죽이자.
            if (isDead)
            {
                dead_timer += Time.deltaTime;
                if (dead_timer >= 3.0f)
                {
                    Destroy(this.gameObject);
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
        Boss2Bullet boss2Bullet = Instantiate(boss2_bullet, boss2_ShootPoint.transform.position, Quaternion.identity)
            .GetComponent<Boss2Bullet>();
        boss2Bullet.target = boss2_BulletTargetPoint.transform.position;
    }
    
    private void CalcPhase()
    {
        float hpPer = ((float)((float)this.HP / (float)this.MaxHP) * 100.0f);
        // hp가 80% 이상 100% 이하일 때  = 1페이즈  ----> first_face 분리 후 제거
        if (hpPer <= 100.0f && hpPer >= 80.0f)
        {
            phase = 1;
        }
        // hp가 50% 이상 80% 미만일 때 = 2페이즈   ----> second_face 분리 후 제거  몬스터 소환만 하는 패턴
        if (hpPer < 80.0f && hpPer >= 50.0f)
        {
            phase = 2;
            
        }
        // hp가 30% 이상 50% 미만일 때 = 3페이즈    ---->  third_face 분리 후 제거  투사체 공격하는 패턴
        if (hpPer < 50.0f && hpPer >= 30.0f)
        {
            phase = 3;
        }
        // hp가 0% 초과 30% 미만일 때 = 4페이즈(광폭화)  ----> 0프로 이하되면 죽음   2,3 패턴 둘다
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
