using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKLeftHandGrab : MonoBehaviour
{
    public Transform leftHandPos;
  
    
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
            
           
        }
        
       
    }
}
