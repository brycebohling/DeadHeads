using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponBarrelSO : ScriptableObject
{
    public enum PartType {
        RifleA,
        RifleB,
        Pistol,
    }

    public enum StatType {
        Damage,
        TimeBetweenShooting,
        Spread,
        Range,
        ReloadTime, 
        TimeBetweenShots,
        MagazineSize, 
        BulletsPerTap,
    }

    public PartType partType;
    public GameObject prefab;
    public float muzzleOffset;
    public StatType statType;
    public float statValue;   
}
