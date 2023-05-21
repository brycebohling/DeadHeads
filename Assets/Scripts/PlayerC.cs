using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerC : MonoBehaviour
{
    CharacterController controller;

    [Header("Keybinds")]
    [SerializeField] KeyCode sprint;

    Vector3 playerVelocity;
    bool groundedPlayer;

    [Header("Speeds")]
    float playerSpeed;
    [SerializeField] float playerWalkSpeed;
    [SerializeField] float playerRunSpeed;
    [SerializeField] float jumpHeight = 1.0f;
    float gravityValue = -9.81f;
    [SerializeField] Transform armPivot;


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

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
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
}
