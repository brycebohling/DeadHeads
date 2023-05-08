using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerC : MonoBehaviour
{
    [SerializeField] GameObject playerSprite;
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] Transform orientation;
    [SerializeField] float groundDrag;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultiplier;
    bool readyToJump = true;

    [Header("Ground Check")]
    [SerializeField] float halfPlayerHeight;
    [SerializeField] LayerMask groundLayer;
    bool isGrounded;

    [Header("Keybinds")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;

    Rigidbody rb;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;

    Animator playerAnim;
    string _currentState;
    string WALK = "walking";
    string IDLE = "idle";
    
    
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        playerAnim = playerSprite.GetComponent<Animator>();
        rb.freezeRotation = true;
    }
    
    void Update()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, halfPlayerHeight + 0.05f, groundLayer);
        Debug.Log(isGrounded);

        GetInputs();
        
        if (isGrounded)
        {
            rb.drag = groundDrag;
        } else
        {
            rb.drag = 0f;
        }
    }

    void FixedUpdate() 
    {
        MovePlayer();
        SpeedControl();

        AnimationState();
    }

    void GetInputs()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && isGrounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed, ForceMode.Force);
        } else if (!isGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * airMultiplier, ForceMode.Force);
        }

        transform.rotation = orientation.rotation;
    }

    void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);


    }

    void ResetJump()
    {
        readyToJump = true; 
    }

    void AnimationState()
    {
        if (Mathf.Abs(rb.velocity.x) < 0.5f || Mathf.Abs(rb.velocity.z) < 0.5f)
        {
            ChangeAnimationState(IDLE);
        } else
        {
            ChangeAnimationState(WALK);
        }


    }

    void ChangeAnimationState(string newState)
    {
        if (newState == _currentState)
        {
            return;
        }

        playerAnim.Play(newState);
        _currentState = newState;
    }

    bool IsAnimationPlaying(Animator animator, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        } else
        {
            return false;
        }
    }

    void OnDrawGizmos() 
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - halfPlayerHeight - 0.05f, transform.position.z));    
    }
}
