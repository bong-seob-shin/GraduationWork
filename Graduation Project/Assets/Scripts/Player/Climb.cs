using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Climb : MonoBehaviour
{
    private Player _player;
    [SerializeField]
    private bool isUpMove =false;

    public IKCCD leftHand;

    public IKCCD rightHand;

    public IKCCD leftFoot;
    public IKCCD rightFoot;

    public Transform[] targetTransforms;
    // Start is called before the first frame update
    void Start()
    {
        _player = Player.Instance;
        this.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (targetTransforms.Length != 0)
        {

            if (!leftHand.isIKOn)
            {
                int index = 0;
                float temp = (leftHand.transform.position - targetTransforms[0].position).sqrMagnitude;
                for (int i = 0; i < targetTransforms.Length - 1; i++)
                {
                    float dist = (leftHand.transform.position - targetTransforms[i].position).sqrMagnitude;

                    if (temp > dist)
                    {
                        index = i;
                        temp = dist;
                    }
                }

                leftHand.SetTarget(targetTransforms[index]);
            }
            
            if (!rightHand.isIKOn)
            {
                int index = 0;
                float temp = (rightHand.transform.position - targetTransforms[0].position).sqrMagnitude;
                for (int i = 0; i < targetTransforms.Length - 1; i++)
                {
                    float dist = (rightHand.transform.position - targetTransforms[i].position).sqrMagnitude;

                    if (temp > dist)
                    {
                        index = i;
                        temp = dist;
                    }
                }

                rightHand.SetTarget(targetTransforms[index]);
            }
            
            
            //if (!leftFoot.isIKOn)
            {
                int index = 0;
                float temp = (leftFoot.transform.position - targetTransforms[0].position).sqrMagnitude;
                for (int i = 0; i < targetTransforms.Length - 1; i++)
                {
                    float dist = (leftFoot.transform.position - targetTransforms[i].position).sqrMagnitude;

                    if (temp > dist)
                    {
                        index = i;
                        temp = dist;
                    }
                }

                leftFoot.SetTarget(targetTransforms[index]);
            }
            
            //if (!rightFoot.isIKOn)
            {
                int index = 0;
                float temp = (rightFoot.transform.position - targetTransforms[0].position).sqrMagnitude;
                for (int i = 0; i < targetTransforms.Length - 1; i++)
                {
                    float dist = (rightFoot.transform.position - targetTransforms[i].position).sqrMagnitude;

                    if (temp > dist)
                    {
                        index = i;
                        temp = dist;
                    }
                }

                rightFoot.SetTarget(targetTransforms[index]);
            }
        }

        Climbing();
    }
    
    void Climbing()
    {

        Vector3 _moveHorizontal = transform.right * Input.GetAxis("Horizontal");
        Vector3 _moveVertical = transform.up * Input.GetAxis("Vertical");
        
        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized *_player.speed/3;
        
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
            targetTransforms = new Transform[0];
            this.enabled = false;
            
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WallTop"))
        {
            if(_player.isClimbing)
                transform.Translate(Vector3.up*2f+Vector3.forward*0.5f);

        }
    }
}
