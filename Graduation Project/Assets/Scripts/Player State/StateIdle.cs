using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateIdle : IState
{
    Player player = Player.Instance;
    UIManager uiManager = UIManager.instance;
    public void OperateEnter()
    {
        
        player.anim.SetFloat("MoveDirX", 0.0f);
        player.anim.SetFloat("MoveDirZ", 0.0f);
        
    }

    public void OperateUpdate()
    {
        if (uiManager.crossHairSize > 80f)
        {
            uiManager.crossHairSize -= 60.0f*Time.deltaTime;
        }
        else if(uiManager.crossHairSize <80f)
        {
            uiManager.crossHairSize =80f;
        }
    }

    public void OperateExit()
    {
    }
}
