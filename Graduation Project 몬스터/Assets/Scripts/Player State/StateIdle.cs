using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : IState
{
    Player player = Player.Instance;
    public void OperateEnter()
    {
        
        player.anim.SetFloat("MoveDirX", 0.0f);
        player.anim.SetFloat("MoveDirZ", 0.0f);

    }

    public void OperateUpdate()
    {
    }

    public void OperateExit()
    {
    }
}
