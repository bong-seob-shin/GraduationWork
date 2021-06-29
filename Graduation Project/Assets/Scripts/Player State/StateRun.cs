using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StateRun : IState
{
    Player player = Player.Instance;
    UIManager uiManager = UIManager.instance;
    public void OperateEnter()
    {
        if (player.isGround)
        {
            if (player.w_keyPress)
            {
                if (player.dirZ < 1f)
                {
                    player.dirZ += 0.04f;
                }
                else
                {
                    player.dirZ = 1f;
                }

              

            }

            if (player.s_keyPress)
            {
                if (player.dirZ > -1f)
                {
                    player.dirZ -= 0.04f;
                }
                else
                {
                    player.dirZ = -1f;
                }


            }

            if (player.a_keyPress)
            {
                if (player.dirX > -1f)
                {
                    player.dirX -= 0.04f;
                }
                else
                {
                    player.dirX = -1f;
                }

               

            }

            if (player.d_keyPress)
            {
                if (player.dirX < 1f)
                {
                    player.dirX += 0.04f;
                }
                else
                {
                    player.dirX = 1f;
                }
                
            }
       
            player.applySpeed = player.runSpeed;
        }
        player.anim.SetFloat("MoveDirZ", player.dirZ);
        player.anim.SetFloat("MoveDirX", player.dirX);
    }

    public void OperateUpdate()
    {
        if (uiManager.crossHairSize < 220.0f)
        {
            uiManager.crossHairSize += 60.0f*Time.deltaTime;
        }
        if(uiManager.crossHairSize > 220.0f)
        {
            uiManager.crossHairSize -= 150.0f*Time.deltaTime;
        }
    }

    public void OperateExit()
    {
        player.applySpeed = player.speed;
  
    }
}
