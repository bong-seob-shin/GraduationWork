using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanePattern : ObjManager
{
    public int damage = 30;
    public GameObject[] planes;
    public GameObject[] corePoints;
    public GameObject core;

    public Vector3 corePosition;
    public int coreRandPos;
    
    public Vector3 target;

    // Update is called once per frame
    void Start()
    {
        coreRandPos = Random.Range(0, corePoints.Length);
        Debug.Log(coreRandPos);
        corePosition = corePoints[coreRandPos].transform.position;
        Instantiate(core, corePosition, Quaternion.identity).transform.parent = corePoints[coreRandPos].transform.parent.transform;
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, 5.0f * Time.deltaTime);
        
        if (transform.position == target)
        {
            Destroy(this.gameObject);
        }
        
    }
}
