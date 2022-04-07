using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStates
{
    Idle,
    Run,
    Jump,
    Hurt,
    Death
}
public class PlatformerCharacter2D : MonoBehaviour
{
    public PlayerStates p_State;
    [SerializeField] private float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
    [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
    [Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;  // Amount of maxSpeed applied to crouching movement. 1 = 100%
    [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
    [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

    private Transform m_GroundCheck;                                     // A position marking where to check if the player is grounded.
    const float k_GroundedRadius = .2f;                                  // Radius of the overlap circle to determine if grounded
    public bool m_Grounded;                                              // Whether or not the player is grounded.
    private Animator m_Anim;                                             // Reference to the player's animator component.
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;                                   // For determining which way the player is currently facing.
    public bool isHit;
    public bool m_Jump;
    private void Awake()
    {
        m_GroundCheck = transform.Find("GroundCheck");
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        p_State = PlayerStates.Idle;
    }
    private void FixedUpdate()
    {
        
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                m_Grounded = true;
        }
        m_Anim.SetBool("Ground", m_Grounded);

        m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
    }
    private void Update()
    {
        bool crouch = Input.GetKey(KeyCode.LeftControl);
        float h = Input.GetAxis("Horizontal");

        if(h != 0)
        {
            SetPlayerState(PlayerStates.Run);
        }
        else
        {
            SetPlayerState(PlayerStates.Idle);
        }

        if (!m_Jump)
        {
            m_Jump = Input.GetButtonDown("Jump");
            if (m_Jump)
            {
                SetPlayerState(PlayerStates.Jump);
            }
        }

        if(isHit)
        {
            m_Anim.SetTrigger("isHurt");
            //SetPlayerState(PlayerStates.Idle);
            //isHit = false;
        }

        if (isHit == false)
        {
            Move(h, crouch, m_Jump);
            m_Jump = false;
        }
    }

    public void Move(float move, bool crouch, bool jump)
    {
        if (!isHit || m_AirControl)
        {
            move = (crouch ? move * m_CrouchSpeed : move);

            m_Anim.SetFloat("Speed", Mathf.Abs(move));

            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);

            if (move > 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (move < 0 && m_FacingRight)
            {
                Flip();
            }
        }
        if (m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }


    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void SetPlayerState(PlayerStates state)
    {
        p_State = state;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(7f, 1f, 0f));
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("enemy hit");
            m_Anim.SetTrigger("isHurt");
        }
    }
}