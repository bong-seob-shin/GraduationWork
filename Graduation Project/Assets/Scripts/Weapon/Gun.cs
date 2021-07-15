using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.VFX;

public class Gun : MonoBehaviour
{

    private const float FireRate = 0.1f;
    private float damage = 30;
    public float range = 300f;
    public float penetration = 50;
    public Camera fpsCam;

    public Animation gunAnim;

    public VisualEffect muzzleFlash;

    public float currentFireRate = FireRate;

    public int maxBulletCount = 40;
    public int bulletCount = 40;

    public bool isShoot =false;

    public TextMeshProUGUI bulletText;
    public bool isPlayer; //플레이어인지 구분

    public GameObject bulletMark;

    public GameObject markParticle;
    
    UIManager _uiManager;

    private Player _player;
    
    public GameObject gunMag;
    public GameObject cloneMag;
    private CameraMove retroCameraMove;

    private float reloadTime = 0f;
    // Start is called before the first frame update
    
    
    public Material insideMaterial;
    public float loadTime;
    public int generations;
    
    void Start()
    {
        gunAnim = GetComponent<Animation>();
        _uiManager = UIManager.instance;
        _player = Player.Instance;
        retroCameraMove = _player.myCam.GetComponent<CameraMove>();
    }

    // Update is called once per frame
    void Update()
    {
        GunFireRateCalc();

        if (isShoot && reloadTime<=0)
        {
            if (currentFireRate <= 0 &&bulletCount>0)
            {
                Shoot();
            }
        }

        if (isPlayer)
        {
            bulletText.text = "Bullet  " + bulletCount.ToString() + " / " + maxBulletCount.ToString();
        }
    }
    
    private void GunFireRateCalc()
    {
        if (currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime;
        }

        if (reloadTime > 0f)
        {
            reloadTime -= Time.deltaTime;
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
            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range,layerMask, QueryTriggerInteraction.Ignore))
            {
                GameObject mark =  Instantiate(bulletMark, hit.point+hit.normal*0.001f,Quaternion.identity,hit.transform);
                GameObject markP =  Instantiate(markParticle, hit.point+hit.normal*0.001f,Quaternion.identity,hit.transform);
                mark.transform.LookAt(hit.point+hit.normal);
                markP.transform.LookAt(hit.point+hit.normal);
                mark.transform.localScale = new Vector3(1/ hit.transform.localScale.x,
                    1 / hit.transform.localScale.x,
                    1 / hit.transform.localScale.x); 
                markP.transform.localScale = new Vector3(1/ hit.transform.localScale.x,
                    1 / hit.transform.localScale.x,
                    1 / hit.transform.localScale.x); 
                Destroy(mark, 5f);
                Destroy(markP, 1f);
                ClosedMonster monster = hit.transform.GetComponent<ClosedMonster>();
                RangedMonster Rmonster = hit.transform.GetComponent<RangedMonster>();
                BossMonster bossMonster = hit.transform.GetComponent<BossMonster>();
                DroneMonster droneMonster = hit.transform.GetComponent<DroneMonster>();
                Core planeCore = hit.transform.GetComponent<Core>();
                Door door = hit.transform.GetComponent<Door>();
                if (monster != null)
                {
                    monster.hit(damage,penetration);
                    monster.target = transform.parent.parent.parent;
                    monster.isHit = true;
                    monster.targetOn = true;
                }

                if (Rmonster != null)
                {
                    Rmonster.hit(damage,penetration);
                    Rmonster.target = transform.parent.parent.parent;
                    Rmonster.isHit = true;
                    Rmonster.targetOn = true;
                }

                if (droneMonster != null)
                {
                    droneMonster.hit(damage,penetration);
                    droneMonster.target = transform.parent.parent.parent;
                    droneMonster.isHit = true;
                    droneMonster.targetOn = true;
                }

                if (bossMonster != null)
                {
                    bossMonster.hit(damage,penetration);
                }

                if (planeCore != null)
                {
                    planeCore.hit(damage,penetration);
                }
                
                if (door != null)
                {
                    door.hit(damage,0.0f);
                    if (door.HP <= 0.0f)
                    {
                        insideMaterial = hit.transform.GetComponent<MeshRenderer>().materials.ElementAt(0);
                        StartCoroutine(ShatterObject(hit.transform.gameObject, hit.point, generations));
                    }
                }
                
            }
            retroCameraMove.HorizontalRetro();
            retroCameraMove.VerticalRetro();
            if (_uiManager.crossHairSize < 300.0f)
            {
                _uiManager.crossHairSize += 2400.0f * Time.deltaTime;
            }
            
            bulletCount -= 1;
        }
        currentFireRate = FireRate;
        
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
        reloadTime = 2.0f;
        gunMag.GetComponent<Rigidbody>().isKinematic = false;
        gunMag.transform.parent = null;
        gunAnim.Play("GunReload");
    }

    public void GenerateGunMag()
    {
        var go =  Instantiate(cloneMag, this.transform);
        go.SetActive(true);
        Destroy(gunMag);
        gunMag = go;
    }
}
