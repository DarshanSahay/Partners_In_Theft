using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuyEnemy : Enemy_Base
{
    public LayerMask layerMask;
    public Transform bombHoldPos;
    public Transform detectionPos;
    private float bombPickUpTime;
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        Collider2D colliders = Physics2D.OverlapBox(detectionPos.position, new Vector2(10f, 0.5f), 0, layerMask);
        if (colliders)
        {
            if (colliders.GetComponent<Bomb>() != null)
            {
                Vector2 direction = colliders.transform.position - transform.position;
                if (colliders.transform.position.x - transform.position.x > 0 && moveDirection.x != -1)
                {
                    moveDirection.x = 1;
                }
                else if (colliders.transform.position.x - transform.position.x < 0 && moveDirection.x != 1)
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
                if (Vector2.Distance(colliders.transform.position, transform.position) > 0.2f)
                {
                    anim.SetBool("canRun", true);
                    transform.position += (Vector3)(speed * Time.deltaTime * direction * 0.3f);
                }
                else
                {
                    anim.SetBool("canRun", false);
                    anim.SetTrigger("canPick");
                    colliders.transform.position = Vector3.Lerp(colliders.transform.position, bombHoldPos.position, Time.deltaTime * 0.4f);
                    bombPickUpTime += Time.deltaTime;
                    if(bombPickUpTime > 0.5f)
                    {
                        Rigidbody2D rb = colliders.GetComponent<Rigidbody2D>();
                        rb.AddForceAtPosition(new Vector2(moveDirection.x * 7f, 5f), rb.position, ForceMode2D.Impulse);
                        bombPickUpTime = 0;
                    }
                }
            }
        }
    }
    public override void UniqueAction()
    {
        base.UniqueAction();
    }
    public override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(detectionPos.position, new Vector3(10f, 0.5f, 0f));
    }
}
