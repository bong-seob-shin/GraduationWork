using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveDoubleButton : InteractObj
{

    public InteractiveDoubleButton pairButton;

    public bool isFirstButton = false;
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
    }

    public override void InteractObjs()
    {
        if (isOn)
        {
            if (isFirstButton)
            {
                for (int i = 0; i < interactiveObjAnims.Length; i++)
                {

                    string animName = interactiveObjAnims[i].name;
                    Debug.Log("온" + animName);
                    interactiveObjAnims[i][animName].normalizedTime = 0f;
                    interactiveObjAnims[i][animName].speed = 1f;
                }
            }

            if (!isFirstButton)
            {
                for (int i = 0; i < interactiveObjAnims.Length; i++)
                {

                    string animName = interactiveObjAnims[i].name;
                    Debug.Log("오프" + animName);
                    interactiveObjAnims[i][animName].normalizedTime = 1f;
                    interactiveObjAnims[i][animName].speed = -1f;
                }
            }

            pairButton.isOn = true;
            isOn = false;
            isSwitch = true;

        }
        
    }
}
