using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKGunGrab : MonoBehaviour
{
    public Transform leftHandPos;
    public Transform RightHandPos;
    
    
    public bool isGrabed = false;
    
    private Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame

    private void OnAnimatorIK(int layerIndex)
    {

        if (isGrabed)
        {
            _anim.SetIKPosition(AvatarIKGoal.LeftHand,leftHandPos.position);
            _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand,1f);
            
            _anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandPos.rotation);
            _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            
            _anim.SetIKPositionWeight(AvatarIKGoal.RightHand,1f);
            _anim.SetIKPosition(AvatarIKGoal.RightHand,RightHandPos.position);
            
            _anim.SetIKRotation(AvatarIKGoal.RightHand, RightHandPos.rotation);
            _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        }
        
       
    }
    
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(leftHandPos.position, 0.01f);
        Gizmos.DrawSphere(RightHandPos.position, 0.01f);

    }
}
