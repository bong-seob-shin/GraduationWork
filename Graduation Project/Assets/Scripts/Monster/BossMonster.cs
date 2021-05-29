﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using Random = UnityEngine.Random;

public class BossMonster : MonsterManager
{
    public GameObject first_face;
    public GameObject second_face;
    public GameObject third_face;
    public GameObject fourth_face;

    public GameObject second_leftArm;
    public GameObject second_rightArm;
    public GameObject third_leftArm;
    public GameObject third_rightArm;

    public GameObject leftLeg;
    public GameObject rightLeg;
    
    // 보스 총알
    public GameObject bossBullet;
    public GameObject electricWire;
    
    // 몬스터 소환 위치
    public Transform leftArmMonsterSpawnPoint;
    public Transform rightArmMonsterSpawnPoint;

    // 총알 발사 위치
    public Transform leftArmShootPoint;
    public Transform rightArmShootPoint;
    
    // 소환할 몬스터 프리팹
    public GameObject[] enemyPrefabs;
    public GameObject portalPrefab;
    
    // 몬스터 소환 텀
    public float MonsterCreateTime;
    public float currentMonsterCreateTime;
    
    // 플레이어 배열
    public GameObject Target;
    public Collider[] TargetsColls;
    
    // 랜덤으로 Targets[]에서 뽑아서 최종 타겟이 될거임.
    private Transform leftArmTarget;
    private Transform rightArmTarget;

    // 공격 텀
    public float attackTime;
    public float currentAttackTime;

    // 보스 상태
    public bool isOperate;
    public bool isDead;
    public bool targetDetect;
    public int phase = 0;
    
    //     블럭이 위에서부터 하나씩 떼어짐 - 단계별로
    //     떼질때마다 아래칸에 눈이 생김 - 색은 변하던 말던 
    //
    //     첫번째칸은 아직 미정
    //     두번째팔에서는 어그로 정하고 (제일 가까운 놈) 플레이어 위치 계산 후 그 위치로 에너지볼 발사. 
    //     세번째팔에서는 원거리랑 근거리 몬스터를 계속 소환 ( 최대 개수 정해놓고 그만큼 계속 소환 ) 텀 10초.
    //     마지막은 발만 있고 얘는 전깃줄을 쏘는데 이걸 플레이어가 점프로 피해야함.
    //
    //     체력 100 - 80퍼 사이는 몬스터 소환만   -   눈이 색 없음
    //     체력 80퍼 이하로는 투사체 공격  -   이때 깨니까 . 초록색
    //     체력 50퍼 이하로는 모든 패턴 다 나옴.
    //     체력 30퍼이하 되면 광폭화로 바뀜 모델만 있으면 플레인만 프리팹으로 교체해서 얼굴 보여줌  - 빨강
    //
    //     광폭화 됬을때 면으로된 공격을 하는데 가운데에는 코어가 박혀있음. 이걸 부셔서 일렬로 패턴을 피하는 방식임.
    
    
    // 생각해봐야 할 부분
    // 몬스터 최대 개수만큼 생성해야되는데 이건 어떻게 할까 ?
    // 보스몬스터 파츠별로 콜라이더와 리지드 바디를 넣어야되나 ?
    
    
    // Update is called once per frame
    void Start()
    {
        isOperate = false; // 플레이어를 찾아내면 isOperate
        isDead = false; // 처음이니까 안죽어있을거고
        targetDetect = false;

        this.MaxHP = 1000;
        this.HP = MaxHP;
        
        // 몬스터 소환 시간
        currentMonsterCreateTime = MonsterCreateTime;
        
        // 몬스터 투사체 공격 시간
        currentAttackTime = attackTime;


    }

    void Update()
    {
        
        // 보스가 활성화 되지 않았을 때 ? --> 플레이어를 발견하면 isOperate를 True로 바꿔주어서 활성화시킨다.
        if (!isOperate)
        {
            TargetsColls = Physics.OverlapSphere(transform.position, 50.0f);
            for (int i = 0; i < TargetsColls.Length; i++)
            {
                if (TargetsColls[i].tag == "Player")
                {
                    isOperate = true;
                    phase = 1;
                }
            }
        }
        // 플레이어가 보스 사거리에 들어갔을 떄 ? isOperate 활성화  --> 활성화 되어야 보스가 움직이기 시작하게
        if (isOperate)
        {
            // isDead가 False일 때니까 살아있는것임.
            if (!isDead)
            {
                CalcPhase();
                
                // 몬스터 소환만 하는 패턴
                if (phase == 1)
                {
                    phaseFirstPattern();
                }

                if (phase == 2)
                {
                    phaseSecondPattern();
                }


            }
            
            //hp가 0% 이하일 때 죽이자.
            if (isDead)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // 얘는 다리가 쏠꺼야 
    private void ElectricWire()
    {
        
    }

    private void phaseFirstPattern()
    {
        currentMonsterCreateTime -= Time.deltaTime;
        if (currentMonsterCreateTime <= 0.0f)
        {
            int randomEnemy = Random.Range(0,enemyPrefabs.Length);
            // 왼쪽 팔이 소환할 놈들
            Instantiate(portalPrefab, leftArmMonsterSpawnPoint.transform.position,leftArmMonsterSpawnPoint.transform.rotation);
            Instantiate(enemyPrefabs[randomEnemy], leftArmMonsterSpawnPoint.transform.position,leftArmMonsterSpawnPoint.transform.rotation);
            
            // 오른쪽 팔이 소환할 놈들
            Instantiate(portalPrefab, rightArmMonsterSpawnPoint.transform.position,rightArmMonsterSpawnPoint.transform.rotation);
            Instantiate(enemyPrefabs[randomEnemy], rightArmMonsterSpawnPoint.transform.position,rightArmMonsterSpawnPoint.transform.rotation);

            currentMonsterCreateTime = MonsterCreateTime;
        }
    }

    private void phaseSecondPattern()
    {
        DetectTarget();
        currentAttackTime -= Time.deltaTime;

        if (currentAttackTime <= 0.0f)
        {
            // 날아갈 위치
            Vector3 movePos = Target.transform.position;
            Rigidbody = Instantiate(bossBullet, leftArmShootPoint.transform.position, leftArmShootPoint.transform.rotation).GetComponent<Rigidbody>();
            Rigidbody rightBullet = Instantiate(bossBullet, rightArmShootPoint.transform.position, rightArmShootPoint.transform.rotation).GetComponent<Rigidbody>();
            
            StartCoroutine(MoveTo(leftBullet, movePos));
            currentAttackTime = attackTime;
        }
    }

    IEnumerator MoveTo(GameObject a, Vector3 toPos)
    {
        float count = 0;
        Vector3 wasPos = a.transform.position;
        while (true)
        {
            count += Time.deltaTime;
            a.transform.position = Vector3.Lerp(wasPos, toPos, count);

            if (count >= 1)
            {
                a.transform.position = toPos;
                break;
            }
            yield return null;
        }
    }

    private void DetectTarget()
    {
        TargetsColls = Physics.OverlapSphere(transform.position, 50.0f);

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
        Debug.Log(phase);
        float hpPer = (float)((float)this.HP / (float)this.MaxHP * (float)100.0f);
        // hp가 80% 이상 100% 이하일 때  = 1페이즈  ----> first_face 분리 후 제거
        if (hpPer <= 100.0f && hpPer >= 80.0f)
        {
            phase = 1;
        }
        // hp가 50% 이상 80% 미만일 때 = 2페이즈   ----> second_face 분리 후 제거  몬스터 소환만 하는 패턴
        if (hpPer < 80.0f && hpPer >= 50.0f)
        {
            phase = 2;
            DestroyParts(first_face , 2.0f);
        }
        // hp가 30% 이상 50% 미만일 때 = 3페이즈    ---->  third_face 분리 후 제거  투사체 공격하는 패턴
        if (hpPer < 50.0f && hpPer >= 30.0f)
        {
            phase = 3;
            DestroyParts(second_face ,2.0f);
        }
        // hp가 0% 초과 30% 미만일 때 = 4페이즈(광폭화)  ----> 0프로 이하되면 죽음   2,3 패턴 둘다
        if (hpPer < 30.0f && hpPer > 0.0f)
        {
            phase = 4;
            DestroyParts(third_face, 2.0f);
        }
        // hp가 0이하일 때 ----> 죽자.
        if (hpPer <= 0.0f)
        {
            isDead = true;
        }
    }

    private void DestroyParts(GameObject parts , float time)
    {
        parts.transform.parent = null;
        parts.AddComponent<Rigidbody>();
        Rigidbody partsRb = parts.GetComponent<Rigidbody>();
        partsRb.useGravity = true;
        partsRb.AddForce(parts.transform.forward * 0.5f , ForceMode.Impulse);
        partsRb.AddForce(transform.up * 2f , ForceMode.Impulse);
        
        float timer = time;
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
        {
            Destroy(parts);
        }
    }
}
