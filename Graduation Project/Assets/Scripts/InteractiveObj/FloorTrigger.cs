using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class FloorTrigger : InteractObj
{
    private float rewindAnimTimer = 0.0f;
    public float rewindTime;
    private bool isTimerOn = false;
    private bool isOpen = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           InteractObjs();
        }
    }

    protected override void Update()
    {
        if (rewindAnimTimer < rewindTime )
        {
            if(isTimerOn) rewindAnimTimer += Time.deltaTime;
        }
        else
        {

            if (isTimerOn)
            {
                for (int i = 0; i < interactiveObjAnims.Length; i++)
                {
                    string animName = interactiveObjAnims[i].name;
                    Debug.Log("오프" + animName);
                    interactiveObjAnims[i][animName].normalizedTime = 1f;
                    interactiveObjAnims[i][animName].speed = -1f;
                    interactiveObjAnims[i].Play();
                }

                isOpen = false;
            }

            isTimerOn = false;
            rewindAnimTimer = 0;
            
        }
    }

    public override void InteractObjs()
    {

        if (!isOpen)
        {
            for (int i = 0; i < interactiveObjAnims.Length; i++)
            {
                string animName = interactiveObjAnims[i].name;
                Debug.Log("온" + animName);
                interactiveObjAnims[i][animName].normalizedTime = 0f;
                interactiveObjAnims[i][animName].speed = 1f;
                interactiveObjAnims[i].Play();
            }
            isTimerOn = true;
            isOpen = true;
        }

    }
}
