using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PlayerStates
{
    Idle,
    Run,
    Jump,
    Hurt,
    Dead
}
public enum Player
{
    Jake,
    Glenn
}
public class PlayerMovement : Player_Base, IDamageable
{
    public PlayerStates p_State;
    public Player currentPlayer;
    public Player activePlayer;
    public Player previousPlayer;
    public BombType bombType;
    public event Action OnPlayerDeath;
    [SerializeField] private float m_MaxSpeed = 10f;
    [SerializeField] private float m_JumpForce = 400f;
    [SerializeField] private LayerMask m_WhatIsGround;

    [SerializeField] private Transform m_GroundCheck;
    const float k_GroundedRadius = .2f;
    public bool m_Grounded;
    private Animator m_Anim;
    public Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    public bool isHit;
    public bool m_Jump;
    public GameObject bombPrefab;
    public const int maxBombs = 3;
    public int currentBombs;
    public float bombLoading;
    public float bombLaunchForce;
    public bool isCharging;
    public Animator bombChargeAnim;
    public GameObject bombChargeImage;
    public int maxHealth;
    public int currentHealth;
    public Image[] healthIcons;

    private void Awake()
    {
        m_Anim = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    public override void Start()
    {
        base.Start();
        p_State = PlayerStates.Idle;
        currentBombs = maxBombs;
        maxHealth = 3;
        currentHealth = maxHealth;
        m_WhatIsGround = LayerMask.GetMask("Ground") | LayerMask.GetMask("Platform");
        OnPlayerDeath += UIManager.Instance.OpenGameOverPanel;
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
        if (currentPlayer == activePlayer)
        {
            if (p_State != PlayerStates.Dead)
            {
                float h_Movement = Input.GetAxis("Horizontal");

                if (Input.GetMouseButton(0) && currentBombs > 0)
                {
                    CreateBomb();
                }
                if (Input.GetMouseButtonUp(0) && currentBombs > 0)
                {
                    DropBomb();
                }
                if (!m_Jump)
                {
                    m_Jump = Input.GetButtonDown("Jump");
                    if (m_Jump)
                    {
                        SetPlayerState(PlayerStates.Jump);
                    }
                }
                if (h_Movement != 0)
                {
                    SetPlayerState(PlayerStates.Run);
                }
                else
                {
                    SetPlayerState(PlayerStates.Idle);
                }
                Move(h_Movement, m_Jump);
                m_Jump = false;
            }
            else
            {
                m_Anim.SetTrigger("isDead");
            }
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                SetPlayer(Player.Glenn);
                Debug.Log(activePlayer);
            }
            if (currentBombs != 3)
            {
                bombLoading += Time.deltaTime;
                if (bombLoading >= 3f)
                {
                    ReSupplyBomb();
                    bombLoading = 0f;
                }
            }
        }
        bombChargeAnim.SetBool("canCharge", isCharging);
    }

    public void Move(float move, bool jump)
    {
        m_Anim.SetFloat("Speed", Mathf.Abs(move));
        if (move != 0)
        {
            m_Rigidbody2D.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2D.velocity.y);
        }

        if (move > 0 && !m_FacingRight)
        {
            Flip();
        }
        else if (move < 0 && m_FacingRight)
        {
            Flip();
        }

        if (m_Grounded && jump && m_Anim.GetBool("Ground"))
        {
            m_Grounded = false;
            m_Anim.SetBool("Ground", false);
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    private void CreateBomb()
    {
        isCharging = true;
        bombChargeImage.SetActive(true);
        bombLaunchForce = Mathf.PingPong(Time.time * 5f, 5f);
        bombChargeAnim.SetFloat("chargeValue", bombLaunchForce);
    }
    private void DropBomb()
    {
        BombController bomb = BombService.Instance.CreateBomb(transform.position, Quaternion.identity, bombType);
        bomb.bombView.GetComponent<Rigidbody2D>().AddForceAtPosition(new Vector2(bombLaunchForce * transform.localScale.x, 3f), bomb.bombView.transform.position, ForceMode2D.Impulse);
        //GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
        //bomb.GetComponent<Rigidbody2D>().AddForceAtPosition(new Vector2(bombLaunchForce * transform.localScale.x, 3f), bomb.transform.position, ForceMode2D.Impulse);
        currentBombs--;
        bombLaunchForce = 0f;
        isCharging = false;
        bombChargeImage.SetActive(false);
    }
    private void ReSupplyBomb()
    {
        currentBombs++;
    }
    private void Flip()
    {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void TakeDamage()
    {
        if (p_State != PlayerStates.Dead)
        {
            currentHealth -= 1;
            if (currentHealth > 0)
            {
                m_Anim.SetTrigger("isHurt");
            }
            else
            {
                m_Anim.SetTrigger("isDead");
                SetPlayerState(PlayerStates.Dead);
                Invoke(nameof(OnPlayerDeathEvent), 1f);
            }

            for (int i = currentHealth; i >= currentHealth; i--)
            {
                healthIcons[i].gameObject.SetActive(false);
            }
        }
    }

    void OnPlayerDeathEvent()
    {
        OnPlayerDeath?.Invoke();
    }

    public void SetPlayerState(PlayerStates state)
    {
        p_State = state;
    }

    public override void SetInActive()
    {
        base.SetInActive();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(7f, 1f, 0f));
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spikes")
        {
            if (currentHealth != 0)
            {
                m_Anim.SetTrigger("isDead");
                m_Rigidbody2D.AddForceAtPosition(new Vector2(transform.localScale.x * -3f, 6f), m_Rigidbody2D.transform.position, ForceMode2D.Impulse);
                this.enabled = false;
                currentHealth = 0;
                Invoke(nameof(OnPlayerDeathEvent), 1f);
            }
        }
    }
}