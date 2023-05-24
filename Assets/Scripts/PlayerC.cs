using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerC : MonoBehaviour
{
    CharacterController controller;

    [Header("Keybinds")]
    [SerializeField] KeyCode sprint;

    
    

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
        if (Input.GetKeyDown(sprint))
        {
            playerSpeed = playerRunSpeed;
        }
        if (Input.GetKeyUp(sprint))
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

    private void OnDrawGizmos() 
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckSize);    
    }
}
