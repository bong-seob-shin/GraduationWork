using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbLadder : MonoBehaviour
{
     private Player _player;
    [SerializeField]
    private bool isUpMove =false;

    public IKCCD leftHand;

    public IKCCD rightHand;

    public IKCCD leftFoot;
    public IKCCD rightFoot;

    
    public Transform[] targetLeftHandTransforms;
    public Transform[] targetLeftFootTransforms;
    public Transform[] targetRightHandTransforms;
    public Transform[] targetRightFootTransforms;

    // Start is called before the first frame update
    void Start()
    {
        _player = Player.Instance;
        this.enabled = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (targetLeftHandTransforms.Length != 0)
        {

            if (!leftHand.isIKOn)
            {
                int index = 0;
                float temp = (leftHand.transform.position - targetLeftHandTransforms[0].position).sqrMagnitude;
                for (int i = 0; i < targetLeftHandTransforms.Length - 1; i++)
                {
                    float dist = (leftHand.transform.position - targetLeftHandTransforms[i].position).sqrMagnitude;

                    if (temp > dist)
                    {
                        index = i;
                        temp = dist;
                    }
                }


                {
                    leftHand.SetTarget(targetLeftHandTransforms[index].position,
                        targetLeftHandTransforms[index].rotation);
                }
            }
        }

        if (targetRightHandTransforms.Length != 0)
        {

            if (!rightHand.isIKOn)
            {
                int index = 0;
                float temp = (rightHand.transform.position - targetRightHandTransforms[0].position).sqrMagnitude;
                for (int i = 0; i < targetRightHandTransforms.Length - 1; i++)
                {
                    float dist = (rightHand.transform.position - targetRightHandTransforms[i].position).sqrMagnitude;

                    if (temp > dist)
                    {
                        index = i;
                        temp = dist;
                    }
                }

                {
                    rightHand.SetTarget(targetRightHandTransforms[index].position,
                        targetRightHandTransforms[index].rotation);
                }

            }
        }

        if (targetLeftFootTransforms.Length != 0)
        {

           // if (!leftFoot.isIKOn)
            {
                int index = 0;
                float temp = (leftFoot.transform.position - targetLeftFootTransforms[0].position).sqrMagnitude;
                for (int i = 0; i < targetLeftFootTransforms.Length - 1; i++)
                {
                    float dist = (leftFoot.transform.position - targetLeftFootTransforms[i].position).sqrMagnitude;

                    if (temp > dist)
                    {
                        index = i;
                        temp = dist;
                    }
                }

                if (leftFoot.targetPos != Vector3.zero)
                {
                
                    Vector3 targetPos = Vector3.Lerp(leftFoot.targetPos, targetLeftFootTransforms[index].position,
                        Time.deltaTime * 5);
                    leftFoot.SetTarget(targetPos, targetLeftFootTransforms[index].rotation);
                
                }
                else
                {
                    leftFoot.SetTarget(targetLeftFootTransforms[index].position, targetLeftFootTransforms[index].rotation);
                }
            }
        }

        if(targetRightFootTransforms.Length != 0){
           // if (!rightFoot.isIKOn)
            {
                int index = 0;
                float temp = (rightFoot.transform.position - targetRightFootTransforms[0].position).sqrMagnitude;
                for (int i = 0; i < targetRightFootTransforms.Length - 1; i++)
                {
                    float dist = (rightFoot.transform.position - targetRightFootTransforms[i].position).sqrMagnitude;

                    if (temp > dist)
                    {
                        index = i;
                        temp = dist;
                    }
                }

                if (rightFoot.targetPos != Vector3.zero)
                {
                    Vector3 targetPos = Vector3.Lerp(rightFoot.targetPos, targetRightFootTransforms[index].position,
                        Time.deltaTime*5);
                    rightFoot.SetTarget(targetPos,targetRightFootTransforms[index].rotation);
                }
                else
                {
                    rightFoot.SetTarget(targetRightFootTransforms[index].position,
                        targetRightFootTransforms[index].rotation);
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
            targetLeftFootTransforms = new Transform[0];
            targetRightHandTransforms = new Transform[0];
            targetRightFootTransforms = new Transform[0];
            targetLeftHandTransforms = new Transform[0];

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
                transform.Translate(Vector3.up*2f+Vector3.forward*0.01f);

        }
    }
}
