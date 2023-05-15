using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttachmentSystem : MonoBehaviour
{
    private GunStatsSystem gunStatScript;
    private WeaponBodySO weaponBodySO;
    private WeaponPartListSO weaponPartListSO;
    [SerializeField] Transform attackPointOrigin;
    [SerializeField] Transform attackPoint;
    [SerializeField] GameObject rail;
    [SerializeField] Transform railAttachPoint;

    [Header("Body")]
    [SerializeField] WeaponBodyListSO weaponBodyListSO;

    [Header("Grip")]
    [SerializeField] GameObject currentGrip;
    [SerializeField] Transform gripAttachPoint;

    [Header("Stock")]
    [SerializeField] GameObject currentStock;
    [SerializeField] Transform stockAttachPoint;

    [Header("Scope")]
    [SerializeField] GameObject currentScope;
    [SerializeField] Transform scopeAttachPoint;

    [Header("Mag")]
    [SerializeField] GameObject currentMag;
    [SerializeField] Transform magAttachPoint;

    [Header("Barrel")]
    [SerializeField] GameObject currentBarrel;
    [SerializeField] Transform barrelAttachPoint;

    [Header("Muzzle")]
    [SerializeField] GameObject currentMuzzle;
    [SerializeField] Transform muzzleAttachPoint;


    private void Start() 
    {
        gunStatScript = GetComponent<GunStatsSystem>();

        ChangeBody(weaponBodyListSO.rifleAWeaponBodySO);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeGrip();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeStock();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeScope();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeMag();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeBarrel();
        }
    }

    public void ChangeBody(WeaponBodySO weaponBodySO) 
    {
        this.weaponBodySO = weaponBodySO;

        GameObject weaponBodyGameobject = Instantiate(weaponBodySO.prefab); 
        weaponBodyGameobject.transform.parent = transform;
        weaponBodyGameobject.transform.localPosition = Vector3.zero; 

        // Temp code for rail
        GameObject railPrefab = Instantiate(rail);
        railPrefab.transform.parent = railAttachPoint;
        railPrefab.transform.localPosition = Vector3.zero; 
    }

    public void ChangeGrip()
    {
        // Remove grip from before
        Destroy(currentGrip);

        // Creates new grip
        List<WeaponPartSO> listOfPartTypes = weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Grip);

        int randomIndex = Random.Range(0, listOfPartTypes.Count);
    
        GameObject prefab = listOfPartTypes[randomIndex].prefab;
        currentGrip = Instantiate(prefab);

        currentGrip.transform.parent = gripAttachPoint;
        currentGrip.transform.localEulerAngles = Vector3.zero;
        currentGrip.transform.localPosition = Vector3.zero;

        ChangeGunStats(listOfPartTypes[randomIndex]);
    }

    public void ChangeStock()
    {
        // Remove Stock from before
        Destroy(currentStock);

        // Creates new Stock
        List<WeaponPartSO> listOfPartTypes = weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Stock);

        int randomIndex = Random.Range(0, listOfPartTypes.Count);
        
        GameObject prefab = listOfPartTypes[randomIndex].prefab;
        currentStock = Instantiate(prefab);

        currentStock.transform.parent = stockAttachPoint;
        currentStock.transform.localEulerAngles = Vector3.zero;
        currentStock.transform.localPosition = Vector3.zero;

        ChangeGunStats(listOfPartTypes[randomIndex]);
    }

    public void ChangeScope()
    {
        // Remove Scope from before
        Destroy(currentScope);

        // Creates new Scope
        List<WeaponPartSO> listOfPartTypes = weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Scope);

        int randomIndex = Random.Range(0, listOfPartTypes.Count);
        
        GameObject prefab = listOfPartTypes[randomIndex].prefab;
        currentScope = Instantiate(prefab);

        currentScope.transform.parent = scopeAttachPoint;
        currentScope.transform.localEulerAngles = Vector3.zero;
        currentScope.transform.localPosition = Vector3.zero;

        ChangeGunStats(listOfPartTypes[randomIndex]);
    }

    public void ChangeMag()
    {
        // Remove Scope from before
        Destroy(currentMag);

        // Creates new Scope
        List<WeaponPartSO> listOfPartTypes = weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Mag);

        int randomIndex = Random.Range(0, listOfPartTypes.Count);
        
        GameObject prefab = listOfPartTypes[randomIndex].prefab;
        currentMag = Instantiate(prefab);

        currentMag.transform.parent = magAttachPoint;
        currentMag.transform.localEulerAngles = Vector3.zero;
        currentMag.transform.localPosition = Vector3.zero;

        ChangeGunStats(listOfPartTypes[randomIndex]);
    }

    public void ChangeBarrel() 
    {
        // Remove Barrel from before
        Destroy(currentBarrel);

        // Creates new Barrel
        List<WeaponPartSO> listOfPartTypes = weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Barrel);

        int randomIndex = Random.Range(0, listOfPartTypes.Count);
        
        GameObject prefab = listOfPartTypes[randomIndex].prefab;
        currentBarrel = Instantiate(prefab);

        currentBarrel.transform.parent = barrelAttachPoint;
        currentBarrel.transform.localEulerAngles = Vector3.zero;
        currentBarrel.transform.localPosition = Vector3.zero;

        float barrelLength = listOfPartTypes[randomIndex].prefab.GetComponent<MeshRenderer>().bounds.size.z;

        attackPoint.position = attackPointOrigin.position + attackPointOrigin.forward * barrelLength; 

        ChangeMuzzle(listOfPartTypes[randomIndex], barrelLength);

        ChangeGunStats(listOfPartTypes[randomIndex]);
    }

    public void ChangeMuzzle(WeaponPartSO weaponPartSO, float barrelLength)
    {
        Destroy(currentMuzzle);

        WeaponBarrelSO weaponBarrelSO = (WeaponBarrelSO)weaponPartSO;

        List<WeaponPartSO> listOfPartTypes = weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Muzzle);
        
        float longestMuzzle = 0f;

        GameObject bestFitMuzzle = listOfPartTypes[0].prefab;

        foreach (WeaponPartSO muzzle in listOfPartTypes)
        {
            float muzzleLength = muzzle.prefab.GetComponent<MeshRenderer>().bounds.size.z;

            if (muzzleLength > longestMuzzle && muzzleLength < barrelLength - 0.06f)
            {
                longestMuzzle = muzzleLength;
                bestFitMuzzle = muzzle.prefab;
            }
        }

        currentMuzzle = Instantiate(bestFitMuzzle);

        currentMuzzle.transform.parent = muzzleAttachPoint;
        currentMuzzle.transform.localEulerAngles = Vector3.zero;
        currentMuzzle.transform.localPosition = Vector3.zero;

        currentMuzzle.transform.localPosition = currentMuzzle.transform.localPosition + muzzleAttachPoint.forward * weaponBarrelSO.muzzleOffset;
    }   

    public void ChangeGunStats(WeaponPartSO weaponPartSO)
    {
        switch (weaponPartSO.statType)
        {
            case WeaponPartSO.StatType.Damage:
                gunStatScript.damage += weaponPartSO.statValue;
                break;
            
            case WeaponPartSO.StatType.MagazineSize:
                gunStatScript.magazineSize += weaponPartSO.statValue;
                break;
            
            case WeaponPartSO.StatType.Range:
                gunStatScript.range += weaponPartSO.statValue;
                break;

            case WeaponPartSO.StatType.ReloadTime:
                gunStatScript.reloadTime += weaponPartSO.statValue;
                break;

            case WeaponPartSO.StatType.Spread:
                gunStatScript.spread += weaponPartSO.statValue;
                break;

            case WeaponPartSO.StatType.TimeBetweenShooting:
                gunStatScript.timeBetweenShooting += weaponPartSO.statValue;
                break;
            
            case WeaponPartSO.StatType.TimeBetweenShots:
                gunStatScript.timeBetweenShots += weaponPartSO.statValue;
                break;
            
            default:
                break;
        }
        if (weaponPartSO.statType == WeaponPartSO.StatType.Damage)
        {

        }
        
    }
}
