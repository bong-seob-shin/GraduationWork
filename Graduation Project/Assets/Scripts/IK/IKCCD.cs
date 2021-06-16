﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKCCD : MonoBehaviour
{
    public Transform leftHandPos;
    public Transform RightHandPos;
    public Transform LeftFootPos;
    public Transform RightFootPos;

    public Vector3 offset;
    
    public bool isGrabed = false;

    public Transform lefthandbone;
    
    private Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        lefthandbone = _anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);
    }

    // Update is called once per frame

    private void LateUpdate()
    {       
       

        lefthandbone.position = lefthandbone.position+offset;
        //bone받아서 계산하기
    }

    // private void OnAnimatorIK(int layerIndex)
    // {
    //
    //     if (isGrabed)
    //     {
    //         _anim.SetIKPosition(AvatarIKGoal.LeftHand,leftHandPos.position);
    //         _anim.SetIKPositionWeight(AvatarIKGoal.LeftHand,1f);
    //         
    //         _anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandPos.rotation);
    //         _anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
    //         
    //         _anim.SetIKPosition(AvatarIKGoal.LeftFoot,LeftFootPos.position);
    //         _anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot,1f);
    //         
    //         _anim.SetIKRotation(AvatarIKGoal.LeftFoot, LeftFootPos.rotation);
    //         _anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
    //         
    //         _anim.SetIKPosition(AvatarIKGoal.RightHand,RightHandPos.position);
    //         _anim.SetIKPositionWeight(AvatarIKGoal.RightHand,1f);
    //
    //         _anim.SetIKRotation(AvatarIKGoal.RightHand, RightHandPos.rotation);
    //         _anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
    //         
    //         _anim.SetIKPosition(AvatarIKGoal.RightFoot,RightFootPos.position);
    //         _anim.SetIKPositionWeight(AvatarIKGoal.RightFoot,1f);
    //
    //         _anim.SetIKRotation(AvatarIKGoal.RightFoot, RightFootPos.rotation);
    //         _anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
    //     }
    //     
    //    
    // }
}
