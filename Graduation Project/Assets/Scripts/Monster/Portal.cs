using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public float portalTime;
    private float currentTime;
    
    private void Awake()
    {
        currentTime = portalTime;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0.0f)
        {
            Destroy(this.gameObject);
            
            currentTime = portalTime;
        }
    }
    
    
}
