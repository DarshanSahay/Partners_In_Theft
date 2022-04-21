using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMover : MonoBehaviour
{
    public Transform pos1, pos2;
    public bool canMove;
    public Vector3 nextPos;
    public float speed;

    void Update()
    {
        if (canMove)
        {
            if (transform.position == pos1.position)
            {
                nextPos = pos2.position;
            }
            if (transform.position == pos2.position)
            {
                nextPos = pos1.position;
            }
            transform.position = Vector3.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            other.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerMovement>() != null)
        {
            other.gameObject.transform.SetParent(null);
        }
    }
}
