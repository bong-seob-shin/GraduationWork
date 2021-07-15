using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2FloorAttack : MonoBehaviour
{
    public float existTime;
    // Update is called once per frame
    void Update()
    {
        existTime += Time.deltaTime;
        if (existTime >= 5.0f)
        {
            //GameObject.Find("_Dungeon2").transform.Find("ElectroFloor").gameObject.SetActive(false);

            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!other.GetComponent<Player>().boss2Safety)
            {
                other.gameObject.GetComponent<Player>().hit(5);
            }
        }
    }
}
