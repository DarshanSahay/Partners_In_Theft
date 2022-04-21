using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnReachingDoor : MonoBehaviour
{
    float time;
    public int playersReached;

    private void Update()
    {
        if(playersReached == 2)
        {
            SceneTransition.Instance.StartTransition(1);
            LevelManager.Instance.OnLevelCompletion();
            playersReached = 0;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerMovement>() != null)
        {
            time += Time.deltaTime;
            collision.GetComponent<PlayerMovement>().m_Rigidbody2D.velocity = Vector2.zero;
            collision.GetComponent<PlayerMovement>().enabled = false;
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
