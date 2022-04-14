using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float explodeTime;
    public Animator anim;

    void Start()
    {
        explodeTime = 0;
    }
    private void Update()
    {
        explodeTime += Time.deltaTime;
        if (explodeTime >= 10)
        {
            Collider2D[] damageAbles = Physics2D.OverlapBoxAll(transform.position, new Vector2(4f, 1f), 0);
            if (damageAbles != null)
            {
                foreach (Collider2D col in damageAbles)
                {
                    Vector2 direction = col.transform.position - transform.position;
                    col.GetComponent<Rigidbody2D>().AddForce(new Vector2(direction.x * 5f,5f),ForceMode2D.Impulse);
                    anim.SetBool("Explode", true);
                    Destroy(gameObject,0.6f);
                }
            }
            explodeTime = 0;
        }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(4f, 1f, 0f));
    }
}
