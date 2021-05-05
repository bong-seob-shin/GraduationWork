using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloor : MonoBehaviour
{
    public GameObject player;
    public bool isPlayerOn;
  

    void Start()
    {
        
    }

    // Update is called once per frame
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!isPlayerOn)
            {
               
                player = other.gameObject;
                player.transform.parent = transform;
                isPlayerOn = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (isPlayerOn)
        {
            player.transform.parent = null;
            player = null;
            isPlayerOn = false;
        }
    }
}
