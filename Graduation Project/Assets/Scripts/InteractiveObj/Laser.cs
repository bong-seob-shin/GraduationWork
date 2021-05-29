using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : ObjManager
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

        transform.position = Vector3.MoveTowards(transform.position, target, 5.0f * Time.deltaTime);
        
        if (Physics.Raycast(transform.position, transform.right, out rightHitInfo, 50.0f))
        {
            if (rightHitInfo.transform.CompareTag("Cave"))
            {
                rightDist =  (rightHitInfo.point-transform.position).magnitude;
                _lineRenderer.SetPosition(0, new Vector3( rightDist, 0 , 0));
                rightX = rightHitInfo.point.x;
            }
        }
        
        
        if (Physics.Raycast(transform.position, -transform.right, out leftHitInfo, 50.0f))
        {
            if (leftHitInfo.transform.CompareTag("Cave"))
            {
                leftDist = (leftHitInfo.point -transform.position).magnitude ;
                _lineRenderer.SetPosition(1, new Vector3(-leftDist,0,0) );
                LeftX = leftHitInfo.point.x;
            }
        }
        
        _boxCollider.size = new Vector3(leftDist+rightDist,1,1);

        if (transform.position == target)
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
