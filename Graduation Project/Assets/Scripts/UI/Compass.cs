using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{

    public Transform playerTransform;
    public Transform targetDir;


    private void Start()
    {
        targetDir = GameObject.FindWithTag("deongunTransform").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = (targetDir.position - playerTransform.position).normalized;
        Vector3 dirXY = new Vector3(dir.x, 0, dir.z);

       
        float rotAngle = Vector3.SignedAngle(dirXY, playerTransform.forward, Vector3.up);
       
       
        transform.eulerAngles = new Vector3(0, 0, rotAngle);
    }
}
