using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    private LineRenderer _lineRenderer;

    private BoxCollider _boxCollider;
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

        float rightDist =0;
        float leftDist =0;

        if (Physics.Raycast(transform.position, transform.right, out rightHitInfo, 50.0f))
        {
            if (rightHitInfo.transform.CompareTag("Cave"))
            {
                rightDist =  (rightHitInfo.point-transform.position).magnitude;
                _lineRenderer.SetPosition(0, new Vector3( rightDist, 0 , 0));
            }
        }
        
        if (Physics.Raycast(transform.position, -transform.right, out leftHitInfo, 50.0f))
        {
            if (leftHitInfo.transform.CompareTag("Cave"))
            {
                leftDist = (leftHitInfo.point -transform.position).magnitude ;
                _lineRenderer.SetPosition(1, new Vector3(-leftDist,0,0) );
            }
        }
        
        _boxCollider.size = new Vector3(leftDist+rightDist,1,1);
    }
}
