using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMissileShockWave : MonoBehaviour
{
    public float radius;

    public Collider[] colls;

    private bool isBomb;
    public float persistTime;

    private void Start()
    {
        isBomb = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isBomb)
        {
            persistTime -= Time.deltaTime;
            
            colls = Physics.OverlapSphere(transform.position, radius);

            for (int i = 0; i < colls.Length; i++)
            {
                if (colls[i].tag == "Player")
                {
                    colls[i].gameObject.GetComponent<Player>().hit(30);
                }
            }

            if (persistTime <= 0.0f)
            {
                isBomb = false;
            }
        }

        if (!isBomb)
        {
            Destroy(this.gameObject);
        }
        
        
        

    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
