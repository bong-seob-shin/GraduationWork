using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2OperationField : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FindObjectOfType<BossMonster2>().isOperate = true;
        }
    }
}
