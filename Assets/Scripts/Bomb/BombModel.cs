using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombModel
{
    public GameObject BombPrefab;
    public Transform DetectionPosition;
    public BombType Type;
    public float CheckRadius { get; private set; }
    public float MaxLifeTime { get; private set; }
    public LayerMask Mask;
    public Vector2 ForceAmount { get; private set; }
    public float BombExplodeTime { get; private set; }

    private BombController b_Controller;

    public BombModel(BombScriptableObject bombSO)
    {
        CheckRadius = bombSO.checkRadius;
        ForceAmount = bombSO.forceAmount;
        BombExplodeTime = bombSO.bombExplodeTime;
        MaxLifeTime = bombSO.maxLifeTime;
        BombPrefab = bombSO.bombPrefab;
        Mask = bombSO.mask;
        DetectionPosition = bombSO.detectionPosition;
        Type = bombSO.btype;
    }

    public void SetBombController(BombController b_Controller)
    {
        this.b_Controller = b_Controller;
    }
}
