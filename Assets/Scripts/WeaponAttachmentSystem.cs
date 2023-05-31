using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttachmentSystem : MonoBehaviour
{
    private WeaponC weaponCScript;
    public WeaponBodySO weaponBodySO;
    [Header("AttackPoints")]
    [SerializeField] Transform attackPointAOrigin;
    [SerializeField] Transform attackPointA;
    [SerializeField] Transform attackPointBOrigin;
    [SerializeField] Transform attackPointB;
    int weaponLayer = 8;

    [Header("Audio")]
    [SerializeField] AudioSource equipPartSFX;

    [Header("Body")]
    [SerializeField] GameObject currentWeaponBody;
    [SerializeField] WeaponBodyListSO weaponBodyListSO;

    [Header("Rail")]
    [SerializeField] GameObject currentRail;
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
        weaponCScript = GetComponent<WeaponC>();

        ChangeBody(weaponBodyListSO.rifleAWeaponBodySO);
        SpawnBasicWeapon();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            if (weaponBodySO == weaponBodyListSO.rifleAWeaponBodySO)
            {
                ChangeBody(weaponBodyListSO.rifleBWeaponBodySO);
                SpawnBasicWeapon();
            } else if (weaponBodySO == weaponBodyListSO.rifleBWeaponBodySO)
            {
                ChangeBody(weaponBodyListSO.rifleAWeaponBodySO);
                SpawnBasicWeapon();
            }
        }
    }

    public void ChangeBody(WeaponBodySO bodySO) 
    {
        Destroy(currentWeaponBody);
        Destroy(currentGrip);
        Destroy(currentStock);
        Destroy(currentScope);
        Destroy(currentBarrel);
        Destroy(currentMuzzle);
        Destroy(currentMag);

        Destroy(currentRail);
        Destroy(currentWeaponBody);

        weaponBodySO = bodySO;

        currentWeaponBody = Instantiate(weaponBodySO.prefab); 
        currentWeaponBody.transform.parent = transform;
        currentWeaponBody.transform.localPosition = Vector3.zero; 
        currentWeaponBody.transform.localRotation = Quaternion.identity;
        currentWeaponBody.layer = weaponLayer;

        if (weaponBodySO == weaponBodyListSO.rifleAWeaponBodySO)
        {
            currentRail = Instantiate(weaponARail);
            currentRail.transform.parent = weaponARailAttachPoint;
        }

        if (weaponBodySO == weaponBodyListSO.rifleBWeaponBodySO)
        {
            currentRail = Instantiate(weaponBRail);
            currentRail.transform.parent = weaponBRailAttachPoint;
        }

        currentRail.transform.localPosition = Vector3.zero; 
        currentRail.transform.localRotation = Quaternion.identity;
        currentRail.layer = weaponLayer;
    }

    public void ChangePart(WeaponPartSO weaponPartSO)
    {
        switch (weaponPartSO.partType)
        {
            case WeaponPartSO.PartType.Grip:
                ChangeGrip(weaponPartSO);
                break;

            case WeaponPartSO.PartType.Stock:
                ChangeStock(weaponPartSO);
                break;

            case WeaponPartSO.PartType.Scope:
                ChangeScope(weaponPartSO);
                break;

            case WeaponPartSO.PartType.Mag:
                ChangeMag(weaponPartSO);
                break;

            case WeaponPartSO.PartType.Barrel:
                ChangeBarrel(weaponPartSO);
                break;
            
            default:
                break;
        }

        equipPartSFX.PlayOneShot(equipPartSFX.clip, 1f);
    }

    public void SpawnBasicWeapon()
    {
        // Random Grip
        List<WeaponPartSO> listOfGripPartTypes = weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Grip);
        int randomIndex = Random.Range(0, listOfGripPartTypes.Count);
        ChangeGrip(listOfGripPartTypes[randomIndex]);

        // Random Stock
        List<WeaponPartSO> listOfStockPartTypes = weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Stock);
        randomIndex = Random.Range(0, listOfStockPartTypes.Count);
        ChangeStock(listOfStockPartTypes[randomIndex]);

        // Random Scope
        List<WeaponPartSO> listOfScopePartTypes = weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Scope);
        randomIndex = Random.Range(0, listOfScopePartTypes.Count);
        ChangeScope(listOfScopePartTypes[randomIndex]);

        // Random Barrel
        List<WeaponPartSO> listOfBarrelPartTypes = weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Barrel);
        randomIndex = Random.Range(0, listOfBarrelPartTypes.Count);
        ChangeBarrel(listOfBarrelPartTypes[randomIndex]);
    
        // Random Mag
        List<WeaponPartSO> listOfMagPartTypes = weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Mag);
        randomIndex = Random.Range(0, listOfMagPartTypes.Count);
        ChangeMag(listOfMagPartTypes[randomIndex]);        
    }

    public void ChangeGrip(WeaponPartSO weaponPartSO)
    {
        Destroy(currentGrip);

        GameObject prefab = weaponPartSO.prefab;
        currentGrip = Instantiate(prefab);
        currentGrip.layer = weaponLayer;

        Transform attachPoint = GetPartAttachPoint(WeaponPartSO.PartType.Grip);
        currentGrip.transform.parent = attachPoint; 

        currentGrip.transform.localEulerAngles = Vector3.zero;
        currentGrip.transform.localPosition = Vector3.zero;

        weaponCScript.ChangeGunStats(weaponPartSO);
    }

    public void ChangeStock(WeaponPartSO weaponPartSO)
    {
        Destroy(currentStock);
        
        GameObject prefab = weaponPartSO.prefab;
        currentStock = Instantiate(prefab);
        currentStock.layer = weaponLayer;

        Transform attachPoint = GetPartAttachPoint(WeaponPartSO.PartType.Stock);
        currentStock.transform.parent = attachPoint; 
        
        currentStock.transform.localEulerAngles = Vector3.zero;
        currentStock.transform.localPosition = Vector3.zero;

        weaponCScript.ChangeGunStats(weaponPartSO);
    }

    public void ChangeScope(WeaponPartSO weaponPartSO)
    {
        Destroy(currentScope);
        
        GameObject prefab = weaponPartSO.prefab;
        currentScope = Instantiate(prefab);
        currentScope.layer = weaponLayer;

        Transform attachPoint = GetPartAttachPoint(WeaponPartSO.PartType.Scope);
        currentScope.transform.parent = attachPoint; 

        currentScope.transform.localEulerAngles = Vector3.zero;
        currentScope.transform.localPosition = Vector3.zero;

        weaponCScript.ChangeGunStats(weaponPartSO);
    }

    public void ChangeMag(WeaponPartSO weaponPartSO)
    {
        Destroy(currentMag);
        
        GameObject prefab = weaponPartSO.prefab;
        currentMag = Instantiate(prefab);
        currentMag.layer = weaponLayer;

        Transform attachPoint = GetPartAttachPoint(WeaponPartSO.PartType.Mag);
        currentMag.transform.parent = attachPoint; 

        currentMag.transform.localEulerAngles = Vector3.zero;
        currentMag.transform.localPosition = Vector3.zero;

        weaponCScript.ChangeGunStats(weaponPartSO);
    }

    public void ChangeBarrel(WeaponPartSO weaponPartSO) 
    {
        Destroy(currentBarrel);
        
        GameObject prefab = weaponPartSO.prefab;
        currentBarrel = Instantiate(prefab);
        currentBarrel.layer = weaponLayer;

        Transform attachPoint = GetPartAttachPoint(WeaponPartSO.PartType.Barrel);
        currentBarrel.transform.parent = attachPoint; 

        currentBarrel.transform.localEulerAngles = Vector3.zero;
        currentBarrel.transform.localPosition = Vector3.zero;

        float barrelLength = weaponPartSO.prefab.GetComponent<MeshRenderer>().bounds.size.z;

        if (weaponBodySO == weaponBodyListSO.rifleAWeaponBodySO)
        {
            attackPointA.position = attackPointAOrigin.position + attackPointAOrigin.forward * barrelLength;
        } else
        {
            attackPointB.position = attackPointBOrigin.position + attackPointBOrigin.forward * barrelLength; 
        }

        

        if (weaponBodySO == weaponBodyListSO.rifleAWeaponBodySO)
        {
            ChangeMuzzle(weaponPartSO, barrelLength);
        }

        weaponCScript.ChangeGunStats(weaponPartSO);
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
        currentMuzzle.layer = weaponLayer;

        Transform attachPoint = GetPartAttachPoint(WeaponPartSO.PartType.Muzzle);
        currentMuzzle.transform.parent = attachPoint;

        currentMuzzle.transform.localEulerAngles = Vector3.zero;
        currentMuzzle.transform.localPosition = Vector3.zero;

        currentMuzzle.transform.localPosition = currentMuzzle.transform.localPosition + muzzleAAttachPoint.forward * weaponBarrelSO.muzzleOffset;
    }   

    public Transform GetPartAttachPoint(WeaponPartSO.PartType partType)
    {
        if (weaponBodySO == weaponBodyListSO.rifleAWeaponBodySO)
        {
            switch (partType)
            {
                case WeaponPartSO.PartType.Grip:
                    return gripAAttachPoint;
                
                case WeaponPartSO.PartType.Stock:
                    return stockAAttachPoint;
                
                case WeaponPartSO.PartType.Scope:
                    return scopeAAttachPoint;

                case WeaponPartSO.PartType.Barrel:
                    return barrelAAttachPoint;

                case WeaponPartSO.PartType.Mag:
                    return magAAttachPoint;
                
                case WeaponPartSO.PartType.Muzzle:
                    return muzzleAAttachPoint;
                default:
                    return null;
            }
        } else
        {
            switch (partType)
            {
                case WeaponPartSO.PartType.Grip:
                    return gripBAttachPoint;
                
                case WeaponPartSO.PartType.Stock:
                    return stockBAttachPoint;
                
                case WeaponPartSO.PartType.Scope:
                    return scopeBAttachPoint;

                case WeaponPartSO.PartType.Barrel:
                    return barrelBAttachPoint;

                case WeaponPartSO.PartType.Mag:
                    return magBAttachPoint;

                case WeaponPartSO.PartType.Muzzle:
                    return muzzleBAttachPoint;
                
                default:
                    return null;
            }
        }
    }
}
