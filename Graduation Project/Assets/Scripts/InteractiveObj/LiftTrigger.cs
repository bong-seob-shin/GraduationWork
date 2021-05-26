using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftTrigger : MonoBehaviour
{

   public InteractiveLiftButton upButton;
   public InteractiveLiftButton downButton;

   public Animation[] interactiveObjAnims;

   public LiftTrigger anotherLift;

   public bool isActivate = false;
   private void Update()
   {
      
   }

   private void OnTriggerEnter(Collider other)
   {
      
      if (other.CompareTag("floor"))
      {
         if (isActivate)
         {
            for (int i = 0; i < interactiveObjAnims.Length; i++)
            {
               string animName = interactiveObjAnims[i].name;
               Debug.Log("온" + animName);
               interactiveObjAnims[i][animName].normalizedTime= 1f;
               interactiveObjAnims[i][animName].speed = -1f;
               interactiveObjAnims[i].Play();
            }

            upButton.isOn = true;
            anotherLift.isActivate = true;
            isActivate = false;
         }
      }

      if (other.CompareTag("ceiling"))
      {
         if (isActivate)
         {
            for (int i = 0; i < interactiveObjAnims.Length; i++)
            {
               string animName = interactiveObjAnims[i].name;
               Debug.Log("온" + animName);
               interactiveObjAnims[i][animName].normalizedTime = 0f;
               interactiveObjAnims[i][animName].speed = 1f;
               interactiveObjAnims[i].Play();
            }
            
            downButton.isOn = false;
            anotherLift.isActivate = true;
            isActivate = false;
         }
      }

   }

  
}
