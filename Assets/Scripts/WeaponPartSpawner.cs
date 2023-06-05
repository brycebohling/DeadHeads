using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPartSpawner : MonoBehaviour
{
    [SerializeField] WeaponAttachmentSystem weaponAttachmentSystemScript;

    // Stat Maxes
    float maxDamgeStat;
    float maxMagSizeStat;
    float maxRangeStat;

    // These are %
    float maxReloadSpeedStat;
    float maxSpreadStat;
    float maxTimeBetweenShotsStat;

    [System.Serializable] public struct RarityStatsList
    {
        public string rarityName;
        public string statType;
        public float statLow;
        public float statHigh;
    }

    [SerializeField] List<RarityStatsList> rarityStatsList;

    bool isValuePercent;

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

    
     private void Start() 
    {
        foreach(RarityStatsList thing in rarityStatsList)
        {
            switch (thing.statType)
            {
                case "Damage":
                    if (thing.statHigh > maxDamgeStat)
                    {
                        maxDamgeStat = thing.statHigh;
                    }                    
                    break;
                
                case "MagSize":
                    if (thing.statHigh > maxMagSizeStat)
                    {
                        maxMagSizeStat = thing.statHigh;
                    }                    
                    break;

                case "Range":
                    if (thing.statHigh > maxRangeStat)
                    {
                        maxRangeStat = thing.statHigh;
                    }                    
                    break;
                
                // These are % so small is big
                case "ReloadTime":
                    if (Mathf.Abs(thing.statHigh - 1) > maxReloadSpeedStat)
                    {
                        maxReloadSpeedStat = Mathf.Abs(thing.statHigh - 1);
                    }                    
                    break;
                
                case "Spread":
                    if (Mathf.Abs(thing.statHigh - 1) > maxSpreadStat)
                    {
                        maxSpreadStat = Mathf.Abs(thing.statHigh - 1);
                    }                    
                    break;
                
                case "TimeBetweenShots":
                    if (Mathf.Abs(thing.statHigh - 1) > maxTimeBetweenShotsStat)
                    {
                        maxTimeBetweenShotsStat = Mathf.Abs(thing.statHigh - 1);
                    }                    
                    break;
                
                default:    
                    Debug.Log("Should not run thing ever");
                    break;
            }
        }
    }
    
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

        List<RarityStatsList> partStatsForSelectedRarity = new List<RarityStatsList>();

        // Adds all the correct raritys to a list
        foreach (RarityStatsList rarityStat in rarityStatsList)
        {
            if (rarityStat.rarityName == rarityList[rarityListIndex].rarityName)
            {
                partStatsForSelectedRarity.Add(rarityStat);
            }
        }

        List<float> sliderValues = RandomValue(listOfPartTypes[randomIndex].statType, partStatsForSelectedRarity);

        partScript.SetPart(listOfPartTypes[randomIndex], attachPoint, rarityList[rarityListIndex].rarityName, rarityParticles, rarityList[rarityListIndex].particleColor, partCanvas, sliderValues, isValuePercent);
    }

    private List<float> RandomValue(WeaponPartSO.StatType statType, List<RarityStatsList> statList)
    {
        List<float> sliderValues = new List<float>();

        switch (statType)
        {
            case WeaponPartSO.StatType.Damage:
                foreach (RarityStatsList rarityStat in statList)
                {
                    if (rarityStat.statType == "Damage")
                    {
                        int low = Mathf.RoundToInt(rarityStat.statLow);
                        int high = Mathf.RoundToInt(rarityStat.statHigh);
                        sliderValues.Add(Random.Range(low, high));
                        sliderValues.Add(maxDamgeStat);
                    }
                }

                isValuePercent = false;
                return sliderValues;
            
            case WeaponPartSO.StatType.MagazineSize:
                foreach(RarityStatsList rarityStat in statList)
                {
                    if (rarityStat.statType == "MagSize")
                    {
                        int low = Mathf.RoundToInt(rarityStat.statLow);
                        int high = Mathf.RoundToInt(rarityStat.statHigh);
                        sliderValues.Add(Random.Range(low, high));
                        sliderValues.Add(maxMagSizeStat);
                    }
                }

                isValuePercent = false;
                return sliderValues;
   
            case WeaponPartSO.StatType.Range:   
                foreach(RarityStatsList rarityStat in statList)
                {
                    if (rarityStat.statType == "Range")
                    {
                        int low = Mathf.RoundToInt(rarityStat.statLow);
                        int high = Mathf.RoundToInt(rarityStat.statHigh);
                        sliderValues.Add(Random.Range(low, high));
                        sliderValues.Add(maxRangeStat);
                    }
                }

                isValuePercent = false;
                return sliderValues;

            // Next 3 are stat type that are % so big is small
            case WeaponPartSO.StatType.Spread:
                foreach(RarityStatsList rarityStat in statList)
                {
                    if (rarityStat.statType == "Spread")
                    {
                        sliderValues.Add(Random.Range(rarityStat.statHigh, rarityStat.statLow));
                        sliderValues.Add(maxSpreadStat);
                    }
                }

                isValuePercent = true;
                return sliderValues;

            case WeaponPartSO.StatType.ReloadTime:
                foreach(RarityStatsList rarityStat in statList)
                {
                    if (rarityStat.statType == "ReloadTime")
                    {
                        sliderValues.Add(Random.Range(rarityStat.statHigh, rarityStat.statLow));
                        sliderValues.Add(maxReloadSpeedStat);
                    }
                }

                isValuePercent = true;
                return sliderValues;

            case WeaponPartSO.StatType.TimeBetweenShots:
                foreach(RarityStatsList rarityStat in statList)
                {
                    if (rarityStat.statType == "TimeBetweenShots")
                    {
                        sliderValues.Add(Random.Range(rarityStat.statHigh, rarityStat.statLow));
                        sliderValues.Add(maxTimeBetweenShotsStat);
                    }
                }

                isValuePercent = true;
                return sliderValues;

            default:
                Debug.Log("WeaponPartSpawnerError");
                return sliderValues;
        }
    }
}
