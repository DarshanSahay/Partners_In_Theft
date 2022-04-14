using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Base : MonoBehaviour
{
    public int health;
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
    public virtual void Update()
    {
        if (canAttack)
        {
            attacktime += Time.deltaTime;
        }
        if (attacktime >= 0.5f && inRange)
        {
            anim.SetTrigger("canAttack");
            attacktime = 0f;
        }
    }
    public virtual void FixedUpdate()
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
            if (col[0].GetComponent<PlatformerCharacter2D>() != null)
            {
                playerDetectionMark.gameObject.SetActive(true);
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
                    if (col[0].transform.position.x - transform.position.x > 0 && moveDirection.x != 1)
                    {
                        moveDirection.x = 1;
                    }
                    else if (col[0].transform.position.x - transform.position.x < 0 && moveDirection.x != -1)
                    {
                        moveDirection.x = -1;
                    }
                    if (Vector2.Distance(col[0].transform.position, transform.position) > attackRange)
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
                        Rigidbody2D detected = col[0].GetComponent<Rigidbody2D>();
                        if (detected != null)
                        {
                            detected.GetComponent<PlatformerCharacter2D>().isHit = true;
                            detected.AddForceAtPosition(new Vector2(moveDirection.x * 2f, 5f), detected.position, ForceMode2D.Impulse);
                        }
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
                time = 0;
            }
        }
        else if(col.Length == 0)
        {
            Debug.Log("Hello here");
            anim.SetBool("canRun", false);
            anim.SetBool("isScared", false);
            playerDetectionMark.gameObject.SetActive(false);
            canAttack = false;
            time = 0;
        }

        //foreach(Collider2D obj in col)
        //{
        //    float dis = Vector2.Distance(obj.transform.position,transform.position);
        //}
        //Collider2D colliders = Physics2D.OverlapBox(transform.position, new Vector2(6f, 0.5f), 0, layer);
        //if (colliders)
        //{
        //    if (colliders.GetComponent<PlatformerCharacter2D>() != null)
        //    {
        //        playerDetectionMark.gameObject.SetActive(true);
        //        time += Time.deltaTime;
        //        if (time >= 1.5f)
        //        {
        //            playerDetectionMark.gameObject.SetActive(false);
        //            inRange = true;
        //            if (colliders.transform.position.x - transform.position.x > 0 && moveDirection.x != 1)
        //            {
        //                moveDirection.x = 1;
        //            }
        //            else if (colliders.transform.position.x - transform.position.x < 0 && moveDirection.x != -1)
        //            {
        //                moveDirection.x = -1;
        //            }
        //            if (Vector2.Distance(colliders.transform.position, transform.position) > attackRange)
        //            {
        //                canAttack = false;
        //                anim.SetBool("canRun", true);
        //                transform.position += (Vector3)(speed * Time.deltaTime * moveDirection);
        //            }
        //            else
        //            {
        //                anim.SetBool("canRun", false);
        //                attacktime = 0f;
        //                canAttack = true;
        //                Rigidbody2D detected = colliders.GetComponent<Rigidbody2D>();
        //                if (detected != null)
        //                {
        //                    detected.GetComponent<PlatformerCharacter2D>().isHit = true;
        //                    detected.AddForceAtPosition(new Vector2(moveDirection.x * 2f, 5f), detected.position, ForceMode2D.Impulse);
        //                }
        //            }
        //            if (moveDirection.x < 0)
        //            {
        //                transform.rotation = new Quaternion(0, 180, 0, 0);
        //            }
        //            else
        //            {
        //                transform.rotation = Quaternion.identity;
        //            }
        //        }

        //    }
        //    else
        //    {
        //        anim.SetBool("isScared", false);
        //    }
        //}
        //else
        //{
        //    anim.SetBool("canRun", false);
        //    playerDetectionMark.gameObject.SetActive(false);
        //    canAttack = false;
        //    time = 0;
        //}
    }
    public virtual void UniqueAction()
    {

    }
    public virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(6f, 0.5f, 0f));
    }
}
