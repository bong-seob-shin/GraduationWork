using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKGunGrab : MonoBehaviour
{
    public Transform leftHandTestPos;
    public Transform RightHandTestPos;
    
    
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
            _anim.SetIKPosition(AvatarIKGoal.LeftHand,leftHandTestPos.position);
            _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand,1f);
            
            _anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandTestPos.rotation);
            _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
            
            _anim.SetIKPositionWeight(AvatarIKGoal.RightHand,1f);
            _anim.SetIKPosition(AvatarIKGoal.RightHand,RightHandTestPos.position);
            
            _anim.SetIKRotation(AvatarIKGoal.RightHand, RightHandTestPos.rotation);
            _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        }
        
       
    }
}
