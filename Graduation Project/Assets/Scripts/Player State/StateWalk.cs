using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateWalk : IState
{
   Player player = Player.Instance;

   public void OperateEnter()
   {
      if (player.w_keyPress)
      {
         player.playerAnim.SetBool("Walk", true);
      }
   }

   public void OperateUpdate()
   {
      
   }

   public void OperateExit()
   {
      player.playerAnim.SetBool("Walk" , false);

   }
}
