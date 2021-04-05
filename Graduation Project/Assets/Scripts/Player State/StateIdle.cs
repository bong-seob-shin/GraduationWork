using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : IState
{
    Player player = Player.Instance;
    public void OperateEnter()
    {
        player.playerAnim.SetBool("Idle" ,true);
    }

    public void OperateUpdate()
    {
    }

    public void OperateExit()
    {
        player.playerAnim.SetBool("Idle" ,false);
    }
}
