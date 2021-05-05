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
      if (other.CompareTag("floor") || other.CompareTag("ceiling"))
      {
         if (isAcivate)
         {
            for (int i = 0; i < interactiveObjAnims.Length; i++)
            {
               interactiveObjAnims[i].Play();
            }

            anotherLift.isAcivate = true;
            isAcivate = false;
         }
      }
     
   }
}
