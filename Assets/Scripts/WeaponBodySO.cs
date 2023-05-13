using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponBodySO : ScriptableObject
{
    public enum Body {
        WeaponA,
        WeaponB,
        Pistol,
    }

    public Body body;
    public GameObject prefab;
    public WeaponPartListSO weaponPartListSO;
}
