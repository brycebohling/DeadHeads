using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerC : MonoBehaviour
{
    Rigidbody rb;

    float moveX;
    float moveY;
    
    
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        moveX = Input.GetAxisRaw("horizontal");
        // moveY = Input.GetAxisRaw("vertical");
    }

    void FixedUpdate() 
    {
        rb.velocity = new Vector3(moveX, 0, 0);
    }
}
