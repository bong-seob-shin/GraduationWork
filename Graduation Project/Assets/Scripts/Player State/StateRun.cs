using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateRun : IState
{
    Player player = Player.Instance;

    public void OperateEnter()
    {
        
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
