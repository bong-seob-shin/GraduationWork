using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SliceManager : MonoBehaviour
{
    public GameObject SliceObject;
    public Material insideMaterial;
    public Vector3 SlicePos;
    public bool isSlice = false;

    private float loadTime = 0.1f;
    private int generations = 5;

    // Update is called once per frame
    void Update()
    {
        if (isSlice)
        {
            StartCoroutine(ShatterObject(SliceObject, SlicePos, generations));
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

}
