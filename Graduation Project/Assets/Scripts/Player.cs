using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Player : AnimationObj
{
    private static Player instance = null;
    
    [Tooltip("스피드 조정")]
    
    public float runSpeed;
    public float crouchSpeed;
    
    
    public float applySpeed;//걷기와 뛰기 함수를 두개만들지말고 이동하는것에 속도를 적용한다.

    [Tooltip("점프 조정")]
    [SerializeField]
    public float jumpForce;
    
    [Tooltip("플레이어 상태 변수")]
    [HideInInspector]public bool isRun;
    public bool isGround = true;
    [HideInInspector]public bool isCrouch = false;
    [HideInInspector]public bool isNearCar = false;
    [HideInInspector] public bool isJump = false;
    public bool _isRideCar = false;


    [Tooltip("자동차 탑승 쿨다운")]
    [SerializeField]
    private float _rideCoolDown = 1.0f;
    
    [Tooltip("얼마나 앉을건지")]
    private float _crouchPosY;
    private float _originPosY;
    private float _applyCouchPosY;

    [SerializeField]
    private float _sensitivity = 15;
    

    private CapsuleCollider _capsuleCollider;

    private RaycastHit _hitInfo;
    
    
    public Rigidbody playerRb;


    private IKGunGrab _gunGrab;
    
    [Tooltip("목 움직이기")]
    private Transform _cameraTansform;
    private Transform _playerSpineTransform;
    private Vector3 _spineDir = new Vector3(0,0,0);

  
    
    [HideInInspector]public Camera myCam;
    
    [SerializeField] private Vector3 _neckOffset = new Vector3(0, 0, 0);
    
    private enum PlayerState
    {
        Idle,
        Walk,
        Run,
        Jump
    }

    [Tooltip("키입력 변수")] 
    [HideInInspector] public bool w_keyPress = false;
    [HideInInspector] public bool a_keyPress = false;
    [HideInInspector] public bool s_keyPress = false;
    [HideInInspector] public bool d_keyPress = false;
    private StateMachine _stateMachine;
    
    private Dictionary<PlayerState, IState> _stateDic = new Dictionary<PlayerState,IState>(); // 상태를 보관할 딕션어리

    public Transform rayCastPoint;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        anim = gameObject.GetComponent<Animator>(); 
        playerRb = gameObject.GetComponent<Rigidbody>();
        _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        _gunGrab = GetComponentInChildren<IKGunGrab>();
        myCam = GetComponentInChildren<Camera>();
        myCam.gameObject.GetComponent<CameraMove>().enabled = true; //갑자기 생긴 버그 때문에 고치기위해서 카메라무브 스크립트를 게임 시작하면 켜줌
        weaponGunAnim = currentWeapon.GetComponent<Animation>();

    }

    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                return null;
            }

            return instance;
        }
    }

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

        speed = 3.0f;
        
        applySpeed = speed;
        runSpeed = speed * 3;
        
        if (anim)
        {
            _playerSpineTransform = anim.GetBoneTransform(HumanBodyBones.Spine); //spine bone transform받아오기
        }

        _cameraTansform = GetComponentInChildren<Camera>().transform;
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
        
        Debug.DrawRay(transform.position, Vector3.down*(_capsuleCollider.bounds.extents.y/5.0f), Color.blue);
    }


    private void LateUpdate()
    {
        OperationBonRotate();
    }

    private void OperationBonRotate()
    {
        _spineDir = _cameraTansform.position + _cameraTansform.forward * 50;
        _playerSpineTransform.LookAt(_spineDir);
        _playerSpineTransform.rotation = _playerSpineTransform.rotation * Quaternion.Euler(_neckOffset); //상체 움직임 보정
    }

    void KeyboardInput()//키입력처리
    {
        _stateMachine.SetState(_stateDic[PlayerState.Idle]);
        
        w_keyPress = false;
        a_keyPress = false;
        s_keyPress = false;
        d_keyPress = false;

        w_keyPress = Input.GetKey(KeyCode.W);
        a_keyPress = Input.GetKey(KeyCode.A);
        s_keyPress = Input.GetKey(KeyCode.S);
        d_keyPress = Input.GetKey(KeyCode.D);


       
        if (w_keyPress || a_keyPress || s_keyPress || d_keyPress)
        {
            _stateMachine.SetState(_stateDic[PlayerState.Walk]);
            
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _stateMachine.SetState(_stateDic[PlayerState.Run]);
                
            }
        }


        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S))
        {
            dirZ = 0;
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            dirX = 0;
        }
        
        if (w_keyPress && s_keyPress)
        {
            dirZ = 0;
        }
       
        if (a_keyPress && d_keyPress)
        {
            dirX = 0;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _stateMachine.SetState(_stateDic[PlayerState.Jump]);
            
        }

        if (Input.GetKeyDown(KeyCode.F)&&_rideCoolDown<=0)
        {
            IsNearCar();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isEquipWeapon = true; //이 변수를 보내자

            currentWeapon.SetActive(true);
            _gunGrab.isGrabed = true;
            weaponGunAnim.Play("GunEject");
            myGun.currentFireRate = 1.0f;

        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {           
            isEquipWeapon = false; //이 변수를 보내자

            _gunGrab.isGrabed = false;

            currentWeapon.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            applySpeed = speed / 2.0f;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            applySpeed = speed;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isShoot = true;//여기서 총쏘고 있다는 것을 샌드하자
            myGun.isShoot = isShoot;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isShoot = false;//여기서 총쏘고 있다는 것을 샌드하자
            myGun.isShoot = isShoot;
        }
    }
    
    
    protected override void Move()
    {

        Vector3 _moveHorizontal = transform.right * dirX;
        Vector3 _moveVertical = transform.forward * dirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized *applySpeed;

        playerRb.MovePosition(transform.position + _velocity * Time.deltaTime);

        
    }
    
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _charcterRotationY = new Vector3(0f, _yRotation, 0f) * _sensitivity;
        playerRb.MoveRotation(playerRb.rotation*Quaternion.Euler(_charcterRotationY));
    }
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, _capsuleCollider.bounds.extents.y/5.0f);
        isJump = true;
        if (isGround && !isJump)
        {
            anim.ResetTrigger("Jump");
        }
        
    }
  

  

    private void IsNearCar()
    {
        isNearCar = Physics.Raycast(rayCastPoint.transform.position, transform.forward,out _hitInfo,_capsuleCollider.bounds.extents.z + 1f);

        if (isNearCar)
        {
            if (_hitInfo.transform.GetComponent<CarController>() != null && !_isRideCar)
            {
                _isRideCar = true;
                _capsuleCollider.enabled = false;
                playerRb.isKinematic = true;
                _hitInfo.transform.GetComponent<CarController>().setCarControll(this);
                Debug.Log("Ride!!");
            }
        }
    }

    public void TakeoffCar()
    {
        
        _rideCoolDown = 1.0f;
        _isRideCar = false;
        _capsuleCollider.enabled = true;
        playerRb.isKinematic = false;
        dirX = 0;
        dirZ = 0;
        myCam.gameObject.SetActive(true);
    }

    
}
