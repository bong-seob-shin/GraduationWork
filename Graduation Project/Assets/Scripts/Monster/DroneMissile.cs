using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMissile : MonoBehaviour
{
    public GameObject shockWave;
    public int damage;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("DroneMonster"))
        {
            Instantiate(shockWave, transform.position, Quaternion.identity);

            Destroy(this.gameObject);
        }
    }
}
