using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunStatsSystem : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] float baseDamage, baseTimeBetweenShooting, baseSpread, baseRange, baseReloadTime, baseTimeBetweenShots, baseMagazineSize;
    [SerializeField] int bulletsPerTap;

    [Header("Public fields")]
    public float increasedDamage;
    public float increasedTimeBetweenShooting, increasedSpread, increasedRange, increasedReloadTime, increasedTimeBetweenShots;
    public float increasedMagazineSize;

    // total stats
    public float damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public float magazineSize;


    // Other gun info
    public bool allowButtonHold;
    float bulletsLeft, bulletsShot;
    bool shooting, readyToShoot, reloading;

    //Reference
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform attackPoint;
    [SerializeField] RaycastHit rayHitEnemy;
    [SerializeField] RaycastHit rayHitSomething;
    [SerializeField] LayerMask whatIsEnemy;
    [SerializeField] LayerMask enviormentLayer;
    [SerializeField] ShootingEnemy shootingEnemyScript;

    //Graphics
    [SerializeField] GameObject muzzleFlash, bulletHoleGraphic;
    [SerializeField] TrailRenderer bulletTrail;

    [SerializeField] float camShakeMagnitude, camShakeDuration;
    [SerializeField] TextMeshProUGUI text;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }

    private void Update()
    {
        UpdateStats();
        MyInput();

        //SetText
        text.SetText(bulletsLeft + " / " + magazineSize);
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0){
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
        // CameraShake.Instance.ShakeCamera(camShakeMagnitude, camShakeDuration);

        //Graphics 
        GameObject flash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);        
        Destroy(flash, 3);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if(bulletsShot > 0 && bulletsLeft > 0)
        Invoke("Shoot", timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
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
        timeBetweenShooting = baseTimeBetweenShooting + increasedTimeBetweenShooting;
        spread = baseSpread + increasedSpread;
        range = baseRange + increasedRange;
        reloadTime = baseReloadTime + increasedReloadTime;
        timeBetweenShots = baseTimeBetweenShots + timeBetweenShots;
        magazineSize = baseMagazineSize + increasedMagazineSize;
    }
}
