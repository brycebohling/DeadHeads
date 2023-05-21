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

    [Header("Body")]
    [SerializeField] WeaponBodyListSO weaponBodyListSO;

    [Header("Rail")]
    [SerializeField] GameObject weaponARail;
    [SerializeField] Transform weaponARailAttachPoint;
    [SerializeField] GameObject weaponBRail;
    [SerializeField] Transform weaponBRailAttachPoint;
    

    [Header("Grip")]
    [SerializeField] GameObject currentGrip;
    [SerializeField] Transform gripAAttachPoint;
    [SerializeField] Transform gripBAttachPoint;

    [Header("Stock")]
    [SerializeField] GameObject currentStock;
    [SerializeField] Transform stockAAttachPoint;
    [SerializeField] Transform stockBAttachPoint;

    [Header("Scope")]
    [SerializeField] GameObject currentScope;
    [SerializeField] Transform scopeAAttachPoint;
    [SerializeField] Transform scopeBAttachPoint;

    [Header("Mag")]
    [SerializeField] GameObject currentMag;
    [SerializeField] Transform magAAttachPoint;
    [SerializeField] Transform magBAttachPoint;

    [Header("Barrel")]
    [SerializeField] GameObject currentBarrel;
    [SerializeField] Transform barrelAAttachPoint;
    [SerializeField] Transform barrelBAttachPoint;

    [Header("Muzzle")]
    [SerializeField] GameObject currentMuzzle;
    [SerializeField] Transform muzzleAAttachPoint;
    [SerializeField] Transform muzzleBAttachPoint;


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
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (weaponBodySO == weaponBodyListSO.rifleAWeaponBodySO)
            {
                ChangeBody(weaponBodyListSO.rifleBWeaponBodySO);
            } else if (weaponBodySO == weaponBodyListSO.rifleBWeaponBodySO)
            {
                ChangeBody(weaponBodyListSO.rifleAWeaponBodySO);
            }
            
        }
    }

    public void ChangeBody(WeaponBodySO weaponBodySO) 
    {
        this.weaponBodySO = weaponBodySO;

        GameObject weaponBodyGameobject = Instantiate(weaponBodySO.prefab); 
        weaponBodyGameobject.transform.parent = transform;
        weaponBodyGameobject.transform.localPosition = Vector3.zero; 

        if (weaponBodySO == weaponBodyListSO.rifleAWeaponBodySO)
        {
            GameObject railPrefab = Instantiate(weaponARail);
            railPrefab.transform.parent = weaponARailAttachPoint;
            railPrefab.transform.localPosition = Vector3.zero; 
        }

        if (weaponBodySO == weaponBodyListSO.rifleBWeaponBodySO)
        {
            GameObject railPrefab = Instantiate(weaponBRail);
            railPrefab.transform.parent = weaponBRailAttachPoint;
            railPrefab.transform.localPosition = Vector3.zero; 
        }
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

        if (weaponBodySO == weaponBodyListSO.rifleAWeaponBodySO)
        {
            currentGrip.transform.parent = gripAAttachPoint;
        } else if (weaponBodySO == weaponBodyListSO.rifleBWeaponBodySO)
        {
            currentGrip.transform.parent = gripBAttachPoint;
        }

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

        if (weaponBodySO == weaponBodyListSO.rifleAWeaponBodySO)
        {
            currentStock.transform.parent = stockAAttachPoint;
        } else if (weaponBodySO == weaponBodyListSO.rifleBWeaponBodySO)
        {
            currentStock.transform.parent = stockBAttachPoint;
        }
        
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

        if (weaponBodySO == weaponBodyListSO.rifleAWeaponBodySO)
        {
            currentScope.transform.parent = scopeAAttachPoint;
        } else if (weaponBodySO == weaponBodyListSO.rifleBWeaponBodySO)
        {
            currentScope.transform.parent = scopeBAttachPoint;
        }

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

        if (weaponBodySO == weaponBodyListSO.rifleAWeaponBodySO)
        {
            currentMag.transform.parent = magAAttachPoint;
        } else if (weaponBodySO == weaponBodyListSO.rifleBWeaponBodySO)
        {
            currentMag.transform.parent = magBAttachPoint;
        }

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

        if (weaponBodySO == weaponBodyListSO.rifleAWeaponBodySO)
        {
            currentBarrel.transform.parent = barrelAAttachPoint;
        } else if (weaponBodySO == weaponBodyListSO.rifleBWeaponBodySO)
        {
            currentBarrel.transform.parent = barrelBAttachPoint;
        }

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

        if (weaponBodySO == weaponBodyListSO.rifleAWeaponBodySO)
        {
            currentMuzzle.transform.parent = muzzleAAttachPoint;
        } else if (weaponBodySO == weaponBodyListSO.rifleBWeaponBodySO)
        {
            currentMuzzle.transform.parent = muzzleBAttachPoint;
        }

        currentMuzzle.transform.localEulerAngles = Vector3.zero;
        currentMuzzle.transform.localPosition = Vector3.zero;

        currentMuzzle.transform.localPosition = currentMuzzle.transform.localPosition + muzzleAAttachPoint.forward * weaponBarrelSO.muzzleOffset;
    }   

    public void ChangeGunStats(WeaponPartSO weaponPartSO)
    {
        switch (weaponPartSO.statType)
        {
            case WeaponPartSO.StatType.Damage:
                gunStatScript.increasedDamage = weaponPartSO.statValue;
                break;
            
            case WeaponPartSO.StatType.MagazineSize:
                gunStatScript.increasedMagazineSize = weaponPartSO.statValue;
                break;
            
            case WeaponPartSO.StatType.Range:
                gunStatScript.increasedRange = weaponPartSO.statValue;
                break;

            case WeaponPartSO.StatType.ReloadTime:
                gunStatScript.increasedReloadTime = weaponPartSO.statValue;
                break;

            case WeaponPartSO.StatType.Spread:
                gunStatScript.increasedSpread = weaponPartSO.statValue;
                break;

            case WeaponPartSO.StatType.TimeBetweenShooting:
                gunStatScript.increasedTimeBetweenShooting = weaponPartSO.statValue;
                break;
            
            case WeaponPartSO.StatType.TimeBetweenShots:
                gunStatScript.increasedTimeBetweenShots = weaponPartSO.statValue;
                break;
            
            default:
                break;
        }        
    }
}
