using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptainEnemy : Enemy_Base
{
    public LayerMask layerMask;
    public float bombDetection;
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
            bombDetectionMark.gameObject.SetActive(true);
            bombDetection += Time.deltaTime;
            if (bombDetection >= 1.5f)
            {
                bombDetectionMark.gameObject.SetActive(false);
                Vector2 direction = col[0].transform.position - transform.position;
                if (col[0].transform.position.x - transform.position.x > 0 && moveDirection.x != -1)
                {
                    moveDirection.x = -1;
                }
                else if (col[0].transform.position.x - transform.position.x < 0 && moveDirection.x != 1)
                {
                    moveDirection.x = 1;
                }
                if (moveDirection.x < 0)
                {
                    transform.rotation = new Quaternion(0, -180, 0, 0);
                }
                else
                {
                    transform.rotation = Quaternion.identity;
                }
                if (Vector2.Distance(col[0].transform.position, transform.position) < 5f)
                {
                    anim.SetBool("isScared", true);
                    transform.position += (Vector3)(speed * Time.deltaTime * -direction * 0.25f);
                }
                else
                {
                    anim.SetBool("isScared", false);
                }
            }
        }
        else
        {
            bombDetection = 0;
            bombDetectionMark.gameObject.SetActive(false);
            anim.SetBool("isScared", false);
        }
    }
    public override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
    }
}
