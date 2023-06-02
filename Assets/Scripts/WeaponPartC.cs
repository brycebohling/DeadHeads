using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    bool showCanvas;
    Slider valueSlider;
    float partValue;

    Transform player;
    TextMeshProUGUI statTypeText;
    TextMeshProUGUI weaponTypeText;
    Image partInfoBGUI;

    string rarity;

    Color commonColor = new Color(1f, 1f, 1f, 0.4f);
    Color rareColor = new Color(0f, 0.25f, 0.78f, 0.4f);
    Color epicColor = new Color(0.5f, 0f, 0.63f, 0.4f);
    Color legendaryColor = new Color(1f, 0.42f, 0f, 0.4f);


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
        SetUI();


    }

    private void Update() 
    {
        if (showCanvas)
        {
            partCanvas.gameObject.SetActive(true);
        } else
        {
            partCanvas.gameObject.SetActive(false);
        }

        showCanvas = false;

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

    public void SetPart(WeaponPartSO part, Transform equipPoint, string rarityName, GameObject particles, Color color, Canvas canvas, List<float> values)
    {
        weaponPartSO = part;
        gameObject.layer = LayerMask.NameToLayer("PickUpLayer");
        attachPoint = equipPoint;
        rarityParticles = particles;
        particleColor = color;
        rarity = rarityName;

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

    private void SetUI()
    {
        foreach (TextMeshProUGUI child in partCanvas.GetComponentsInChildren<TextMeshProUGUI>())
        {
            switch (child.tag)
            {
                case "StatTypeUI":
                    statTypeText = child;
                    break;

                case "WeaponTypeUI":
                    weaponTypeText = child;
                    break;

                default:
                    break;
            }
        }

        foreach (Image child in partCanvas.GetComponentsInChildren<Image>())
        {
            
            switch (child.tag)
            {
                case "PartInfoBGUI":
                    partInfoBGUI = child;
                    break;
            }
        }

        switch (weaponPartSO.partType)
        {
            case WeaponPartSO.PartType.Grip: 
                weaponTypeText.text = "Grip";
                break;
            
            case WeaponPartSO.PartType.Stock: 
                weaponTypeText.text = "Stock";
                break;
            
            case WeaponPartSO.PartType.Scope: 
                weaponTypeText.text = "Scope";
                break;
            
            case WeaponPartSO.PartType.Barrel: 
                weaponTypeText.text = "Barrel";
                break;

            case WeaponPartSO.PartType.Mag: 
                weaponTypeText.text = "Magazine";
                break;

            default:
                break;
        }

        switch (weaponPartSO.statType)
        {
            case WeaponPartSO.StatType.Damage:
                statTypeText.text = "Damage";
                break;

            case WeaponPartSO.StatType.MagazineSize:
                statTypeText.text = "Magazine Size";
                break;
            
            case WeaponPartSO.StatType.Range:
                statTypeText.text = "Range";
                break;
            
            case WeaponPartSO.StatType.ReloadTime:
                statTypeText.text = "Reload Time";
                break;
            
            case WeaponPartSO.StatType.Spread:
                statTypeText.text = "Bullet Spread";
                break;

            case WeaponPartSO.StatType.TimeBetweenShots:
                statTypeText.text = "Fire Rate";
                break;

            default:
                break;
        }

        switch (rarity)
        {
            case "Common":
                partInfoBGUI.color = commonColor;
                break;
            
            case "Rare":
                partInfoBGUI.color = rareColor;
                break;
            
            case "Epic":
                partInfoBGUI.color = epicColor;
                break;
            
            case "Legendary":
                partInfoBGUI.color = legendaryColor;
                break;

            default:
                break;
        }

        
    }

    public void ShowUI()
    {
        showCanvas = true;
    }
}
