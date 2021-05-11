using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private float _lookSensitivity = 15;


    [SerializeField] //카메라 상하 제한 각 
    private float _cameraRotationLimit = 45;
    
    private float _currentCameraRotationX = 0;
    // Start is called before the first frame update

    public Vector3 camDeadoffset;

    private bool isSetThridCam = false;
    public Animator playeranim;
    private Rigidbody camRg;
    private void Awake()
    {
        playeranim = GetComponentInParent<Animator>();
        camRg = GetComponent<Rigidbody>();
       
        
    }

  
    // Update is called once per frame
    void Update()
    {
        Player _player = Player.Instance;
        CameraRotation();
       
        
    }

    private void LateUpdate()
    {
        Player _player = Player.Instance;
        Transform neckTransform = playeranim.GetBoneTransform(HumanBodyBones.Head);
        if (!_player.isDead)
        {
            transform.position = neckTransform.position;
        }
        if(_player.isDead)
        {
          
                transform.localPosition =  new Vector3(camDeadoffset.x, camDeadoffset.y, camDeadoffset.z);
           
        }
    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * _lookSensitivity;
        _currentCameraRotationX -= _cameraRotationX;
        _currentCameraRotationX = Mathf.Clamp(_currentCameraRotationX, -_cameraRotationLimit, _cameraRotationLimit);
        
        transform.localEulerAngles = new Vector3(_currentCameraRotationX,0f,0f);
    }
    
    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _charcterRotationY = new Vector3(0f, _yRotation, 0f) * _lookSensitivity;
        camRg.MoveRotation(camRg.rotation*Quaternion.Euler(_charcterRotationY));
    }
}
