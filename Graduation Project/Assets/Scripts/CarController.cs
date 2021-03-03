using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{

    [Tooltip("자동차 속도 및 방향")]
    private float _moveDirX;
    private float _moveDirZ;
    private float _currentSteeringAngle;
    private float _currentBreakForce;
    [SerializeField]
    private float _decelerationForce;
    [SerializeField]
    private float _motorForce;

    [SerializeField] private float _breakForce;
    [SerializeField] private float _maxSteeringAngle;
    
    
    private bool isBreaking;
  
    [SerializeField]
    private bool _isCarStartControll = false;

    [Tooltip("자동차 탑승 쿨다운")]
    [SerializeField]
    private float _rideCoolDown = 1.0f;

    public Camera carCamera;
    
    
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider RearRightWheelCollider;
    [SerializeField] private WheelCollider RearLeftWheelCollider;
    
    [SerializeField] Transform  frontRightWheelTransform;
    [SerializeField] Transform  frontLeftWheelTransform;
    [SerializeField] Transform  RearRightWheelTransform;
    [SerializeField] Transform  RearLeftWheelTransform;
    
    private Player _driver;

    [SerializeField]
    private Quaternion _driverOriginRotation;
    // Start is called before the first frame update
   

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
            frontLeftWheelCollider.motorTorque = _moveDirZ * _motorForce;
            frontRightWheelCollider.motorTorque = _moveDirZ * _motorForce;
            
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
        _driver.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
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
        _moveDirX = Input.GetAxisRaw("Horizontal");
        _moveDirZ = Input.GetAxisRaw("Vertical");
        isBreaking = Input.GetKey(KeyCode.Space);
        
        if (Input.GetKeyDown(KeyCode.F)&&_rideCoolDown<=0)
        {
            _rideCoolDown = 1;
            _isCarStartControll = false;
            _driver.transform.position = new Vector3(transform.position.x+5,transform.position.y, transform.position.z);
            carCamera.gameObject.SetActive(false);
            _driver.gameObject.SetActive(true);
            _driver.TakeoffCar();
           
            Debug.Log("TakeOff!");
        }

       

    }
    public void setCarControll(Player player)
    {
        _driver = player;
        _driverOriginRotation = player.transform.rotation;
        _driver.gameObject.SetActive(false);
        carCamera.gameObject.SetActive(true);
        _isCarStartControll = true;
    }
    
    
}
