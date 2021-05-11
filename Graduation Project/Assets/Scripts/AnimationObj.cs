using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationObj : ObjManager //애니메이션을 하는 오브젝트를 관리함
{
    [HideInInspector]public Animator anim;

    public bool isEquipWeapon; //이게 true이면 무기를 장착중인것이고 아니면 맨손인것으로 하면 좋을 것같고 enum으로 해도 됌 weapon type같은걸로 그리고 이 변수가 바뀔때 마다 센드를 해주면될듯

    [Tooltip("총 관리해주는 변수들")]
    public GameObject currentWeapon; //현재 가지고 있는 무기 변수

    public Gun myGun;
    [HideInInspector]public Animation weaponGunAnim; // 무기 애니메이션을 실행하기 위한 애니메이션 변수

    [HideInInspector] public bool isShoot = false; // 총을 쏘고있다는 것을 알리는 변수


    [HideInInspector] public bool isDead = false;
    
    public int rideCarID = -1; // 이건 cellpos에 있어야함! 그래야 아더가 차를 알아서 탈 수 있음
}
