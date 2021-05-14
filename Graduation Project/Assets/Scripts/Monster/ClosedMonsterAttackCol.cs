using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosedMonsterAttackCol : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().hit(damage);
        }
    }
}
