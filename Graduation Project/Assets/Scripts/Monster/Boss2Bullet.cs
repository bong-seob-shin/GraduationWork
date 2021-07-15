using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Bullet : MonoBehaviour
{
    public Vector3 target;

    public GameObject floorAttack;
    // Update is called once per frame
    void Update()
    {
        this.transform.localScale = new Vector3(transform.localScale.x + Time.deltaTime,
            transform.localScale.y + Time.deltaTime, transform.localScale.z + Time.deltaTime);

        this.transform.position = Vector3.MoveTowards(transform.position, target, 5.0f * Time.deltaTime);

        if (transform.position == target)
        {
            Instantiate(floorAttack, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
    
}
