using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    //Gun stats
    [SerializeField] int damage;
    [SerializeField] float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    [SerializeField] int magazineSize, bulletsPerTap;
    [SerializeField] bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform attackPoint;
    [SerializeField] RaycastHit rayHit;
    [SerializeField] LayerMask whatIsEnemy;

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
        if (Physics.Raycast(mainCamera.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);

            // if (rayHit.collider.CompareTag("Enemy"))
                // rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
        }

        //ShakeCamera
        CameraShake.Instance.ShakeCamera(camShakeMagnitude, camShakeDuration);

        //Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.LookRotation(rayHit.normal));
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
        TrailRenderer trail = Instantiate(bulletTrail, attackPoint.position, Quaternion.identity);
        StartCoroutine(SpawnTrail(trail, rayHit));

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
}
