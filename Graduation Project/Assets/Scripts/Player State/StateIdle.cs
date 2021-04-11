using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : IState
{
    Player player = Player.Instance;
    public void OperateEnter()
    {
        player.playerAnim.SetFloat("MoveDirX", 0.0f);
        player.playerAnim.SetFloat("MoveDirZ", 0.0f);

    }

    public void OperateUpdate()
    {
    }

    public void OperateExit()
    {
    }
}
