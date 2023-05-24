using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponSwayBob : MonoBehaviour
{
    [Header("References")]
    [SerializeField] PlayerC playerScript;
    [SerializeField] FPCamera cameraScript;
    Rigidbody rb;

    public bool sway = true;
    public bool swayRotation = true;
    public bool bobOffset = true;
    public bool bobSway = true;

    Vector2 moveInput;
    Vector2 lookInput;

    [Header("Sway")]
    [SerializeField] float step = 0.01f;
    [SerializeField] float maxStepDistance = 0.06f;
    Vector3 swayPos;

    [Header("Sway Rotation")]
    public float rotationStep = 4f;
    public float maxRotationStep = 5f;
    Vector3 swayEulerRot;

    [Header("Smoothness")]
    float smooth = 10f;
    float smoothRot = 12f;
    private Vector3 startOffset;

    [Header("Bobbing")]
    public float speedCurve;
    float curveSin { get => Mathf.Sin(speedCurve); }
    float curveCos { get => Mathf.Cos(speedCurve); }
    public Vector3 travelLimit = Vector3.one * 0.025f;
    public Vector3 bobLimit = Vector3.one * 0.01f;
    Vector3 bobPosition;
    [Header("Bob Rotation")]
    public Vector3 multiplier;
    Vector3 bobEulerRotation;


    private void Start() 
    {
        rb = GetComponent<Rigidbody>();
        startOffset = transform.localPosition;    
    }
    private void Update() 
    {
        GetInput();

        Sway();
        SwayRotation();
        BobOffset();
        BobRotation();

        CompositePositionRotation();
    }

    private void GetInput() 
    {
        moveInput.x = playerScript.GetMoveInput().x;
        moveInput.y = playerScript.GetMoveInput().y;
        moveInput = moveInput.normalized;

        lookInput.x = cameraScript.GetMouseInput().x;
        lookInput.y = cameraScript.GetMouseInput().y;
    }

    private void Sway()
    {
        if (sway == false) { swayPos = Vector3.zero; return; }

        Vector3 invertLook = lookInput * -step;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxStepDistance, maxStepDistance);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = invertLook;
    }

    private void SwayRotation()
    {
        if (swayRotation == false) { swayEulerRot = Vector3.zero; return; }

        Vector2 invertLook = lookInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);

        swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
    }

    private void BobOffset()
    {
        speedCurve += Time.deltaTime * (playerScript.IsGrounded() ? playerScript.GetComponent<CharacterController>().velocity.magnitude : 1f) + 0.01f;

        if (bobOffset == false) { bobPosition = Vector3.zero; return; }

        bobPosition.x = (curveCos * bobLimit.x * (playerScript.IsGrounded() ? 1 : 0)) - (moveInput.x * travelLimit.x);
        bobPosition.y = (curveSin * bobLimit.y) - (rb.velocity.y * travelLimit.y); 
        bobPosition.z = - (moveInput.y * travelLimit.z);
    }
    
    private void BobRotation()
    {
        if (bobSway == false) { bobEulerRotation = Vector3.zero; return; }

        bobEulerRotation.x = (moveInput != Vector2.zero ?  multiplier.x * (Mathf.Sin(2 * speedCurve)) : multiplier.x * (Mathf.Sin(2 * speedCurve) / 2));
        bobEulerRotation.y = (moveInput != Vector2.zero ? multiplier.y * curveCos : 0);
        bobEulerRotation.z = (moveInput != Vector2.zero ? multiplier.z * curveCos * moveInput.x : 0);  
    }

    private void CompositePositionRotation()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, swayPos + bobPosition + startOffset, Time.deltaTime * smooth);

        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(swayEulerRot) * Quaternion.Euler(bobEulerRotation), Time.deltaTime * smoothRot);
    }
}