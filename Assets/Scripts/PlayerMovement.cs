using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator p_Anim;
    public float h_Value;
    public float moveSpeed;
    public float jumpForce;

    private void Start()
    {
        moveSpeed = 5f;
        jumpForce = 800f;
    }
    void FixedUpdate()
    {
        h_Value = Input.GetAxisRaw("Horizontal");

        Movement(h_Value);
        Jump();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            p_Anim.SetTrigger("Jump");
        }
    }

    private void Movement(float horizontal)
    {
        rb.velocity = new Vector2(horizontal * moveSpeed, 0f);
        
        if(horizontal != 0)
        {
            p_Anim.SetBool("Move",true);
            
            transform.rotation = horizontal < 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        }
        else
        {
            p_Anim.SetBool("Move", false);
        }
    }
}
