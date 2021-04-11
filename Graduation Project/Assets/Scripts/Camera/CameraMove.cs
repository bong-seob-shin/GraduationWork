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
    

    // Update is called once per frame
    void Update()
    {
        CameraRotation();
    }
    
    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y");
        float _cameraRotationX = _xRotation * _lookSensitivity;
        _currentCameraRotationX -= _cameraRotationX;
        _currentCameraRotationX = Mathf.Clamp(_currentCameraRotationX, -_cameraRotationLimit, _cameraRotationLimit);
        
        transform.localEulerAngles = new Vector3(_currentCameraRotationX,0f,0f);
    }
}
