using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController
{

    private float currentBombTime;
    public BombModel bombModel { get; private set; }
    public BombView bombView { get; private set; }

    public BombController(BombModel _model,BombView _bombView, Vector3 _position, Quaternion _rotation)
    {
        bombModel = _model;
        bombView = GameObject.Instantiate<BombView>(_bombView, _position, _rotation);
        bombView.SetBombController(this);
        bombModel.SetBombController(this);
    }

    public void OnBombActivation(float explodeTime, Transform detectionPos,Transform bombPosition,LayerMask mask,Animator anim)
    {
        currentBombTime += Time.deltaTime;
        if (currentBombTime >= explodeTime)
        {
            Collider2D[] damageAbles = Physics2D.OverlapBoxAll(detectionPos.position, new Vector2(4f, 1f), 0, mask);
            if (damageAbles != null)
            {
                Debug.Log(damageAbles);
                Debug.Log("this is working");
                foreach (Collider2D col in damageAbles)
                {
                    Debug.Log("not working");
                    Vector2 direction = col.transform.position - bombPosition.position;
                    Debug.Log(direction);
                    Rigidbody2D rb = col.GetComponent<Rigidbody2D>();
                    IDamageable Idamage = col.GetComponent<IDamageable>();
                    rb.AddForceAtPosition(new Vector2(direction.x * 3f, 6f), rb.position, ForceMode2D.Impulse);
                    if (Idamage != null)
                    {
                        Idamage.TakeDamage();
                    }
                }
            }
            anim.SetBool("Explode", true);
        }
    }
}
