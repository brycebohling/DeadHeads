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

    [Header("Health")]
    [SerializeField] float maxHealth;
    float currentHealth;
    [SerializeField] float iFrameTime;
    bool invincible;
    public bool isDead;

    [Header("WeaponPickUp")]
    [SerializeField] LayerMask pickUpLayer;
    [SerializeField] Camera mainCamera;
    [SerializeField] float pickUpRange;
    RaycastHit rayHitWeaponPart;
    [SerializeField] GameObject partPickUpTextObject;

    [Header("Audio")]
    [SerializeField] AudioSource walkSFX;
    [SerializeField] float timeBetweenWalkSFX;
    float timeBetweenWalkSFXTimer;


    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        playerSpeed = playerWalkSpeed;
        currentHealth = maxHealth;
        
    }

    private void Update()
    {
        if (isDead)
        {
            Debug.Log("Dead");
        }

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

        if (move.x != 0 && timeBetweenWalkSFXTimer < 0 || move.z != 0 && timeBetweenWalkSFXTimer < 0)
        {
            float randomSoundVolume = Random.Range(0.5f, 1f);
            walkSFX.PlayOneShot(walkSFX.clip, 1f);
            timeBetweenWalkSFXTimer = timeBetweenWalkSFX;
        } else
        {
            timeBetweenWalkSFXTimer -= Time.deltaTime;
        }
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

            WeaponPartC partScript = rayHitWeaponPart.collider.GetComponent<WeaponPartC>();
            partScript.ShowUI();

            if (Input.GetKeyDown(weaponPickUpKey))
            {
                partScript.Equip();
            }
        } else
        {
            partPickUpTextObject.SetActive(false);    
        }
    }

    public void DamagePlayer(float dmg)
    {
        if (!invincible)
        {
            Debug.Log("hit");
            currentHealth -= dmg;

            if (currentHealth <= 0)
            {
                isDead = true;
            }

            invincible = true;
            Invoke("ResetInvincible", iFrameTime);
        }
    }

    private void ResetInvincible()
    {
        invincible = false;
    }

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckSize);    
    }
}
