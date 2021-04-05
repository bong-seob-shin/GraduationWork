using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRun : IState
{
    Player player = Player.Instance;

    public void OperateEnter()
    {
        player.playerAnim.SetBool("Run" , true);
        player.applySpeed = player.runSpeed;
        
    }

    public void OperateUpdate()
    {
    }

    public void OperateExit()
    {
        player.applySpeed = player.walkSpeed;
        player.playerAnim.SetBool("Run" , false);

    }
}
