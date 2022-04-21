using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    Idle,
    Chasing,
    Unique,
    Attacking,
    Death
}
public class Enemy_Base : MonoBehaviour, IDamageable
{
    public EnemyStates E_State;
    public int maxHealth;
    public int currentHealth;
    public float time;
    public LayerMask layer;
    public float attackRange;
    public Rigidbody2D rb;
    public bool isFacingRight;
    public Vector2 moveDirection = Vector2.zero;
    public float speed;
    public Animator anim;
    public bool canAttack;
    public float attacktime;
    public bool inRange;
    public GameObject playerDetectionMark;
    public GameObject bombDetectionMark;
    public Collider2D[] col;
    public Transform rayCastPos;
    public LayerMask whatisGround;
    public float timer;

    private void Start()
    {
        maxHealth = 2;
        currentHealth = maxHealth;
        layer = LayerMask.GetMask("Player") | LayerMask.GetMask("AttackAble");
    }
    public virtual void Update()
    {
        if (canAttack)
        {
            attacktime += Time.deltaTime;
        }
        if (attacktime >= 0.5f && inRange)
        {
            attacktime = 0f;
        }
    }
    public virtual void FixedUpdate()
    {
        if (E_State != EnemyStates.Death)
        {
            col = Physics2D.OverlapBoxAll(transform.position, new Vector2(6f, 0.5f), 0, layer);
            if (col.Length != 0)
            {
                for (int i = 0; i < col.Length - 1; i++)
                {
                    int minCol = i;
                    for (int j = i + 1; j < col.Length; j++)
                    {
                        float dis1 = Vector2.Distance(col[j].transform.position, transform.position);
                        float dis2 = Vector2.Distance(col[minCol].transform.position, transform.position);
                        if (dis1 < dis2)
                        {
                            minCol = j;
                        }
                        Collider2D temp = col[minCol];
                        col[minCol] = col[i];
                        col[i] = temp;
                    }
                }
                if (col[0].GetComponent<PlayerMovement>() != null)
                {
                    playerDetectionMark.gameObject.SetActive(true);
                    if (col[0].transform.position.x - transform.position.x > 0 && moveDirection.x != 1)
                    {
                        moveDirection.x = 1;
                    }
                    else if (col[0].transform.position.x - transform.position.x < 0 && moveDirection.x != -1)
                    {
                        moveDirection.x = -1;
                    }
                    if (moveDirection.x < 0)
                    {
                        transform.rotation = new Quaternion(0, 180, 0, 0);
                    }
                    else
                    {
                        transform.rotation = Quaternion.identity;
                    }
                    time += Time.deltaTime;
                    if (time >= 1.5f)
                    {
                        playerDetectionMark.gameObject.SetActive(false);
                        inRange = true;

                        RaycastHit2D groundCol = Physics2D.Raycast(rayCastPos.position, Vector2.down, 1f, whatisGround);
                        if (groundCol)
                        {
                            if (Vector2.Distance(col[0].transform.position, transform.position) > attackRange)
                            {
                                E_State = EnemyStates.Chasing;
                                canAttack = false;
                                anim.SetBool("canRun", true);
                                transform.position += (Vector3)(speed * Time.deltaTime * moveDirection);
                            }
                            else
                            {
                                anim.SetBool("canRun", false);
                                attacktime = 0f;
                                canAttack = true;
                                Rigidbody2D detected = col[0].GetComponent<Rigidbody2D>();
                                if (detected != null)
                                {
                                    timer += Time.deltaTime;
                                    anim.SetTrigger("canAttack");
                                    E_State = EnemyStates.Attacking;
                                    if (timer >= 0.5)
                                    {
                                        IDamageable player = detected.GetComponent<IDamageable>();
                                        player.TakeDamage();
                                        time = 0;
                                        timer = 0;
                                        detected.AddForceAtPosition(new Vector2(moveDirection.x * 3f, 7f), detected.position, ForceMode2D.Impulse);
                                    }
                                }
                            }
                        }
                        else
                        {
                            anim.SetBool("isScared", false);
                            anim.SetBool("canRun", false);
                        }
                    }
                    else
                    {
                        anim.SetBool("isScared", false);
                        anim.SetBool("canRun", false);
                    }
                }
                else if (col[0].GetComponent<Bomb>() != null)
                {
                    anim.SetBool("canRun", false);
                    UniqueAction();
                    E_State = EnemyStates.Unique;
                    time = 0;
                }
            }
            else if (col.Length == 0)
            {
                anim.SetBool("canRun", false);
                anim.SetBool("isScared", false);
                E_State = EnemyStates.Idle;
                playerDetectionMark.gameObject.SetActive(false);
                canAttack = false;
                time = 0;
            }
        }
    }
    public virtual void UniqueAction()
    {
        //seperate actions performed when the enemy detects bomb
    }
    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(6f, 0.5f, 0f));
    }
    public void TakeDamage()
    {
        if(E_State != EnemyStates.Death)
        {
            currentHealth -= 1;
            if (currentHealth != 0)
            {
                anim.SetTrigger("isHurt");
            }
            else
            {
                anim.SetTrigger("isDead");
                E_State = EnemyStates.Death;
            }
        }
    }
}
