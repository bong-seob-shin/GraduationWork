using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Laser : ObjManager
{
    public int damage = 30;
    
    private LineRenderer _lineRenderer;

    private BoxCollider _boxCollider;

    public Vector3 target;
    
    private float rightDist = 0;
    private float leftDist = 0;

    // Start is called before the first frame update
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit rightHitInfo;
        RaycastHit leftHitInfo;

        float rightX = 0;
        float LeftX = 0;

        transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.x,transform.position.y,target.z), 5.0f * Time.deltaTime);
        
        if (Physics.Raycast(transform.position, transform.right, out rightHitInfo, 100.0f))
        {
            if (rightHitInfo.transform != null)
            {
                rightDist =  (rightHitInfo.point-transform.position).magnitude;
                _lineRenderer.SetPosition(0, new Vector3( rightDist, 0 , 0));
            }
        }


        if (Physics.Raycast(transform.position, -transform.right, out leftHitInfo, 100.0f))
        {
            if (leftHitInfo.transform != null)
            {
                leftDist = (leftHitInfo.point - transform.position).magnitude;
                _lineRenderer.SetPosition(1, new Vector3(-leftDist, 0, 0));
            }
        }

        _boxCollider.size = new Vector3(leftDist+rightDist,1,1);

        if (transform.position == new Vector3(target.x,transform.position.y,target.z)) 
        {
            Destroy(this.gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().hit(damage);
        }
    }
}
