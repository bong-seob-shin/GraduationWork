using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRun : IState
{
    Player player = Player.Instance;

    public void OperateEnter()
    {
        if (player.w_keyPress)
        {
            if (player.DirZ < 1f)
            {
                player.DirZ += 0.02f;
            }

            player.playerAnim.SetFloat("MoveDirZ" , player.DirZ);
        }
        if (player.s_keyPress)
        {
            if (player.DirZ > -1f)
            {
                player.DirZ -= 0.02f;
            }
            player.playerAnim.SetFloat("MoveDirZ" , player.DirZ);

        }
        if (player.a_keyPress)
        {
            if (player.DirX > -1f)
            {
                player.DirX -= 0.02f;
            }
            player.playerAnim.SetFloat("MoveDirX" , player.DirX);

        }
        if (player.d_keyPress)
        {
            if (player.DirX < 1f)
            {
                player.DirX += 0.02f;
            }
         
            player.playerAnim.SetFloat("MoveDirX" , player.DirX);
        }
        player.applySpeed = player.runSpeed;
        
    }

    public void OperateUpdate()
    {
    }

    public void OperateExit()
    {
        player.applySpeed = player.walkSpeed;

    }
}
