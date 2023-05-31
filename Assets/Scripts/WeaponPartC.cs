using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPartC : MonoBehaviour
{
    WeaponAttachmentSystem weaponAttachmentSystemScript;
    WeaponC weaponCScript;
    WeaponPartSO weaponPartSO;
    Transform parentObject;
    float bobFrequency = 1;
    float bobAmplitude = 0.005f;
    Vector3 currentPos;
    float colliderRadius = 0.5f;
    MeshRenderer meshChildhRend;
    Vector3 centerOfMesh;
    float meshLength;
    float meshHeight;
    float particleLenghtFromPart = 0.18f;
    Vector3 weaponCenterPoint;
    bool equiping;
    float weaponLerp;
    float weaponLerpSpeed = 4f;
    float equipHeight = 0.5f;
    Transform attachPoint;
    Vector3 partStartPoint;
    Vector3 partControlPoint;
    Vector3 partEndPoint;
    GameObject rarityParticles;
    Color particleColor;

    Canvas partCanvas;
    Slider valueSlider;
    float partValue;

    Transform player;


    void Start() 
    {
        weaponAttachmentSystemScript = GameObject.FindWithTag("Weapon").GetComponent<WeaponAttachmentSystem>();
        weaponCScript = GameObject.FindWithTag("Weapon").GetComponent<WeaponC>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();

        SphereCollider collider = gameObject.AddComponent<SphereCollider>();
        collider.isTrigger = true;

        // Have to look for childern bc some parts have fixed pivots
        if (TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
        {
            centerOfMesh = meshRenderer.localBounds.center;
            meshLength = meshRenderer.localBounds.size.z;
            meshHeight = meshRenderer.localBounds.size.y;
        } else
        {
            meshChildhRend = GetComponentInChildren<MeshRenderer>();
            centerOfMesh = meshChildhRend.localBounds.center;
            meshLength = meshChildhRend.localBounds.size.z;
            meshHeight = meshChildhRend.localBounds.size.y;
        }
        
        collider.center = centerOfMesh;
        collider.radius = colliderRadius;

        // Creates parent to center the pivot point
        weaponCenterPoint = transform.TransformVector(centerOfMesh);
        parentObject = new GameObject().GetComponent<Transform>();

        transform.SetParent(parentObject);
        transform.parent.name = "WeaponPartParent";
        transform.parent.position = transform.position;
        transform.localPosition = Vector3.zero - weaponCenterPoint;

        // Creates the particles
        GameObject particles = Instantiate(rarityParticles, parentObject.transform.position, Quaternion.identity);
        ParticleSystem.MainModule particleSettings = particles.GetComponent<ParticleSystem>().main;
        particleSettings.startColor = particleColor;

        if (meshLength > meshHeight)
        {
            particleSettings.startSizeX = meshLength + particleLenghtFromPart;
            particleSettings.startSizeY = meshLength + particleLenghtFromPart;
            particleSettings.startSizeZ = meshLength + particleLenghtFromPart;
        } else
        {
            particleSettings.startSizeX = meshHeight + particleLenghtFromPart;
            particleSettings.startSizeY = meshHeight + particleLenghtFromPart;
            particleSettings.startSizeZ = meshHeight + particleLenghtFromPart;
        }
        

        particles.transform.SetParent(transform);
        partCanvas.transform.SetParent(parentObject);
    }

    private void Update() 
    {

        if (equiping)
        {
            if (weaponLerp < 1.0f) 
            {
                weaponLerp += weaponLerpSpeed * Time.deltaTime;

                Vector3 m1 = Vector3.Lerp(partStartPoint, partControlPoint, weaponLerp);
                Vector3 m2 = Vector3.Lerp(partControlPoint, partEndPoint, weaponLerp);
                transform.position = Vector3.Lerp(m1, m2, weaponLerp);
            } else
            {
                weaponAttachmentSystemScript.ChangePart(weaponPartSO);
                weaponCScript.ChangeGunStats(weaponPartSO, partValue);
                Destroy(transform.parent.gameObject);
            }

            return;
        }

        transform.parent.Rotate(0, 35 * Time.deltaTime, 0);
        currentPos = transform.position;
        currentPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * bobFrequency) * bobAmplitude;
        transform.position = currentPos;

        partCanvas.transform.LookAt(player);
    }

    public void SetPart(WeaponPartSO part, Transform equipPoint, GameObject particles, Color color, Canvas canvas, List<float> values)
    {
        weaponPartSO = part;
        gameObject.layer = LayerMask.NameToLayer("PickUpLayer");
        attachPoint = equipPoint;
        rarityParticles = particles;
        particleColor = color;

        partCanvas = Instantiate(canvas, transform.position, Quaternion.identity);
        // Parents partCanvas in the start func
        valueSlider = partCanvas.GetComponentInChildren<Slider>();
        valueSlider.value = values[0] / values[1];
        partValue = values[0];
    }

    public void Equip()
    {
        partCanvas.gameObject.SetActive(false);

        partStartPoint = transform.position;
        partEndPoint = attachPoint.position;
        partControlPoint = partStartPoint + (partEndPoint - partStartPoint)/2 + Vector3.up * equipHeight;

        equiping = true;

    }
}
