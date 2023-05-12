using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttachmentSystem : MonoBehaviour
{
    [Header("Body")]
    [SerializeField] GameObject currentGunBody;

    [Header("Grip")]
    [SerializeField] List<GameObject> grips = new List<GameObject>();
    [SerializeField] GameObject currentGrip;
    [SerializeField] Transform gripAttachPoint;


    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeGrip();
        }
    }

    private void ChangeGrip()
    {
        // Remove grip from before
        Destroy(currentGrip);

        // Creates new grip
        int gripIndex = Random.Range(0, grips.Count);
        currentGrip = Instantiate(grips[gripIndex], transform.position, Quaternion.identity);

        currentGrip.transform.parent = gripAttachPoint;
        currentGrip.transform.localEulerAngles = Vector3.zero;
        currentGrip.transform.localPosition = Vector3.zero;
    }
}
