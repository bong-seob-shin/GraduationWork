using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractiveNonAnimButton : InteractObj
{
   
    public enum NonObjType
    {
        Lamp,None
    }
    public NonObjType btnType;
    public GameObject interactGObj;
    protected override void Start()
    {
        buttonList.Add(this);
        
    }
    

    public override void InteractObjs()
    {
        if (!isOn)
        {
            
            switch (btnType)
            {
                case NonObjType.Lamp:
                    interactGObj.GetComponent<OnOffLamp>().isOnOff = true;
                    break;
                default:
                    break;
            }
            isOn = !isOn;
            isSwitch = true;

            return;
        }

        if (isOn)
        {
           
            switch (btnType)
            {
                case NonObjType.Lamp:
                    interactGObj.GetComponent<OnOffLamp>().isOnOff = false;
                    break;
                default:
                    break;
            }

            isOn = !isOn;
            isSwitch = true;

        }
        
    }
}
