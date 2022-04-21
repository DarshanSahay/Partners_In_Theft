using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player_Base : MonoBehaviour
{
    public GameObject jakeHealthIcon;
    public GameObject glennHealthIcon;
    public PlayerMovement movement;
    public virtual void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }
    public virtual void SetInActive()
    {
        this.enabled = false;
    }
    public void SetPlayer(Player player)
    {
        if(movement.activePlayer != player)
        {
            movement.previousPlayer = movement.activePlayer;
            movement.activePlayer = player;
            glennHealthIcon.SetActive(true);
        }
        else
        {
            movement.activePlayer = movement.previousPlayer;
            glennHealthIcon.SetActive(false);
        }
    }
}
