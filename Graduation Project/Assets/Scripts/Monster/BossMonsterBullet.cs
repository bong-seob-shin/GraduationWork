using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterBullet : ObjManager
{
    public int damage = 30;
     

    [SerializeField] private Vector3 target;
    private Vector3 dir;

    private void Start()
    {
        speed = 10.0f;
    }

    private void Update()
    {
        transform.position += dir * Time.deltaTime * speed;
        
    }

    public void SetTarget(Vector3 t)
    {
        target = t;
        dir = (target - transform.position).normalized;
        dir += new Vector3(0.0f, 0.02f, 0.0f);
    }

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

        if (other.CompareTag("Cave"))
        {
            Destroy(this.gameObject);
        }
    }
}
