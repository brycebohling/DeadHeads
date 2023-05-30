using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class WeaponC : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent startedReloading;
    public UnityEvent finishedReloading;
    public UnityEvent startedRecoil;
    public UnityEvent finishedRecoil;

    [Header("References")]
    [SerializeField] CameraShake cameraShakeScipt;
    WeaponSwayBob weaponSwayBobScript;
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform attackPoint;
    [SerializeField] RaycastHit rayHitEnemy;
    [SerializeField] RaycastHit rayHitSomething;
    [SerializeField] LayerMask whatIsEnemy;
    [SerializeField] LayerMask enviormentLayer;
    [SerializeField] ShootingEnemy shootingEnemyScript;
    [SerializeField] Transform reloadPos;
    [SerializeField] Transform recoilPos;

    [Header("Base Stats")]
    [SerializeField] int bulletsPerTap;
    [SerializeField] float baseDamage, baseSpread, baseRange, baseReloadTime, baseTimeBetweenShots, baseMagazineSize;
    

    [Header("Public fields, DONT TOUCH")]
    public float increasedDamage;
    public float increasedSpread, increasedRange, increasedReloadTime, decreaseTimeBetweenShots;
    public float increasedMagazineSize;

    // total stats
    public float damage;
    public float spread, range, reloadTime, timeBetweenShots;
    public float magazineSize;


    // Other gun info
    public bool allowButtonHold;
    float bulletsLeft, bulletsShot;
    bool shooting, readyToShoot, reloading;

    [Header("Graphics")]
    [SerializeField] GameObject muzzleFlash, bulletHoleGraphic;
    [SerializeField] TrailRenderer bulletTrail;

    [SerializeField] float camShakeDuration;
    [SerializeField] TextMeshProUGUI text;
    
    [Header("Animations")]
    [SerializeField] float reloadLerpSpeed;
    [SerializeField] float recoilAnimLength;
    float recoilAnimTimer;
    bool playRecoilAnim;
    [SerializeField] float recoilLerpSpeed;

    [Header("Audio")]
    [SerializeField] AudioSource shootSFX;
    [SerializeField] AudioSource reloadSFX;
    [SerializeField] float timeBetweenReloadSFX;
    

    private void Start() 
    {
        UpdateStats();

        weaponSwayBobScript = GetComponent<WeaponSwayBob>();

        recoilAnimTimer = recoilAnimLength;
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        UpdateStats();
        MyInput();

        if (reloading)
        {
            ReloadingAnim();
        } else if (playRecoilAnim)
        {
            RecoilAnim();

            recoilAnimTimer -= Time.deltaTime;

            if (recoilAnimTimer <= 0)
            {
                playRecoilAnim = false;
                finishedRecoil.Invoke();
                recoilAnimTimer = recoilAnimLength;
            }
        }

        //SetText
        text.SetText(bulletsLeft + " / " + magazineSize);
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
        {
            StartCoroutine(Reload());
        }
        
        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = mainCamera.transform.forward + new Vector3(x, y, 0);

        //RayCast
        bool hitEnemy = Physics.Raycast(mainCamera.transform.position, direction, out rayHitEnemy, range, whatIsEnemy);
        bool hitSomthing = Physics.Raycast(mainCamera.transform.position, direction, out rayHitSomething, range, enviormentLayer);

        if (hitEnemy)
        {
            shootingEnemyScript.DmgEnemy(damage, rayHitEnemy.collider);
            TrailRenderer trail = Instantiate(bulletTrail, attackPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, rayHitEnemy));

            GameObject bulletHole = Instantiate(bulletHoleGraphic, rayHitEnemy.point, Quaternion.LookRotation(rayHitEnemy.normal));
    
            Destroy(bulletHole, 5);
            
        } else if (hitSomthing)
        {
            TrailRenderer trail = Instantiate(bulletTrail, attackPoint.position, Quaternion.identity);
            StartCoroutine(SpawnTrail(trail, rayHitSomething));
            
            GameObject bulletHole = Instantiate(bulletHoleGraphic, rayHitSomething.point, Quaternion.LookRotation(rayHitSomething.normal));
            Destroy(bulletHole, 5);
        }

        //ShakeCamera
        cameraShakeScipt.Shake();

        //Graphics 
        GameObject flash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);        
        Destroy(flash, 3);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShots);

        playRecoilAnim = true;
        startedRecoil.Invoke();

        shootSFX.PlayOneShot(shootSFX.clip, 1f);
    }

    private void RecoilAnim()
    {
        transform.position = Vector3.Lerp(transform.position, recoilPos.position, recoilLerpSpeed);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private IEnumerator Reload()
    {
        startedReloading.Invoke();

        reloading = true;
        Invoke("ReloadFinished", reloadTime);

        while(reloading)
        {
            reloadSFX.PlayOneShot(reloadSFX.clip, 1f);

            yield return new WaitForSeconds(timeBetweenReloadSFX);
        }
    }

    private void ReloadingAnim()
    {
        transform.position = Vector3.Lerp(transform.position, reloadPos.position, reloadLerpSpeed);
    }

    private void ReloadFinished()
    {
        finishedReloading.Invoke();

        bulletsLeft = magazineSize;
        reloading = false;
    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0f;
        Vector3 startPos = trail.transform.position;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPos, hit.point, time);

            time += Time.deltaTime / trail.time;

            yield return null;
        }

        trail.transform.position = hit.point;

        Destroy(trail.gameObject, trail.time);
    }
    
    private void UpdateStats()
    {
        damage = baseDamage + increasedDamage;
        spread = baseSpread + increasedSpread;
        range = baseRange + increasedRange;
        reloadTime = baseReloadTime + increasedReloadTime;
        timeBetweenShots = baseTimeBetweenShots - decreaseTimeBetweenShots;
        magazineSize = baseMagazineSize + increasedMagazineSize;
    }
}
