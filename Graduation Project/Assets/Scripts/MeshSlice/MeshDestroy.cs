using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDestroy : MonoBehaviour
{
    private float destroyTime = 20.0f;

    // Update is called once per frame
    void Update()
    {
        destroyTime -= Time.deltaTime;
        if (destroyTime <= 0.0f)
        {
            Destroy(this.gameObject);
        }
    }
}
