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

    private Animation _gunAnim;

    public VisualEffect muzzleFlash;

    private float _currentFireRate =FireRate;

    private int _bulletCount = 20;
    // Start is called before the first frame update
    void Start()
    {
        _gunAnim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();
        
        if (Input.GetButton("Fire1"))
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
        if (!_gunAnim.isPlaying)
        {
            _gunAnim.Play();
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
