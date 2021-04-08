using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    public Vector3 centerOfMass;    
    
    [Tooltip("자동차 속도 및 방향")]
    private float _moveDirX;
    private float _moveDirZ;
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
    private bool _isCarStartControll = false;

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
    
    private Player _driver;

    
    

    private void Awake()
    {
        _carRigid = GetComponent<Rigidbody>();
        
        _carRigid.centerOfMass = centerOfMass;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (_isCarStartControll)
        {
            
            KeyboardInput();
            Move();
            Handling();
            UpdateWheel();
            if (_rideCoolDown > 0)
            {
                _rideCoolDown -= Time.deltaTime;
            }
        }
        else
        {
            frontLeftWheelCollider.brakeTorque = _decelerationForce;
            frontRightWheelCollider.brakeTorque = _decelerationForce;
            RearLeftWheelCollider.brakeTorque = _decelerationForce;
            RearRightWheelCollider.brakeTorque = _decelerationForce;
        }
    }
    
    private void Move()
    {


        if (_moveDirZ != 0f)
        {
            RearLeftWheelCollider.motorTorque = _moveDirZ * _motorForce;
            RearRightWheelCollider.motorTorque = _moveDirZ * _motorForce;
            
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
        _currentSteeringAngle = _maxSteeringAngle * _moveDirX;
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
            _driver.TakeoffCar();
            
            _rideCoolDown = 1;
            Debug.Log("TakeOff!");
        }
        
        _moveDirX = Input.GetAxisRaw("Horizontal");
        _moveDirZ = Input.GetAxisRaw("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
        
        
        
    }
    public void setCarControll(Player player)
    {
        _driver = player;
        _driver.gameObject.SetActive(false);
        carCamera.gameObject.SetActive(true);
        _isCarStartControll = true;
    }
    
    
}
