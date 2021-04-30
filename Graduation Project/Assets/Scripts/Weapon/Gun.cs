using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

public class Gun : MonoBehaviour
{

    private const float FireRate = 0.2f;
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;

    public Animation gunAnim;

    public VisualEffect muzzleFlash;

    private float _currentFireRate =FireRate;

    private int _bulletCount = 20;
    // Start is called before the first frame update
    void Start()
    {
        gunAnim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();

        if (Input.GetKey(KeyCode.Mouse0)) 
        {
            if (_currentFireRate <= 0 )
            {
                Shoot();
            }
        }

        
            
        
    }

    private void GunFireRateCalc()
    {
        

        if (_currentFireRate > 0)
        {
            _currentFireRate -= Time.deltaTime;
        }
    }

    private void Shoot()
    {
        if (!gunAnim.isPlaying)
        {
            gunAnim.Play("GunShot");
        }

        muzzleFlash.SendEvent("OnPlay");

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
           // Debug.Log(hit.transform.name);
        }

        _currentFireRate = FireRate;
        _bulletCount -= 1;
    }
    
    private void OnDrawGizmos()
    {
        Debug.DrawLine(fpsCam.transform.position, fpsCam.transform.position+fpsCam.transform.forward*range);
    }
}
