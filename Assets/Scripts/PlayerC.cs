using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerC : MonoBehaviour
{
    [Header("Movement")]
    Rigidbody rb;
    [SerializeField] float moveSpeed;
    [SerializeField] Transform orientation;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }
    
    void Update()
    {
        GetInputs();
        
    }

    void FixedUpdate() 
    {
        MovePlayer();
    }

    void GetInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
    }
}
