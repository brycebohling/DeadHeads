using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerC : MonoBehaviour
{
    CharacterController controller;
    Vector3 playerVelocity;
    bool groundedPlayer;
    [SerializeField] float playerSpeed = 2.0f;
    [SerializeField] float jumpHeight = 1.0f;
    float gravityValue = -9.81f;
    [SerializeField] Transform mainCamera;


    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
    }

    private void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        move = mainCamera.forward * move.z + mainCamera.right * move.x;
        move.y = 0f;

        transform.localEulerAngles = new Vector3(0f, mainCamera.eulerAngles.y, 0f);

        controller.Move(move * Time.deltaTime * playerSpeed);

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
