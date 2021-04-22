using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;

    private Animation _gunAnim;
    // Start is called before the first frame update
    void Start()
    {
        _gunAnim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }

        if (Input.GetButtonUp("Fire1")) 
        {
            _gunAnim.Stop();
        }
    }

    private void Shoot()
    {
        _gunAnim.Play();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);
        }
        
        
    }
    
    private void OnDrawGizmos()
    {
        Debug.DrawLine(fpsCam.transform.position, fpsCam.transform.position+fpsCam.transform.forward*range);
    }
}
