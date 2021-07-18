using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Door : ObjManager
{
    public GameObject[] monsters;
    public Vector3 slicePos;

    private void Start()
    {
        MaxHP = 150;
        HP = MaxHP;
    }

    private void Update()
    {
        if (this.HP <= 0.0f)
        {
            SliceManager sliceManager = FindObjectOfType<SliceManager>();
            sliceManager.SliceObject = this.transform.gameObject;
            sliceManager.insideMaterial = GetComponent<MeshRenderer>().materials.ElementAt(0);
            sliceManager.SlicePos = slicePos;
            sliceManager.isSlice = true;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].SetActive(true);
        }
    }
}
