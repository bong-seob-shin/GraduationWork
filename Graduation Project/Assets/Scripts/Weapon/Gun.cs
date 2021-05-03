using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public float currentFireRate =FireRate;

    public int maxBulletCount = 20;
    public int bulletCount = 20;

    public bool isShoot =false;

    public TextMeshProUGUI bulletText;
    public bool isPlayer; //플레이어인지 구분
    // Start is called before the first frame update
    void Start()
    {
        gunAnim = GetComponent<Animation>();

    }

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();

        if (isShoot)
        {
            if (currentFireRate <= 0 &&bulletCount>0)
            {
                Shoot();
            }
        }

        bulletText.text = "Bullet  " + bulletCount.ToString() + " / " + maxBulletCount.ToString();




    }

    private void GunFireRateCalc()
    {
        

        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }
    }

    private void Shoot()
    {
        if (!gunAnim.isPlaying)
        {
            gunAnim.Play("GunShot");
        }

        muzzleFlash.SendEvent("OnPlay");

        if (isPlayer)
        {
            RaycastHit hit;
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
            {
                // Debug.Log(hit.transform.name);
            }


            bulletCount -= 1;
        }
        currentFireRate = FireRate;

    }
    
    private void OnDrawGizmos()
    {
        if (isPlayer)
        {
            Debug.DrawLine(fpsCam.transform.position, fpsCam.transform.position + fpsCam.transform.forward * range);
        }
    }

    public void Reload()
    {
        bulletCount = maxBulletCount;
    }
}
