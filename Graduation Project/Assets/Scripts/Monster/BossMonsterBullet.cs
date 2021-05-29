using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterBullet : MonoBehaviour
{
    public int damage = 30;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().hit(damage);
            Destroy(this.gameObject);
        }

        if (other.CompareTag("Terrain"))
        {
            Destroy(this.gameObject);
        }
    }
}
