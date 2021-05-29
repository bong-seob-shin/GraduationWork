using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : ObjManager
{
    // Start is called before the first frame update
    void Start()
    {
        MaxHP = 80;
        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Destroy(this.transform.parent.gameObject);
            Destroy(this.gameObject);
        }
    }
}
