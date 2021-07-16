using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Bullet : MonoBehaviour
{
    public Vector3 target;

    public float existTime;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = Vector3.MoveTowards(transform.position, target, 10.0f * Time.deltaTime);

        if (transform.position == target)
        {
            existTime -= Time.deltaTime;
            if (existTime <= 0.0f)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().hit(30);
            Destroy(this.gameObject);
        }
    }
}
