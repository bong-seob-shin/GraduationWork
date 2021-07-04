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
        if (other.CompareTag("Player"))
        {
            Instantiate(shockWave, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    
        if (other.CompareTag("Terrain"))
        {
            Instantiate(shockWave, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
