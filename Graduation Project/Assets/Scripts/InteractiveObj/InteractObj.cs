﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObj : MonoBehaviour
{
    
    public bool isOn =false;
    [SerializeField] protected bool isSwitch = false;
    public Animation[] interactiveObjAnims;

    public Transform ikPosition;

    
    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    public virtual void InteractObjs()
    {
        
    }
    
   
}
