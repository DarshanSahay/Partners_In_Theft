using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombView : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public BombController bombController { get; private set; }

    private void Start()
    {
        Destroy(gameObject, bombController.bombModel.MaxLifeTime);
    }
    public void SetBombController(BombController _bombController)
    {
        bombController = _bombController;
    }

    private void FixedUpdate()
    {
        bombController.OnBombActivation(bombController.bombModel.BombExplodeTime, bombController.bombModel.DetectionPosition,this.transform, bombController.bombModel.Mask, anim);
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(bombController.bombModel.DetectionPosition.position, new Vector3(4f, 1f, 0f));
    }
}
