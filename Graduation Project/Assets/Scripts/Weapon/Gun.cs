using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

public class Gun : MonoBehaviour
{

    private const float FireRate = 0.2f;
    public int damage = 80;
    public float range = 100f;

    public Camera fpsCam;

    public Animation gunAnim;

    public VisualEffect muzzleFlash;

    public float currentFireRate = FireRate;

    public int maxBulletCount = 20;
    public int bulletCount = 20;

    public bool isShoot =false;

    public TextMeshProUGUI bulletText;
    public bool isPlayer; //플레이어인지 구분

    public GameObject bulletMark;

    public GameObject markParticle;
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
            int layerMask = (-1) - (1 << LayerMask.NameToLayer("PlayerCamera"));  // Everything에서 Player 레이어만 제외하고 충돌 체크함
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range,layerMask))
            {
                Debug.Log(hit.transform.name);
                GameObject mark =  Instantiate(bulletMark, hit.point+hit.normal*0.001f,Quaternion.identity,hit.transform);
                GameObject markP =  Instantiate(markParticle, hit.point+hit.normal*0.001f,Quaternion.identity,hit.transform);
                mark.transform.LookAt(hit.point+hit.normal);
                markP.transform.LookAt(hit.point+hit.normal);
                Destroy(mark, 5f);
                Destroy(markP, 1f);
                ClosedMonster monster = hit.transform.GetComponent<ClosedMonster>();
                RangedMonster Rmonster = hit.transform.GetComponent<RangedMonster>();
                if (monster != null)
                {
                    monster.hit(damage);
                    monster.isHit = true;
                }

                if (Rmonster != null)
                {
                    Rmonster.hit(damage);
                    monster.isHit = true;
                }
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
