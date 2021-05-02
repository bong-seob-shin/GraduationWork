using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : NoneAnimationObj
{

    public static IDictionary<int, CarController> carList= new Dictionary<int, CarController>(); //모든 차 리스트를 담아 놓았음 (id, 객체)
    
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
    
    private AnimationObj _driver;
    public int usingUserId = 1; // 원래 디폴트 값은 -1로 둘예정이지만 싱글모드에서 확인용으로 1을 디폴트로 주었다. 서버에서는 이값을 브로드캐스팅해서 모든 차들에게 전달해서 업데이트 해주면된다.
    
    
    

    private void Awake()
    {
        _carRigid = GetComponent<Rigidbody>();
        
        _carRigid.centerOfMass = centerOfMass;
        this.id = 1;//지금은 id 1로줬음
        carList.Add(id, this);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (_isCarStartControll)
        {
            if (_driver.id == usingUserId)
            {
                KeyboardInput();
            }
            else
            {
                //update otherplayer's car info //아더플레이어가 움직인 차정보를 동기화 하는 코드가 필요함 
                //dirX와 dirY를 동기화 해야함
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
            carCamera.gameObject.SetActive(false);
            _driver.gameObject.SetActive(true);
            _driver.gameObject.GetComponent<Player>().TakeoffCar();
            
            _rideCoolDown = 1;
            Debug.Log("TakeOff!");
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
        
    }
    public void setOtherCarControll(OhterPlayer player)
    {
        _driver = player;
        _driver.gameObject.SetActive(false);
        _isCarStartControll = true;
        
    }
    
}
