using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveLiftButton : InteractObj
{
    public LiftTrigger liftTrigger =null;
    public Animation[] liftAnims;
    public InteractiveDoubleButton downButton;
    public InteractiveDoubleButton upButton;
    public bool isUpbutton = false;
    
 
    protected override void Start()
    {
        buttonList.Add(this);
    }
   
    protected override void Update()
    {
        if (isSwitch)
        {
            if (liftTrigger.isActivate)
            {
                for (int i = 0; i < liftAnims.Length; i++)
                {
                    liftAnims[i].Play();

                }
            }
            else
            {
                for (int i = 0; i < interactiveObjAnims.Length; i++)
                {
                    interactiveObjAnims[i].Play();
                }
            }





            isSwitch = false;
        }
        
  
    }

    public override void InteractObjs()
    {
        if (isUpbutton)
        {
            upButton.isOn = false;
            downButton.isOn = true;
        }
        else
        {
            upButton.isOn = true;
            downButton.isOn = false;
        }
        if (!isOn)
        {
            
            for (int i = 0; i < interactiveObjAnims.Length; i++)
            {

                string animName = interactiveObjAnims[i].name;
                Debug.Log("온" + animName);
                interactiveObjAnims[i][animName].normalizedTime= 0f;
                interactiveObjAnims[i][animName].speed = 1f;
            }

            
                for (int i = 0; i < liftAnims.Length; i++)
                {
                    string liftAnimName = liftAnims[i].name;
                    liftAnims[i][liftAnimName].normalizedTime = 1f;
                    liftAnims[i][liftAnimName].speed = -1f;
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

           
                for (int i = 0; i < liftAnims.Length; i++)
                {
                    string liftAnimName = liftAnims[i].name;
                    liftAnims[i][liftAnimName].normalizedTime = 0f;
                    liftAnims[i][liftAnimName].speed = 1f;
                  
                }
            

            isOn = !isOn;
            isSwitch = true;

        }
        
    }
}
