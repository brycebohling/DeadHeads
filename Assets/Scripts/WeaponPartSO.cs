using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponPartSO : ScriptableObject
{
    public enum PartType {
        Grip,
        Stock,
        Scope,
        Barrel,
        Muzzle,
        Mag,
    }

    public enum StatType {
        Damage,
        Spread,
        Range,
        ReloadTime, 
        TimeBetweenShots,
        MagazineSize, 
        BulletsPerTap,
        Nothing,
    }

    public PartType partType;
    public GameObject prefab;
    public StatType statType;
    public float statValue;    
}

