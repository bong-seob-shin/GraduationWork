using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Climb : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private bool isUpMove =false;

    public Vector3 Rayoffset;
    // Start is called before the first frame update
    void Start()
    {
        _player = Player.Instance;
        this.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        Climbing();
    }
    
    void Climbing()
    {

        Vector3 _moveHorizontal = transform.right * Input.GetAxis("Horizontal");
        Vector3 _moveVertical = transform.up * Input.GetAxis("Vertical");
        
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized *_player.applySpeed;
        
        _player.playerRb.position += _velocity * Time.deltaTime;

    
        if (_player.isGround &&isUpMove&&Input.GetKey(KeyCode.S))
        {
            _player.isClimbing = false;
            _player.playerRb.useGravity = true;

            isUpMove = false;
            this.enabled = false;

        }  
        if (Input.GetKey(KeyCode.W))
        {
            isUpMove = true;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lever"))
        {
            
            _player.isClimbing = false;
            _player.playerRb.useGravity = true;
            this.enabled = false;
        }

      
    }
}
