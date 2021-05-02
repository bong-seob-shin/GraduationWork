using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class OhterPlayer : NoneControlObj
{
   [Tooltip("스피드 조정")]
   public float runSpeed;
    public float crouchSpeed;
    
    
    public float applySpeed;//걷기와 뛰기 함수를 두개만들지말고 이동하는것에 속도를 적용한다.

    [Tooltip("점프 조정")]
    [SerializeField]
    public float jumpForce = 5;
    
    [Tooltip("플레이어 상태 변수")]
    [HideInInspector]public bool isRun;
    [HideInInspector]public bool isGround = true;
    [HideInInspector]public bool isCrouch = false;
    [HideInInspector]public bool isNearCar = false;
    [HideInInspector] public AnimState state = AnimState.Idle;

    public bool isJump;
    private bool _isRideCar = false;
    
    
    [Tooltip("얼마나 앉을건지")]
    private float _crouchPosY;
    private float _originPosY;
    private float _applyCouchPosY;

   
    
    private CapsuleCollider _capsuleCollider;

    private RaycastHit _hitInfo;
    
    
    public Rigidbody playerRb;

    private IKGunGrab _gunGrab;
    
    [Tooltip("목 움직이기")]
    //private Transform targetTransform; //서버에서 현재 목위치를 전달 받아야함
    private Transform _playerNeckTransform;
    private Vector3 _neckDir = new Vector3(0,0,0);


    private bool _isGunEject = false; //총 꺼내는 애니메이션 반복을 막기 위한 변수
  
    
    //총 드는거는 ikGunGrab 스크립트의 isgrab 이 총 드는 스크립트인데  true가 되면 총을 잡음 이걸 이용해서 플레이어에서 isgrab이 true될 때 넘기고 서버에서  그걸 넘겨 받으면 될 것 같음 총 여러개 되었을 때는 enum으로 해서 바꿔주면 될거같음
    
    
    [SerializeField] private Vector3 _neckOffset = new Vector3(0, 0, 0);
    
    [HideInInspector] public enum AnimState //상태는 오브젝트마다 다를거같아서 따로 관리하는게 좋을것 같음
    {
        Idle,
        Walk,
        Run,
        Jump
    }

    private void Awake()
    {
        playerRb = gameObject.GetComponent<Rigidbody>();
        _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        anim = gameObject.GetComponent<Animator>();
        _gunGrab = GetComponent<IKGunGrab>(); // isgrab 받아오기 위해서 받아옴
        weaponGunAnim = currentWeapon.GetComponent<Animation>();//애니메이션 변수 가져옴
    }

    // Start is called before the first frame update
    void Start()
    {
        
        speed = 3.0f;
        
        destPos = transform.position;//위치 조정을 여기서 해주면될듯함
        
        applySpeed = speed;
        runSpeed = speed * 3;
        
        
        if (anim)
        {
            _playerNeckTransform = anim.GetBoneTransform(HumanBodyBones.Neck); //spine bone transform받아오기
        }

       
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isRideCar)
        {
            Move();
            AnimUpdate();
            if (isJump)
            {
                Jump();
            }

            if (isEquipWeapon)//총 꺼낼때 애니메이션 및 처리
            {
                currentWeapon.SetActive(true);
                _gunGrab.isGrabed = true;
                if (!_isGunEject)
                {
                    weaponGunAnim.Play("GunEject");
                    _isGunEject = true;
                }
            }
            else
            {
                _gunGrab.isGrabed = false;
                currentWeapon.SetActive(false);
                _isGunEject = false;
            }

            if (_gunGrab.isGrabed) //무기를 쥐고 있을 때 shoot 상태변수를 받아와서 이게 true이면 쏘는 애니메이션을 보여주도록하자
            {
                myGun.isShoot = isShoot;
            }
        }

    }

    private void Jump()
    {
        anim.SetTrigger("Jump");
        isJump = false;
    }

    private void AnimUpdate()
    {
        anim.SetFloat("MoveDirX", dirX);
        anim.SetFloat("MoveDirZ", dirZ);
    }

    
    private void LateUpdate()
    {
       // OperationBonRotate();
    }

    // private void OperationBonRotate()
    // {
    //     _neckDir = targetTransform.position + targetTransform.forward * 50; //  받아온 vector
    //     _playerNeckTransform.LookAt(_neckDir);
    //     _playerNeckTransform.rotation = _playerNeckTransform.rotation * Quaternion.Euler(_neckOffset); //상체 움직임 보정
    // }

   
    
    
  
    
    
    
  

  

  

    
}
