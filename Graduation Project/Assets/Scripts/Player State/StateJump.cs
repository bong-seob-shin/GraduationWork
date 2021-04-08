using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump : IState
{
    Player player = Player.Instance;

    public void OperateEnter()
    {
        if (player.isGround)
        {

            player.playerAnim.SetTrigger("Jump");
            player.playerRb.velocity = player.transform.up * player.jumpForce;
        }
        
    }

    public void OperateUpdate()
    {
    }

    public void OperateExit()
    {
    }
}
