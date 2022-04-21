using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    public GameObject platformToOpen;
    public PlatformMover platform;
    public SpriteRenderer leverSprite;
    public Sprite onTrigger;
    public bool isPlatformActive;
    public LayerMask layer;
    void Start()
    {
        leverSprite = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 3f, layer);
        if (col != null)
        {
            if (col.GetComponent<PlayerMovement>() != null)
            {
                if (Input.GetKeyDown(KeyCode.LeftShift))
                {
                    if (!isPlatformActive)
                    {
                        platformToOpen.SetActive(true);
                        isPlatformActive = true;
                        this.enabled = false;
                    }
                    else
                    {
                        platform.canMove = true;
                    }
                    leverSprite.sprite = onTrigger;
                }
            }
        }
    }
}
