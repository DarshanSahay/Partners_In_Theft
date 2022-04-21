using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGuyEnemy : Enemy_Base
{
    public Transform bombHoldPos;
    public Transform detectionPos;
    private float bombPickUpTime;

    private void Start()
    {
        maxHealth = 2;
        currentHealth = maxHealth;
    }
    public override void Update()
    {
        base.Update();
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
    public override void UniqueAction()
    {
        base.UniqueAction();
        if (col.Length != 0)
        {
            Vector2 direction = col[0].transform.position - transform.position;
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
            if (Vector2.Distance(col[0].transform.position, transform.position) > 0.2f)
            {
                anim.SetBool("canRun", true);
                transform.position += (Vector3)(speed * Time.deltaTime * direction * 0.3f);
            }
            else
            {
                anim.SetBool("canRun", false);
                anim.SetTrigger("canPick");
                col[0].transform.position = Vector3.Lerp(col[0].transform.position, bombHoldPos.position, Time.deltaTime * 0.4f);
                bombPickUpTime += Time.deltaTime;
                if (bombPickUpTime > 0.5f)
                {
                    Rigidbody2D rb = col[0].GetComponent<Rigidbody2D>();
                    rb.AddForceAtPosition(new Vector2(moveDirection.x * 6f, 5f), rb.position, ForceMode2D.Impulse);
                    bombPickUpTime = 0;
                }
            }
        }
    }
    public override void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(detectionPos.position, new Vector3(6f, 0.5f, 0f));
    }
}
