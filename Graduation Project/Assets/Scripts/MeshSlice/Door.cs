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
    
    public Material insideMaterial;
    public float loadTime;
    public int generations;
    /// </summary>
    
    private void Start()
    {
        MaxHP = 150;
        HP = MaxHP;
    }

    private void Update()
    {
        if (this.HP <= 0.0f)
        {
            insideMaterial = this.transform.GetComponent<MeshRenderer>().materials.ElementAt(0);
            StartCoroutine(ShatterObject(this.gameObject, slicePos, generations));
        }
    }

    private IEnumerator ShatterObject(GameObject obj,Vector3 hitpoint,int gen)
    {
        yield return new WaitForSeconds(Random.Range(loadTime, loadTime * 2));
        
        GameObject[] pieces = MeshManipulation.MeshCut.Cut(obj, /*obj.GetComponent<Collider>().bounds.center*/ hitpoint, GetAngle(obj,gen), insideMaterial);

        foreach (GameObject piece in pieces)
        {
            piece.AddComponent<Rigidbody>().ResetCenterOfMass();
            piece.AddComponent<MeshCollider>().convex = true;

            piece.GetComponent<Rigidbody>().AddForce(transform.forward * 2.0f , ForceMode.Impulse);
            piece.transform.tag = "Sliceable";
            if (gen > 0)
                StartCoroutine(ShatterObject(piece, hitpoint, gen - 1));

            piece.AddComponent<MeshDestroy>();
        }
        Destroy(obj);
    }
    private Vector3 GetAngle(GameObject obj , int gen)
    {
        Quaternion q = Quaternion.Euler(Random.Range(-40, 40), Random.Range(-40, 40), Random.Range(-40, 40));
        Vector3[] faces = {obj.transform.forward, obj.transform.right, obj.transform.up};
        return q * faces[gen % 3];
    }
    private void OnDestroy()
    {
        for (int i = 0; i < monsters.Length; i++)
        {
            monsters[i].SetActive(true);
        }
    }
}
