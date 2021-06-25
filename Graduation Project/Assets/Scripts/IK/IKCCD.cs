using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKCCD : MonoBehaviour
{
    

    
    public Transform targetPos;
    
    public Transform endEffector;
    public Transform parentBone;


    public int maxIterCount = 10;

    [Range(0,1)]
    public float weight = 0;
    
    public List<Transform> _boneList = new List<Transform>();
    public List<RotationLimit> _boneRotationLimit = new List<RotationLimit>();
    


    
    private Transform standardPos;
    public float armDistance;

    // Start is called before the first frame update
    void Start()
    {
    
     
       
        Transform currentBone = endEffector;

        // while (currentBone != parentBone)
        // {
        //     _boneList.Add(currentBone);
        //     currentBone = currentBone.parent;
        // }
        // _boneList.Add(currentBone);

        standardPos = _boneList[_boneList.Count - 1];
        armDistance = (standardPos.position - endEffector.position).sqrMagnitude;

        for (int i = 0; i < _boneList.Count; i++)
        {
            _boneRotationLimit.Add(_boneList[i].GetComponent<RotationLimit>());
        }
     
    }

    // Update is called once per frame

    private void LateUpdate()
    {
                
        
        if (targetPos !=null)//null연산에 사용되는데 비용이 적게듬
        {
            float dist = (standardPos.position - targetPos.position).sqrMagnitude;

           
            if (dist < armDistance)
            {


                IKCCDSolution();
            }
        }


        //bone받아서 계산하기
    }

    void IKCCDSolution()
    {
        Vector3 effectorPos = _boneList[0].position;
        
        Vector3 target = Vector3.Lerp(effectorPos, targetPos.position, weight);

        
        float sqrDistance;


        int iterCount =0;
        do
        {
            for (int i = 0; i < _boneList.Count - 2; i++)
            {
                for (int j = 1; j < i + 3 && j < _boneList.Count; j++)
                {
                    RotateBone(_boneList[0], _boneList[j],target, _boneRotationLimit[j]._boneRotationLimitMax,_boneRotationLimit[j]._boneRotationLimitMin);

                    sqrDistance = (_boneList[0].position - target).sqrMagnitude;

                    if (sqrDistance <= 0.01f)
                    {
                        return;
                    }
                }
            }

            sqrDistance = (_boneList[0].position - target).sqrMagnitude;
            iterCount++;
        } while (iterCount <= maxIterCount && sqrDistance > 0.01f);
    }
    
    void RotateBone(Transform effector, Transform bone, Vector3 targetPos, Vector3 boneRotLimitMax, Vector3 boneRotLimitMin)
    {
        Vector3 endEffectorPos = effector.position;
        Vector3 boneToTarget = targetPos -  bone.position;
        Vector3 boneToEnd = endEffectorPos - bone.position;
        Quaternion boneRotation = bone.rotation;

        
        
        
        Quaternion fromToRotation = Quaternion.FromToRotation(boneToEnd, boneToTarget);

        // Vector3 eulerFTR = fromToRotation.eulerAngles;
        //
        // eulerFTR.x = (eulerFTR.x > 180) ? eulerFTR.x - 360 : eulerFTR.x;
        // eulerFTR.y = (eulerFTR.y > 180) ? eulerFTR.y - 360 : eulerFTR.y;
        // eulerFTR.z = (eulerFTR.z > 180) ? eulerFTR.z - 360 : eulerFTR.z;
        //
        // eulerFTR.x = Mathf.Clamp(eulerFTR.x, boneRotLimitMin.x, boneRotLimitMax.x);
        // eulerFTR.y = Mathf.Clamp(eulerFTR.y, boneRotLimitMin.y, boneRotLimitMax.y);
        // eulerFTR.z = Mathf.Clamp(eulerFTR.z, boneRotLimitMin.z, boneRotLimitMax.z);

        Quaternion newRot = fromToRotation * boneRotation;
        //Quaternion newRot = Quaternion.Euler(eulerFTR) * boneRotation;

        
        bone.rotation = newRot;
        
        
        Vector3 eulerFTR =  bone.localRotation.eulerAngles;

        eulerFTR.x = (eulerFTR.x > 180) ? -eulerFTR.x : eulerFTR.x;
        eulerFTR.y = (eulerFTR.y > 180) ? eulerFTR.y - 360 : eulerFTR.y;
        eulerFTR.z = (eulerFTR.z > 180) ? eulerFTR.z - 360 : eulerFTR.z;
        
        eulerFTR.x = Mathf.Clamp(eulerFTR.x, boneRotLimitMin.x, boneRotLimitMax.x);
        eulerFTR.y = Mathf.Clamp(eulerFTR.y, boneRotLimitMin.y, boneRotLimitMax.y);
        eulerFTR.z = Mathf.Clamp(eulerFTR.z, boneRotLimitMin.z, boneRotLimitMax.z);

        
        bone.localRotation = Quaternion.Euler(eulerFTR);
    }
}
