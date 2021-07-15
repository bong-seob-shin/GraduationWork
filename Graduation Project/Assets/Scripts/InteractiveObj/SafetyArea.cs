using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().boss2Safety = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player>().boss2Safety = false;
        }
    }
}
