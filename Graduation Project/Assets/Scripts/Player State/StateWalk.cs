﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWalk : IState
{
   Player player = Player.Instance;
   UIManager uiManager = UIManager.instance;
   public void OperateEnter()
   {
     // if (player.isGround)
      {
         if (player.w_keyPress)
         {
            if (player.dirZ < 0.5f)
            {
               player.dirZ += 0.02f;
            }

            if (player.dirZ > 0.5f)
            {
               player.dirZ -= 0.02f;
            }

         }

         if (player.s_keyPress)
         {
            if (player.dirZ > -0.5f)
            {
               player.dirZ -= 0.02f;
            }

            if (player.dirZ < -0.5f)
            {
               player.dirZ += 0.02f;
            }

      
         }

         if (player.a_keyPress)
         {
            if (player.dirX > -0.5f)
            {
               player.dirX -= 0.02f;
            }

            if (player.dirX < -0.5f)
            {
               player.dirX += 0.02f;
            }

     

         }

         if (player.d_keyPress)
         {
            if (player.dirX < 0.5f)
            {
               player.dirX += 0.02f;
            }

            if (player.dirX > 0.5f)
            {
               player.dirX -= 0.02f;
            }

         
         }

         
         
      }
      player.anim.SetFloat("MoveDirZ", player.dirZ);
      player.anim.SetFloat("MoveDirX", player.dirX);
   }

   public void OperateUpdate()
   {
      if (uiManager.crossHairSize < 160.0f)
      {
         uiManager.crossHairSize += 30.0f * Time.deltaTime;
      }
      if(uiManager.crossHairSize > 160.0f)
      {
         uiManager.crossHairSize -= 150.0f * Time.deltaTime;
      }
      if (!player.isGround)
      {
         player.applySpeed = player.speed/3.0f;

      }
      else
      {
         player.applySpeed = player.speed;

      }
   }

   public void OperateExit()
   {
      
      

   }
}
