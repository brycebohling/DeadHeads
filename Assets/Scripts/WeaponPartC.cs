using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPartC : MonoBehaviour
{
    WeaponAttachmentSystem weaponAttachmentSystemScript;
    WeaponPartSO weaponPartSO;
    float bobFrequency = 1;
    float bobAmplitude = 0.005f;
    Vector3 currentPos;
    float colliderRadius = 0.5f;
    Vector3 centerOfMesh;
    Vector3 weaponCenterPoint;


    void Start() 
    {
        weaponAttachmentSystemScript = GameObject.FindWithTag("Weapon").GetComponent<WeaponAttachmentSystem>();

        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;

        // Have to look for childern bc some parts have fixed pivots
        if (TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
        {
            centerOfMesh = meshRenderer.localBounds.center;
        } else
        {
            centerOfMesh = GetComponentInChildren<MeshRenderer>().localBounds.center;
        }
        
        collider.center = centerOfMesh;
        collider.radius = colliderRadius;

        // Creates parent to center the pivot point
        weaponCenterPoint = transform.TransformVector(centerOfMesh);
        Transform parentObject = new GameObject().GetComponent<Transform>();
        transform.SetParent(parentObject);
        transform.parent.name = "WeaponPartParent";
        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero - weaponCenterPoint;
    }

    private void Update() 
    {

        transform.parent.Rotate(0, 35 * Time.deltaTime, 0);
        currentPos = transform.position;
        currentPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * bobFrequency) * bobAmplitude;
        transform.position = currentPos;
    }

    public void SetPart(WeaponPartSO part)
    {
        weaponPartSO = part;
        gameObject.layer = LayerMask.NameToLayer("PickUpLayer");
    }

    public void Equip()
    {
        weaponAttachmentSystemScript.ChangePart(weaponPartSO);
        Destroy(transform.parent.gameObject);
    }
}
