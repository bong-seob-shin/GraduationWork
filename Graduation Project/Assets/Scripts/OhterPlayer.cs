using System.Collections;
using System.Collections.Generic;
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

    
    
    [Tooltip("목 움직이기")]
    private Transform targetTransform; //서버에서 현재 목위치를 전달 받아야함
    private Transform _playerNeckTransform;
    private Vector3 _neckDir = new Vector3(0,0,0);

    
    
    [SerializeField] private Vector3 _neckOffset = new Vector3(0, 0, 0);
    
    [HideInInspector] public enum AnimState //상태는 오브젝트마다 다를거같아서 따로 관리하는게 좋을것 같음
    {
        Idle,
        Walk,
        Run,
        Jump
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
        speed = 3.0f;
        
        destPos = transform.position;//위치 조정을 여기서 해주면될듯함
        
        applySpeed = speed;
        runSpeed = speed * 3;
        
        playerRb = gameObject.GetComponent<Rigidbody>();
        _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        anim = gameObject.GetComponent<Animator>();
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
           
            AnimUpdate();
            if (isJump)
            {
                Jump();
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

    private void OperationBonRotate()
    {
        _neckDir = targetTransform.position + targetTransform.forward * 50;
        _playerNeckTransform.LookAt(_neckDir);
        _playerNeckTransform.rotation = _playerNeckTransform.rotation * Quaternion.Euler(_neckOffset); //상체 움직임 보정
    }

   
    
    
  
    
    
    
  

  

  

    
}
