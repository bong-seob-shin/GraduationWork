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

            //if (!leftHand.isIKOn)
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

                if (leftHand.targetPos != Vector3.zero)
                {
                    Vector3 targetPos = Vector3.Lerp(leftHand.targetPos, targetTransforms[index].position,
                        Time.deltaTime * 5);
                    leftHand.SetTarget(targetPos,targetTransforms[index].rotation);
                }
                else
                {
                    leftHand.SetTarget(targetTransforms[index].position,targetTransforms[index].rotation);
                }
            }
            
            //if (!rightHand.isIKOn)
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
                if (rightHand.targetPos != Vector3.zero)
                {
                    Vector3 targetPos = Vector3.Lerp(rightHand.targetPos, targetTransforms[index].position,
                        Time.deltaTime * 5);
                    rightHand.SetTarget(targetPos,targetTransforms[index].rotation);
                }
                else
                {
                    rightHand.SetTarget(targetTransforms[index].position,targetTransforms[index].rotation);
                }
               
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

                if (leftFoot.targetPos != Vector3.zero)
                {
                    
                    Vector3 targetPos = Vector3.Lerp(leftFoot.targetPos, targetTransforms[index].position,
                        Time.deltaTime*5);
                    leftFoot.SetTarget(targetPos, targetTransforms[index].rotation);

                }
                else
                {
                    leftFoot.SetTarget(targetTransforms[index].position,targetTransforms[index].rotation);
                }
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

                if (rightFoot.targetPos != Vector3.zero)
                {
                    Vector3 targetPos = Vector3.Lerp(rightFoot.targetPos, targetTransforms[index].position,
                        Time.deltaTime*5);
                    rightFoot.SetTarget(targetPos,targetTransforms[index].rotation);
                }
                else
                {
                    rightFoot.SetTarget(targetTransforms[index].position,targetTransforms[index].rotation);
                }
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
            leftHand.targetPos = new Vector3();
            rightHand.targetPos = new Vector3();
            leftFoot.targetPos = new Vector3();
            rightFoot.targetPos = new Vector3();

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
