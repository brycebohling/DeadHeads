using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPartSpawner : MonoBehaviour
{
    [SerializeField] WeaponAttachmentSystem weaponAttachmentSystemScript;
    [SerializeField] Transform WeaponPartSpawn;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            SpawnWeaponPart();
        }
    }

    public void SpawnWeaponPart()
    {
        if (Input.GetKeyDown(KeyCode.P))
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

            int randomIndex = Random.Range(0, listOfPartTypes.Count);
        
            GameObject prefab = listOfPartTypes[randomIndex].prefab;
            GameObject part = Instantiate(prefab, WeaponPartSpawn.position, Quaternion.identity);

            WeaponPartC partScript = part.AddComponent<WeaponPartC>();
            part.AddComponent<BoxCollider>();
            partScript.SetPart(listOfPartTypes[randomIndex]);
        }
    }
}
