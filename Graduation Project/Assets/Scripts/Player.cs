using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.PackageManager;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Tooltip("스피드 조정")]
    [SerializeField]
    private float _walkSpeed;
    [SerializeField]
    private float _runSpeed;
    [SerializeField]
    private float _crouchSpeed;
    
    [SerializeField]
    private float _applySpeed;//걷기와 뛰기 함수를 두개만들지말고 이동하는것에 속도를 적용한다.

    [Tooltip("점프 조정")]
    [SerializeField]
    private float _jumpForce;
    
    [Tooltip("플레이어 상태 변수")]
    private bool _isRun;
    private bool _isGround = true;
    private bool _isCrouch = false;
    private bool _isNearCar = false;
    [SerializeField]
    private bool _isRideCar = false;


    [Tooltip("자동차 탑승 쿨다운")]
    [SerializeField]
    private float _rideCoolDown = 1.0f;
    
    [Tooltip("얼마나 앉을건지")]
    [SerializeField]
    private float _crouchPosY;
    private float _originPosY;
    private float _applyCouchPosY;

    [SerializeField]
    private float _sensitivity = 15;
    

    private CapsuleCollider _capsuleCollider;

    private RaycastHit _hitInfo;
    
    
    private Rigidbody _playerRb;

    private Animator _playerAnim;
  
    
    [Tooltip("목 움직이기")]
    public Transform cameraTansform;
    private Transform _playerNeckTransform;
    private Vector3 _neckDir = new Vector3();

    [SerializeField] private Vector3 _neckOffset = new Vector3(0, 0, 0);
    
    private enum PlayerState
    {
        Idle,
        Walk,
        Run,
        Jump
    }


    private StateMachine _stateMachine;
    
    private Dictionary<PlayerState, IState> _stateDic = new Dictionary<PlayerState,IState>(); // 상태를 보관할 딕션어리

    public Transform rayCastPoint;

    // Start is called before the first frame update
    void Start()
    {
        
        
        //상태를 생성함 -> 새로운 상태가 생기면 추가하면됨
        IState idle = new StateIdle();
        IState walk = new StateWalk();
        IState run = new StateRun();
        IState jump = new StateJump();

        //상태를 딕션에 넣음
        
        _stateDic.Add(PlayerState.Idle,idle);
        _stateDic.Add(PlayerState.Walk, walk);
        _stateDic.Add(PlayerState.Run, run);
        _stateDic.Add(PlayerState.Jump, jump);
        
        _stateMachine = new StateMachine(idle);
        
        _applySpeed = _walkSpeed;
        _runSpeed = _walkSpeed * 3;
        _playerRb = gameObject.GetComponent<Rigidbody>();
        _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        _playerAnim = gameObject.GetComponent<Animator>();
        if (_playerAnim)
        {
            _playerNeckTransform = _playerAnim.GetBoneTransform(HumanBodyBones.Neck); //spine bone transform받아오기
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isRideCar)
        {
            IsGround();
            KeyboardInput();
            CharacterRotation();
            Move();
            _stateMachine.ExecuteUpdate();
        }

        if (_rideCoolDown > 0.0f)
        {
            _rideCoolDown -= Time.deltaTime;
            if (transform.rotation.z != 0)
            {
                Debug.Log("Get up!!");

                transform.rotation = Quaternion.Euler(new Vector3(0,0,0));

            }
        }
    }


    private void LateUpdate()
    {
        OperationBonRotate();
    }

    private void OperationBonRotate()
    {
        _neckDir = cameraTansform.position + cameraTansform.forward * 50;
        _playerNeckTransform.LookAt(_neckDir);
        _playerNeckTransform.rotation = _playerNeckTransform.rotation * Quaternion.Euler(_neckOffset); //상체 움직임 보정
    }

    void KeyboardInput()//키입력처리
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D))
        {
            
            _stateMachine.SetState(_stateDic[PlayerState.Walk]);
            
        }
        
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _stateMachine.SetState(_stateDic[PlayerState.Run]);
           

            Running();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            StopRunning();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _stateMachine.SetState(_stateDic[PlayerState.Jump]);

            if (_isGround)
            {
                Jump();
            }
        }

        if (Input.GetKeyDown(KeyCode.F)&&_rideCoolDown<=0)
        {
            
            IsNearCar();
        }

       
    }
    
    
    private void Move()
    {
       
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized *_applySpeed;

        _playerRb.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void Running()
    {
        _applySpeed = _runSpeed;
        
    }

    private void StopRunning()
    {
        _applySpeed = _walkSpeed;
    }
    
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _charcterRotationY = new Vector3(0f, _yRotation, 0f) * _sensitivity;
        _playerRb.MoveRotation(_playerRb.rotation*Quaternion.Euler(_charcterRotationY));
    }
    private void IsGround()
    {
        _isGround = Physics.Raycast(transform.position, Vector3.down, _capsuleCollider.bounds.extents.y + 0.1f);
    }
    private void Jump()
    {
        _playerRb.velocity = transform.up * _jumpForce;
    }

  

    private void IsNearCar()
    {
        _isNearCar = Physics.Raycast(rayCastPoint.transform.position, transform.forward,out _hitInfo,_capsuleCollider.bounds.extents.z + 1f);
        
        if (_isNearCar&&!_isRideCar)
        {
            _isRideCar = true;
            _capsuleCollider.enabled = false;
            _playerRb.isKinematic = true;
            _hitInfo.transform.GetComponent<CarController>().setCarControll(this);
            Debug.Log("Ride!!");
        }
    }

    public void TakeoffCar()
    {
        
        _rideCoolDown = 1.0f;
        _isRideCar = false;
        _capsuleCollider.enabled = true;
        _playerRb.isKinematic = false;

        
    }

    
}
