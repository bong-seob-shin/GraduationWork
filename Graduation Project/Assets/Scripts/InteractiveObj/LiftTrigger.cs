using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftTrigger : MonoBehaviour
{
   public Animation[] interactiveObjAnims;

   public LiftTrigger anotherLift;

   public bool isAcivate = false;
   private void Update()
   {
      
   }

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("floor"))
      {
         if (isAcivate)
         {
            for (int i = 0; i < interactiveObjAnims.Length; i++)
            {
               string animName = interactiveObjAnims[i].name;
               Debug.Log("온" + animName);
               interactiveObjAnims[i][animName].normalizedTime= 1f;
               interactiveObjAnims[i][animName].speed = -1f;
               interactiveObjAnims[i].Play();
            }

            anotherLift.isAcivate = true;
            isAcivate = false;
         }
      }

      if (other.CompareTag("ceiling"))
      {
         if (isAcivate)
         {
            for (int i = 0; i < interactiveObjAnims.Length; i++)
            {
               string animName = interactiveObjAnims[i].name;
               Debug.Log("온" + animName);
               interactiveObjAnims[i][animName].normalizedTime = 0f;
               interactiveObjAnims[i][animName].speed = 1f;
               interactiveObjAnims[i].Play();
            }

            anotherLift.isAcivate = true;
            isAcivate = false;
         }
      }

   }
}
