using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPartSpawner : MonoBehaviour
{
    [SerializeField] WeaponAttachmentSystem weaponAttachmentSystemScript;

    [Header("Stat Raneges")]
    [SerializeField] int damgeStatLow;
    [SerializeField] int damgeStatHigh;
    [SerializeField] int magSizeStatLow;
    [SerializeField] int magSizeStatHigh;
    [SerializeField] int rangeStatLow;
    [SerializeField] int rangeStatHigh;
    [SerializeField] float reloadSpeedStatLow;
    [SerializeField] float reloadSpeedStatHigh;
    [SerializeField] float spreadStatLow;
    [SerializeField] float spreadStatHigh;
    [SerializeField] float timeBetweenShotsStatLow;
    [SerializeField] float timeBetweenShotsStatHigh;

    [SerializeField] Transform WeaponPartSpawn;

    [Header("Rarity Stuffs")]
    [SerializeField] GameObject rarityParticles;
    Transform attachPoint;
    [System.Serializable] public struct RarityList
    {
        public string rarityName;
        public float chance;
        public Color particleColor;
    }

    [SerializeField] List<RarityList> rarityList;
    Color partRarityColor;

    [Header("UI")]
    [SerializeField] Canvas partCanvas;
    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnWeaponPart();
        }
    }

    public void SpawnWeaponPart()
    {
        int lenghtOfPartTypes = WeaponPartSO.PartType.GetNames(typeof(WeaponPartSO.PartType)).Length - 1; // Have to subtract 1 bc don't want to spawn muzzle
        int randomPartTypeValue = Random.Range(0, lenghtOfPartTypes);
        List<WeaponPartSO> listOfPartTypes = new List<WeaponPartSO>();

        switch (randomPartTypeValue)
        {
            case 0:
                listOfPartTypes = weaponAttachmentSystemScript.weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Grip);
                break;

            case 1:
                listOfPartTypes = weaponAttachmentSystemScript.weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Stock);
                break;

            case 2:
                listOfPartTypes = weaponAttachmentSystemScript.weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Scope);
                break;

            case 3:
                listOfPartTypes = weaponAttachmentSystemScript.weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Barrel);
                break;

            case 4:
                listOfPartTypes = weaponAttachmentSystemScript.weaponBodySO.weaponPartListSO.GetWeaponPartSOList(WeaponPartSO.PartType.Mag);
                break;

            default:
                break;
        }

        // Create random part
        int randomIndex = Random.Range(0, listOfPartTypes.Count);
    
        GameObject prefab = listOfPartTypes[randomIndex].prefab;
        GameObject part = Instantiate(prefab, WeaponPartSpawn.position, Quaternion.identity);

        WeaponPartC partScript = part.AddComponent<WeaponPartC>();

        attachPoint = weaponAttachmentSystemScript.GetPartAttachPoint(listOfPartTypes[randomIndex].partType);

        // Create random rarity
        float randomRarity = Random.Range(0f, 1f);
        float cloestValue = 1;
        int rarityListIndex = 0;            
        
        for (int i = 0; i < rarityList.Count; i++)
        {
            if (rarityList[i].chance > randomRarity && randomRarity - rarityList[i].chance < Mathf.Abs(cloestValue))
            {
                cloestValue = randomRarity - rarityList[i].chance;
                rarityListIndex = i;
            }
        }

        List<float> sliderValues = RandomValue(listOfPartTypes[randomIndex].statType);

        partScript.SetPart(listOfPartTypes[randomIndex], attachPoint, rarityParticles, rarityList[rarityListIndex].particleColor, partCanvas, sliderValues);
    }

    private List<float> RandomValue(WeaponPartSO.StatType statType)
    {
        List<float> sliderValues = new List<float>();

        switch (statType)
        {
            case WeaponPartSO.StatType.Damage:
                sliderValues.Add(Random.Range(damgeStatLow, damgeStatHigh));
                sliderValues.Add(damgeStatHigh);
                return sliderValues;
            
            case WeaponPartSO.StatType.MagazineSize:
                sliderValues.Add(Random.Range(magSizeStatLow, magSizeStatHigh));
                sliderValues.Add(magSizeStatHigh);
                return sliderValues;
   
            case WeaponPartSO.StatType.Range:   
                sliderValues.Add(Random.Range(rangeStatLow, rangeStatHigh));
                sliderValues.Add(rangeStatHigh);
                return sliderValues;
        
            case WeaponPartSO.StatType.Spread:
                sliderValues.Add(Random.Range(spreadStatLow, spreadStatHigh));
                sliderValues.Add(spreadStatHigh);
                return sliderValues;

            case WeaponPartSO.StatType.ReloadTime:
                sliderValues.Add(Random.Range(reloadSpeedStatLow, reloadSpeedStatHigh));
                sliderValues.Add(reloadSpeedStatHigh);
                return sliderValues;

            case WeaponPartSO.StatType.TimeBetweenShots:
                sliderValues.Add(Random.Range(timeBetweenShotsStatLow, timeBetweenShotsStatHigh));
                sliderValues.Add(timeBetweenShotsStatHigh);
                return sliderValues;

            default:
                sliderValues.Add(1);
                sliderValues.Add(100);
                return sliderValues;
        }
    }
}
