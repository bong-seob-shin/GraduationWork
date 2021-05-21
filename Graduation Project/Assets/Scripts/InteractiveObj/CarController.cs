using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : NoneAnimationObj
{

    public static IDictionary<int, CarController> carList= new Dictionary<int, CarController>(); //모든 차 리스트를 담아 놓았음 (id, 객체) 객체를 생성할때 add를 해주자!, 이것을 이용해서 아더가 서버에서 전달받은 차번호로 차를 탈 수있다.
    
    public Vector3 centerOfMass;    
    
    [Tooltip("자동차 속도 및 방향")]
    private float _currentSteeringAngle;
    private float _currentBreakForce;
    [SerializeField]
    private float _decelerationForce = 500;
    [SerializeField]
    private float _motorForce = 1000;

    [SerializeField] private float _breakForce = 3000;
    [SerializeField] private float _maxSteeringAngle = 30;

    private Rigidbody _carRigid;
    
    private bool isBreaking;
  
    [SerializeField]
    private bool _isCarStartControll = false; // 차가 현재 컨트롤 되고 있는지 변수

    [Tooltip("자동차 탑승 쿨다운")]
    [SerializeField]
    private float _rideCoolDown = 1.0f;

    public Camera carCamera;

    public Transform takeOffPos;
    [HideInInspector] public WheelCollider frontRightWheelCollider;
    [HideInInspector] public WheelCollider frontLeftWheelCollider;
    [HideInInspector] public WheelCollider RearRightWheelCollider;
    [HideInInspector] public WheelCollider RearLeftWheelCollider;
    
    [HideInInspector] public Transform  frontRightWheelTransform;
    [HideInInspector] public Transform  frontLeftWheelTransform;
    [HideInInspector] public Transform  RearRightWheelTransform;
    [HideInInspector] public Transform  RearLeftWheelTransform;

    public GameObject CarHandle;
    private AnimationObj _driver; //
    public int usingUserId = -1; //누가 타고있는지을 알려주는 변수 모든차들에게 브로드캐스팅해서 넘겨줘야함 값이 변경될때마다



    public float constrictHandleAngle = 60.0f;
    [SerializeField]private float currentHandleAngle = 0;
    
    private void Awake()
    {
        _carRigid = GetComponent<Rigidbody>();
        
        _carRigid.centerOfMass = centerOfMass;
        this.id = 1;//지금은 id 1로줬음 생성될때 아이디를 줘야함
        carList.Add(id, this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (_isCarStartControll)
        {
            if (_driver.isPlayer) //_driver객체가 플레이어일때
            {
                KeyboardInput();
            }
            else//아닐때
            {
                //dirx diry를 가진 패킷받기
                
                //update otherplayer's car info //아더플레이어가 움직인 차정보를 동기화 하는 코드가 필요함 
                //핸들의 회전값들이나 이런게 필요가없긴함
                //필요하다면 위치x,y와 rotation값을 주고받아도되는데 내생각에는 dirx와 diry를 넘겨주면 알아서 코드상에서 자동차가 그거에 따라서 움직일것이라고 생각함
                //그래서 dirX와 dirY를 동기화 해야함
                //요약------> dirx dirY(carController의)를 보내고 받자
                //봐야되는스크립트 otherplayer, animationobj, carctr

                _driver.rideCarID = usingUserId;//임시 해결책(1)
                if (_driver.rideCarID < 0) //아더가 rideCarID 를 -1로 전달받으면 차에서 내리게됨 이부분이 애매한데 아더의 ridecarID의 값을 서버에서 받아서 바꿔주고 싶지만
                                           //other스크립트가 꺼져있어 거기서 패킷을 주고 받을 수가없음 해결책(1)은 usingUserID를 차가 계속 동기화 해서 이것을 rideCarID에다가 넣어주는것임 [87번줄]
                {
                    _isCarStartControll = false;
            
                    _driver.gameObject.SetActive(true);

                }
            }
            Move();
            Handling();
            UpdateWheel();
            if (_rideCoolDown > 0)
            {
                _rideCoolDown -= Time.deltaTime;
            }
          
        }
        else//자동 감속
        {
            frontLeftWheelCollider.brakeTorque = _decelerationForce;
            frontRightWheelCollider.brakeTorque = _decelerationForce;
            RearLeftWheelCollider.brakeTorque = _decelerationForce;
            RearRightWheelCollider.brakeTorque = _decelerationForce;
        }
        
        
    }
    
    protected override void Move()
    {


        if (dirZ != 0f)
        {
            RearLeftWheelCollider.motorTorque = dirZ * _motorForce;
            RearRightWheelCollider.motorTorque = dirZ * _motorForce;
            
            frontLeftWheelCollider.brakeTorque = 0;
            frontRightWheelCollider.brakeTorque =0;
            RearLeftWheelCollider.brakeTorque = 0;
            RearRightWheelCollider.brakeTorque = 0;
        }
        else
        {
            
            frontLeftWheelCollider.brakeTorque = _decelerationForce;
            frontRightWheelCollider.brakeTorque = _decelerationForce;
            RearLeftWheelCollider.brakeTorque = _decelerationForce;
            RearRightWheelCollider.brakeTorque = _decelerationForce;
        }

        _currentBreakForce = isBreaking ? _breakForce : 0f;

        if (isBreaking)
        {
           ApplyBreaking();
        }
        
    
  
        _driver.transform.rotation = transform.rotation;
        _driver.transform.position = takeOffPos.position;
    }


    private void ApplyBreaking()
    {
        frontLeftWheelCollider.brakeTorque = _currentBreakForce;
        frontRightWheelCollider.brakeTorque = _currentBreakForce;
        RearLeftWheelCollider.brakeTorque = _currentBreakForce;
        RearRightWheelCollider.brakeTorque = _currentBreakForce;
    }

    private void Handling()
    {
        _currentSteeringAngle = _maxSteeringAngle * dirX;
        frontLeftWheelCollider.steerAngle = _currentSteeringAngle;
        frontRightWheelCollider.steerAngle = _currentSteeringAngle;

       
        currentHandleAngle += _currentSteeringAngle/10.0f;

        if (dirX == 0)
        {
            Debug.Log("dirx =0");
            if (currentHandleAngle > 5)
            {
                Debug.Log("curangle >5");
                currentHandleAngle -=  _maxSteeringAngle/10.0f;
                CarHandle.transform.Rotate(Vector3.forward, -_maxSteeringAngle/10.0f);
                
            }
            if (currentHandleAngle < -5)
            {
                currentHandleAngle +=  _maxSteeringAngle/10.0f;
                CarHandle.transform.Rotate(Vector3.forward, _maxSteeringAngle/10.0f);
                
            }
        }
        if ( currentHandleAngle <= constrictHandleAngle && currentHandleAngle >= -constrictHandleAngle)
        {
            CarHandle.transform.Rotate(Vector3.forward, _currentSteeringAngle/10.0f);
        }

        if (currentHandleAngle > constrictHandleAngle)
        {
            currentHandleAngle = constrictHandleAngle;
        }
        if (currentHandleAngle < -constrictHandleAngle)
        {
            currentHandleAngle = -constrictHandleAngle;
        }

       
    }

   
    private void UpdateWheel()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(RearLeftWheelCollider, RearLeftWheelTransform);
        UpdateSingleWheel(RearRightWheelCollider, RearRightWheelTransform);

    }

    private void UpdateSingleWheel(WheelCollider wheelCol, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        
        wheelCol.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }
    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.F)&&_rideCoolDown<=0)
        {
            
            _isCarStartControll = false;
            
            _driver.gameObject.SetActive(true);
            

            _rideCoolDown = 1;
            usingUserId = -1;
            //내리기전에 여기서 패킷을 한번더 보내서 usingUserID를 동기화해준다.
            Debug.Log("TakeOff!");
            if (_driver.isPlayer)
            {
                _driver.gameObject.GetComponent<Player>().TakeoffCar();
                carCamera.gameObject.SetActive(false);
            }
            
        }
        
        dirX = Input.GetAxisRaw("Horizontal");
        dirZ = Input.GetAxisRaw("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
        
        
        
    }
    public void setCarControll(Player player)
    {
        _driver = player;
        _driver.gameObject.GetComponent<Player>().myCam.gameObject.SetActive(false);
        _driver.gameObject.SetActive(false);
        carCamera.gameObject.SetActive(true);
        _isCarStartControll = true;
        usingUserId = _driver.id;//여기서 변경됨

    }
    public void setOtherCarControll(OhterPlayer other)
    {
        _driver = other;
        _driver.gameObject.SetActive(false);
        _isCarStartControll = true;
        usingUserId = _driver.id;//여기서 변경됨
    }
    
}
