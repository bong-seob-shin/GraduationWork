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
    
    [HideInInspector]public enum ButtonTypes
    {
        Single,
        DoubleA,
        DoubleB
    }

    public ButtonTypes currentButtonTypes;
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
        if (!isOn&&(currentButtonTypes == ButtonTypes.Single || currentButtonTypes == ButtonTypes.DoubleA))
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

        if (isOn&&(currentButtonTypes == ButtonTypes.Single || currentButtonTypes == ButtonTypes.DoubleB))
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
