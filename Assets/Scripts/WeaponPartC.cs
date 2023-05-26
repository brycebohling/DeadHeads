using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPartC : MonoBehaviour
{
    [Header("Weapon Pick Up")]
    WeaponAttachmentSystem weaponAttachmentSystemScript;
    int pickUpLayer;
    WeaponPartSO weaponPartSO;


    void Start() 
    {
        weaponAttachmentSystemScript = GameObject.FindWithTag("Weapon").GetComponent<WeaponAttachmentSystem>();
    }

    public void SetPart(WeaponPartSO part)
    {
        weaponPartSO = part;
        gameObject.layer = LayerMask.NameToLayer("PickUpLayer");
    }

    public void Equip()
    {
        weaponAttachmentSystemScript.ChangePart(weaponPartSO);
        Destroy(gameObject);
    }
}
