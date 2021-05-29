﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateJump : IState
{
    Player player = Player.Instance;
    UIManager uiManager = UIManager.instance;
    public void OperateEnter()
    {

        if (player.isGround&&player.isJump)
        {
            Debug.Log("점프");
            player.anim.SetTrigger("Jump");
            //player.playerRb.velocity = player.transform.up * player.jumpForce;
            player.playerRb.AddForce(Vector3.up*player.jumpForce);
            player.isJump = false;
            uiManager.crossHairSize = 320.0f;
        }
      

    }

    public void OperateUpdate()
    {
    }

    public void OperateExit()
    {
       

    }
}
