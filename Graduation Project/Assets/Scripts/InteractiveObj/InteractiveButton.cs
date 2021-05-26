using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class InteractiveButton : InteractObj
{
    
    public bool isAnimsPlay = false;

   
    protected override void Update()
    {
        if (isSwitch)
        {
            for (int i = 0; i < interactiveObjAnims.Length; i++)
            {
                interactiveObjAnims[i].Play();
            }

           

            isSwitch = false;
        }
        
        for (int i = 0; i < interactiveObjAnims.Length; i++)
        {
            if (!interactiveObjAnims[i].isPlaying)
            {
                isAnimsPlay = false;
                break;
            }
        }

        if (!isAnimsPlay)
        {
        
            if (_ikLeftHandGrab != null)
            {
                _ikLeftHandGrab.isGrabed = false;
                _ikLeftHandGrab = null;
            }
        }


    }

    public override void InteractObjs()
    {
        isAnimsPlay = true;
        if (!isOn)
        {
            
            for (int i = 0; i < interactiveObjAnims.Length; i++)
            {

                string animName = interactiveObjAnims[i].name;
                Debug.Log("온" + animName);
                interactiveObjAnims[i][animName].normalizedTime= 0f;
                interactiveObjAnims[i][animName].speed = 1f;
            }

          

            isOn = !isOn;
            isSwitch = true;

            return;
        }

        if (isOn)
        {
            for (int i = 0; i < interactiveObjAnims.Length; i++)
            {

                string animName = interactiveObjAnims[i].name;
                Debug.Log("오프" + animName);
                interactiveObjAnims[i][animName].normalizedTime= 1f;
                interactiveObjAnims[i][animName].speed = -1f;
            }

         

            isOn = !isOn;
            isSwitch = true;

        }
        
    }
}
