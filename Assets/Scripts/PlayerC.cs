using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerC : MonoBehaviour
{
    CharacterController controller;

    [Header("Keybinds")]
    [SerializeField] KeyCode sprintKey;
    [SerializeField] KeyCode weaponPickUpKey;

    [Header("Speeds")]
    float playerSpeed;
    Vector3 playerVelocity;
    [SerializeField] float playerWalkSpeed;
    [SerializeField] float playerRunSpeed;
    [SerializeField] float jumpHeight = 1.0f;
    float gravityValue = -9.81f;

    [SerializeField] Transform armPivot;
    Vector3 move;
    bool groundedPlayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;
    [SerializeField] float groundCheckSize;

    [Header("WeaponPickUp")]
    [SerializeField] LayerMask pickUpLayer;
    [SerializeField] Camera mainCamera;
    [SerializeField] float pickUpRange;
    RaycastHit rayHitWeaponPart;
    [SerializeField] GameObject partPickUpTextObject;



    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerSpeed = playerWalkSpeed;
        
    }

    private void Update()
    {
        HandleRunning();
        LookingAtWeaponPart();

        groundedPlayer = IsGrounded();
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        
        move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        move = armPivot.forward * move.z + armPivot.right * move.x;
        move.y = 0f;

        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void HandleRunning()
    {
        if (Input.GetKeyDown(sprintKey))
        {
            playerSpeed = playerRunSpeed;
        }
        if (Input.GetKeyUp(sprintKey))
        {
            playerSpeed = playerWalkSpeed;
        }
    }

    public Vector3 GetMoveInput()
    {
        return move;
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, groundCheckSize, ground);
    }

    void LookingAtWeaponPart()
    {
        Vector3 direction = mainCamera.transform.forward;
        bool hitPart = Physics.Raycast(mainCamera.transform.position, direction, out rayHitWeaponPart, pickUpRange, pickUpLayer);

        if (hitPart)
        {
            partPickUpTextObject.SetActive(true);

            if (Input.GetKeyDown(weaponPickUpKey))
            {
                WeaponPartC partScript = rayHitWeaponPart.collider.GetComponent<WeaponPartC>();
                partScript.Equip();
            }
        } else
        {
            partPickUpTextObject.SetActive(false);
        }
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckSize);    
    }
}
