using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OhterPlayer : NoneControlObj
{
   [Tooltip("스피드 조정")]
   public float runSpeed;
    public float crouchSpeed;
    
    
    public float applySpeed;//걷기와 뛰기 함수를 두개만들지말고 이동하는것에 속도를 적용한다.

    [Tooltip("점프 조정")]
    [SerializeField]
    public float jumpForce;
    
    [Tooltip("플레이어 상태 변수")]
    [HideInInspector]public bool isRun;
    [HideInInspector]public bool isGround = true;
    [HideInInspector]public bool isCrouch = false;
    [HideInInspector]public bool isNearCar = false;
    
    private bool _isRideCar = false;


    
   
    
    [Tooltip("얼마나 앉을건지")]
    private float _crouchPosY;
    private float _originPosY;
    private float _applyCouchPosY;

    [SerializeField]
    private float _sensitivity = 15;
    

    private CapsuleCollider _capsuleCollider;

    private RaycastHit _hitInfo;
    
    
    public Rigidbody playerRb;

    
    
    [Tooltip("목 움직이기")]
    private Transform _cameraTansform;
    private Transform _playerNeckTransform;
    private Vector3 _neckDir = new Vector3(0,0,0);

    
    
    [SerializeField] private Vector3 _neckOffset = new Vector3(0, 0, 0);
    
    private enum PlayerState
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
        
        destPos = new Vector3(0,0,0);
        
        applySpeed = speed;
        runSpeed = speed * 3;
        
        playerRb = gameObject.GetComponent<Rigidbody>();
        _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        anim = gameObject.GetComponent<Animator>();
        if (anim)
        {
            _playerNeckTransform = anim.GetBoneTransform(HumanBodyBones.Neck); //spine bone transform받아오기
        }

        _cameraTansform = GetComponentInChildren<Camera>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isRideCar)
        {
            IsGround();
            CharacterRotation();
            Move();
          
        }

    }


    private void LateUpdate()
    {
        OperationBonRotate();
    }

    private void OperationBonRotate()
    {
        _neckDir = _cameraTansform.position + _cameraTansform.forward * 50;
        _playerNeckTransform.LookAt(_neckDir);
        _playerNeckTransform.rotation = _playerNeckTransform.rotation * Quaternion.Euler(_neckOffset); //상체 움직임 보정
    }

   
    
    
  
    
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _charcterRotationY = new Vector3(0f, _yRotation, 0f) * _sensitivity;
        playerRb.MoveRotation(playerRb.rotation*Quaternion.Euler(_charcterRotationY));
    }
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, _capsuleCollider.bounds.extents.y + 0.1f);
    }
  

  

  

    
}
