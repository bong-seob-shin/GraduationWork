using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

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
    [HideInInspector]public bool isInteract = false;
    [HideInInspector] public bool isJump = false;
    [HideInInspector] public bool onInteractKey = false;

    [Tooltip("상호작용 버튼 쿨다운")]
    [SerializeField]
    private float _interactCoolDown = 0.0f;

    public bool isOnLift = false;
    
    // [Tooltip("얼마나 앉을건지")]
    // private float _crouchPosY;
    // private float _originPosY;
    // private float _applyCouchPosY;

    public float sensitivity = 15;
    

    private CapsuleCollider _capsuleCollider;
    public SphereCollider camCol;
    
    
    private RaycastHit _hitInfo;
    
    
    public Rigidbody playerRb;


    private IKGunGrab _gunGrab;

    
    [Tooltip("목 움직이기")]
    private Transform _cameraTansform;
    private Transform _playerSpineTransform;
    private Vector3 _spineDir = new Vector3(0,0,0);



    [HideInInspector]public Camera myCam;
    
    [SerializeField] private Vector3 _neckOffset = new Vector3(0, 0, 0);

    public GameObject CenterUI;

    private float invincibilityTime = 0.0f;
    public Image hitImage;


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


    public TextMeshProUGUI hpText;
    public TextMeshProUGUI interactText;

    public Transform cheatPos;

    private UIManager _uiManager;
    private void Awake()
    {
        
        this.id = 1;//차 타는거 테스트
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
        myGun.bulletText.text = "Bullet  " +myGun. bulletCount.ToString() + " / " + myGun.maxBulletCount.ToString();
        isPlayer = true;
        cheatPos =  GameObject.FindWithTag("deongunTransform").GetComponent<Transform>();
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
        
        MaxHP = 100;
        HP = MaxHP;
        applySpeed = speed;
        runSpeed = speed * 3;
        
        if (anim)
        {
            _playerSpineTransform = anim.GetBoneTransform(HumanBodyBones.Spine); //spine bone transform받아오기
        }

        _cameraTansform = GetComponentInChildren<Camera>().transform;
        _uiManager = UIManager.instance;

    }

    // Update is called once per frame
    void Update()
    {
        if (rideCarID<=0)
        {
            //IsGround();
            KeyboardInput();
            Interact();
            //CharacterRotation();
            //Move();
            //_stateMachine.ExecuteUpdate();
        }

        if (_interactCoolDown > 0.0f)
        {
            _interactCoolDown -= Time.deltaTime;
            if (transform.rotation.z != 0)
            {
                Debug.Log("Get up!!");

                transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
                
            }
        }

        hpText.text = "HP  " + ((int)HP).ToString() +" / "+((int)MaxHP).ToString();
        Debug.DrawRay(transform.position, Vector3.down*(_capsuleCollider.bounds.extents.y/5.0f), Color.blue);

        if (invincibilityTime > 0)
        {
            invincibilityTime -= Time.deltaTime;
            var tempColor = hitImage.color;
            tempColor.a = invincibilityTime;
            hitImage.color = tempColor;
        }

        if (HP <= 0)
        {
            HP = 0;
            anim.SetBool("Dead", true);
            _capsuleCollider.direction = 2;
            camCol.enabled = false;
            isDead = true;
            
            _gunGrab.isGrabed = false;

            currentWeapon.SetActive(false);

            myGun.bulletText.gameObject.SetActive(false);
        }
    }

 

    private void FixedUpdate()//물리적인 충돌을 계산하기위해서 움직임등을 모두 fixedupdate에 넣음 이 update는 매 프레임마다 불림
    {
        if (rideCarID<=0 && !isDead)
        {
            IsGround();
            
            CharacterRotation();
            Move();
            _stateMachine.ExecuteUpdate();
        }
    }

    private void LateUpdate()
    {
        if (!isDead)
        {
            OperationBonRotate();
        }
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



        if (!isDead)
        {
            if (w_keyPress || a_keyPress || s_keyPress || d_keyPress)
            {
                _stateMachine.SetState(_stateDic[PlayerState.Walk]);

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    _stateMachine.SetState(_stateDic[PlayerState.Run]);

                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _stateMachine.SetState(_stateDic[PlayerState.Jump]);
            
            }

            if (Input.GetKeyDown(KeyCode.F)&&_interactCoolDown<=0)
            {
                onInteractKey = true;
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                isEquipWeapon = true; //이 변수를 보내자
            
            
                myGun.bulletText.gameObject.SetActive(true);//ui setActive

                currentWeapon.SetActive(true);
                _gunGrab.isGrabed = true;
                weaponGunAnim.Play("GunEject");
                myGun.currentFireRate = 1.0f;
                _uiManager.centerPoint.SetActive(true);

            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {           
                isEquipWeapon = false; //이 변수를 보내자

                _gunGrab.isGrabed = false;

                currentWeapon.SetActive(false);

                myGun.bulletText.gameObject.SetActive(false);
                
                _uiManager.centerPoint.SetActive(false);
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

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_gunGrab.isGrabed)
                {
                    myGun.Reload();
                }
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                transform.position = cheatPos.position;
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                HP = MaxHP;
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
       
    }
    
    
    protected override void Move()
    {

        Vector3 _moveHorizontal = transform.right * dirX;
        Vector3 _moveVertical = transform.forward * dirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized *applySpeed;

        //playerRb.MovePosition(transform.position + _velocity * Time.deltaTime);
        playerRb.position += _velocity * Time.deltaTime;
        //playerRb.position += _velocity * Time.deltaTime;
    }

    public void hit(int damage)
    {
        if (invincibilityTime <= 0)
        {
            HP -= damage;
            Debug.Log(transform.name + ":" + HP);
            invincibilityTime = 1.0f;
        }
    }
    
    
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _charcterRotationY = new Vector3(0f, _yRotation, 0f) * sensitivity;
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
  

  

    private void Interact()
    {
        int layerMask = (-1) - (1 << LayerMask.NameToLayer("PlayerCamera"));
        isInteract = Physics.Raycast(myCam.transform.position,  myCam.transform.forward,out _hitInfo,_capsuleCollider.bounds.extents.z + 1f,layerMask);
        Debug.DrawRay(myCam.transform.position,  myCam.transform.forward,Color.blue);
        
        if (isInteract)
        {
            CarController car = _hitInfo.transform.GetComponent<CarController>();
            InteractiveButton ib = _hitInfo.transform.GetComponent<InteractiveButton>();
            InteractiveDoubleButton idb = _hitInfo.transform.GetComponent<InteractiveDoubleButton>();
            InteractiveLiftButton ilb = _hitInfo.transform.GetComponent<InteractiveLiftButton>();
            if ( car != null && rideCarID<=0 )
            {
                if (onInteractKey)
                {
                    if (car.usingUserId < 0)
                    {
                        interactText.text = " ";

                        CenterUI.SetActive(false);
                        _capsuleCollider.enabled = false;
                        playerRb.isKinematic = true;
                        car.setCarControll(this);
                        rideCarID = car.id; //여기서 넣은 ridecarID를 서버에 보내서 아더가 받으면 차를타게된다.
                        onInteractKey = false;
                    }
                }
                else
                {
                    interactText.text = "Interact Key 'F'";
                    Debug.Log(_hitInfo.transform.name);
                }
            }

            if (ib != null )
            {
                if (onInteractKey)
                {
                    ib.InteractObjs();
                    //버튼ik관련코드
                    //ib.setIKLeft(ikLeftHandGrab);
                    //ikLeftHandGrab.leftHandPos = ib.transform;
                    //ikLeftHandGrab.isGrabed = true;
                    Debug.Log("불렸음");
                    _interactCoolDown = 1.0f;
                    onInteractKey = false;
                }
                else
                {
                    interactText.text = "Interact Key 'F'";
                    Debug.Log(_hitInfo.transform.name);
                }
            }
            if (idb != null )
            {
                if (onInteractKey)
                {
                    idb.InteractObjs();
                    Debug.Log("불렸음");
                    _interactCoolDown = 1.0f;
                    onInteractKey = false;
                }
                else
                {
                    interactText.text = "Interact Key 'F'";
                    Debug.Log(_hitInfo.transform.name);
                }
            }
            if (ilb != null )
            {
                if (onInteractKey)
                {
                    ilb.InteractObjs();
                    Debug.Log("불렸음");
                    _interactCoolDown = 1.0f;
                    onInteractKey = false;
                }
                else
                {
                    interactText.text = "Interact Key 'F'";
                    Debug.Log(_hitInfo.transform.name);
                }
            }
        }
        else
        {
            interactText.text = " ";
        }
        onInteractKey = false;
    }

    public void TakeoffCar()
    {
        CenterUI.SetActive(true);
        _interactCoolDown = 1.0f;
        _capsuleCollider.enabled = true;
        playerRb.isKinematic = false;
        dirX = 0;
        dirZ = 0;
        myCam.gameObject.SetActive(true);
        rideCarID = -1; // 차를 내릴때 불리는 함수에서 rideCarID를 디폴트값으로 수정한다.

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Trap"))
        {
            hit(10);
        }
    }
}
