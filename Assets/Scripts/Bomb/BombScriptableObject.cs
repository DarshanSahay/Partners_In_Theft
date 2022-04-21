using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bomb Data", menuName = "Bombs", order = 51)]
public class BombScriptableObject : ScriptableObject
{
    public float checkRadius;
    public Vector2 forceAmount;
    public float bombExplodeTime;
    public float maxLifeTime;
    public GameObject bombPrefab;
    public Transform detectionPosition;
    public LayerMask mask;
    public BombType btype;
}
