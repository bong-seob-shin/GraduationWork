using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKCCD : MonoBehaviour
{
    
    public Vector3 offset;
    
    public bool isGrabed = false;
    public Transform targetPos;
    
    public Transform endEffector;
    public Transform parentBone;
    private Animator _anim;

    public int iterCount =0;
    
    public List<Transform> _boneList = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        _anim = GetComponent<Animator>();
        endEffector = _anim.GetBoneTransform(HumanBodyBones.LeftLowerArm);

        Transform currentBone = endEffector;

        while (currentBone != parentBone)
        {
            _boneList.Add(currentBone);
            currentBone = currentBone.parent;
        }
    }

    // Update is called once per frame

    private void LateUpdate()
    {

        while (iterCount < 10 && (targetPos.position - endEffector.position).sqrMagnitude > 0.1f)
        {

            
            iterCount++;
        }
        //endEffector.position = endEffector.position+offset;
        //bone받아서 계산하기
    }

   
}
