using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : ObjManager
{
    private Transform _parent;
    // Start is called before the first frame update
    void Start()
    {
        _parent = transform.parent;
        transform.parent = null;
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        transform.parent = _parent.parent;
        MaxHP = 80;
        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0)
        {
            Destroy(_parent.gameObject);
            Destroy(this.gameObject);
        }
    }
}
