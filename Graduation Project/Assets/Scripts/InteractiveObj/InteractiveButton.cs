using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class InteractiveButton : MonoBehaviour
{

    public Animation[] interactiveObjAnims;
    
    [SerializeField]private bool isOn =false;
    [SerializeField] private bool isSwitch = false;
    private void Update()
    {
        if (isSwitch)
        {
            for (int i = 0; i < interactiveObjAnims.Length; i++)
            {
                interactiveObjAnims[i].Play();
            }

            isSwitch = false;
        }
    }

    public void InteractObj()
    {
        if (!isOn)
        {
            for (int i = 0; i < interactiveObjAnims.Length; i++)
            {
               // interactiveObjAnims[i].Stop();

                string animName = interactiveObjAnims[i].name;
                Debug.Log("온" + animName);
                interactiveObjAnims[i][animName].normalizedTime= 0f;
                interactiveObjAnims[i][animName].speed = 1f;
                //interactiveObjAnims[i].Play();
            }

            isOn = !isOn;
            isSwitch = true;
            return;
        }

        if (isOn)
        {
            for (int i = 0; i < interactiveObjAnims.Length; i++)
            {

                //interactiveObjAnims[i].Stop();
                string animName = interactiveObjAnims[i].name;
                Debug.Log("오프" + animName);
                interactiveObjAnims[i][animName].normalizedTime= 1f;
                interactiveObjAnims[i][animName].speed = -1f;
                //interactiveObjAnims[i].Play();
            }

            isOn = !isOn;
            isSwitch = true;

        }
        
    }
}
