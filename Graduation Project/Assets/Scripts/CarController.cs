using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private float _carSpeed = 50f;

    private Rigidbody _carRb;

    [SerializeField]
    private bool _isCarStartControll = false;

    [Tooltip("자동차 탑승 쿨다운")]
    [SerializeField]
    private float _rideCoolDown = 1.0f;

    
    private Player _driver;
    // Start is called before the first frame update
    void Start()
    {
        _carRb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isCarStartControll)
        {
            Move();
            KeyboardInput();

            if (_rideCoolDown > 0)
            {
                _rideCoolDown -= Time.deltaTime;
            }
        }
    }
    
    private void Move()
    {
       
        float _moveDirX = Input.GetAxisRaw("Horizontal");
        float _moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized *_carSpeed;

        _carRb.MovePosition(transform.position + _velocity * Time.deltaTime);
        _driver.transform.rotation = transform.rotation;
        _driver.transform.position = transform.position;
    }

    private void KeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.F)&&_rideCoolDown<=0)
        {
            _rideCoolDown = 1;
            _isCarStartControll = false;
            _driver.TakeoffCar();
            _driver = null;
            Debug.Log("TakeOff!");
        }
    }
    public void setCarControll(Player player)
    {
        _driver = player;
        _isCarStartControll = true;
    }
    
    
}
