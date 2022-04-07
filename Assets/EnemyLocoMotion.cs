using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLocoMotion : MonoBehaviour
{
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
    private void Update()
    {
        if (canAttack)
        {
            attacktime += Time.deltaTime;
            anim.SetTrigger("canAttack");
            canAttack = false;
            if(attacktime >= 2f && inRange)
            {
                canAttack = true;
            }
        }
    }
    private void FixedUpdate()
    {
        Collider2D colliders = Physics2D.OverlapBox(transform.position, new Vector2(6f,1f),0,layer);
        if (colliders)
        {
            inRange = true;
            if (colliders.transform.position.x - transform.position.x > 0 && moveDirection.x != 1)
            {
                moveDirection.x = 1; 
            }
            else if (colliders.transform.position.x - transform.position.x < 0 && moveDirection.x != -1)
            {
                moveDirection.x = -1;
            }
            if (Vector2.Distance(colliders.transform.position,transform.position) > attackRange)
            {
                canAttack = false;
                anim.SetBool("canRun", true);
                transform.position += (Vector3)(speed * Time.deltaTime * moveDirection);
            }
            else
            {
                anim.SetBool("canRun", false);
                attacktime = 0f;
                canAttack = true;
                Rigidbody2D detected = colliders.GetComponent<Rigidbody2D>();
                if(detected != null)
                {
                    //detected.GetComponent<PlatformerCharacter2D>().isHit = true;
                    detected.AddForceAtPosition(new Vector2(moveDirection.x * 2f,5f),detected.position,ForceMode2D.Impulse);
                }
            }
            if (moveDirection.x < 0)
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.identity;
            }
        }
        else
        {
            anim.SetBool("canRun", false);
            inRange = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(6f, 1f, 0f));
    }
}
