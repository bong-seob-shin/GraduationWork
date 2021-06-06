using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject[] monsters;
    private void OnDestroy()
    {
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].SetActive(true);
        }
    }
}
