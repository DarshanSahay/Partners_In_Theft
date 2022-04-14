using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnReachingDoor : MonoBehaviour
{
    float time;
    public int playersReached;

    private void Update()
    {
        if(playersReached == 2)
        {
            //move back to level select screen
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.GetComponent<PlatformerCharacter2D>() != null)
        {
            time += Time.deltaTime;
            collision.GetComponent<PlatformerCharacter2D>().m_Rigidbody2D.velocity = Vector2.zero;
            collision.GetComponent<PlatformerCharacter2D>().enabled = false;
            collision.transform.position = Vector2.Lerp(collision.transform.position,transform.position, Time.smoothDeltaTime * .5f);
            if(time >= 2f)
            {
                collision.GetComponent<Animator>().Play("DoorIn");
                if(time >= 3f)
                {
                    collision.gameObject.SetActive(false);
                    playersReached++;
                    time = 0;
                }
            }
        }
    }
}
