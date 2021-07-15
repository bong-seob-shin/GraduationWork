using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Door : ObjManager
{
    public GameObject[] monsters;

    private void Start()
    {
        MaxHP = 150;
        HP = MaxHP;
    }

    private void OnDestroy()
    {
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].SetActive(true);
        }
    }
}
