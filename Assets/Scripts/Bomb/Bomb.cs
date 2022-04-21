using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explodeTime;
    public Animator anim;
    public Transform detectionPos;
    public LayerMask mask;

    void Start()
    {
        explodeTime = 0;
        mask = LayerMask.GetMask("Player") | LayerMask.GetMask("Enemies") | LayerMask.GetMask("Objects");
    }
    private void FixedUpdate()
    {
        explodeTime += Time.deltaTime;
        if (explodeTime >= 3)
        {
            Collider2D[] damageAbles = Physics2D.OverlapBoxAll(detectionPos.position, new Vector2(4f, 1f), 0, mask);
            if (damageAbles != null)
            {
                foreach (Collider2D col in damageAbles)
                {
                    Vector2 direction = col.transform.position - transform.position;
                    Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                    IDamageable dmage = col.GetComponent<IDamageable>();
                    rb.AddForceAtPosition(new Vector2(direction.x * 3f,6f),rb.position,ForceMode2D.Impulse);
                    if (dmage != null)
                    {
                        dmage.TakeDamage();
                    }
                }
            }
            anim.SetBool("Explode", true);
            Destroy(gameObject, 0.6f);
            explodeTime = 0;
        }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(4f, 1f, 0f));
    }
}
