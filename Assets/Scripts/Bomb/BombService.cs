using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BombType
{
    JakeBomb,
    GlennBomb
}
public class BombService : GenericSingleton<BombService>
{
    public BombSOList bombList;

    public BombController CreateBomb(Vector3 position, Quaternion rotation, BombType bType)
    {
        foreach (BombScriptableObject bomb in bombList.bombTypes)
        {
            if (bomb.btype == bType)
            {
                BombModel bombModel = new BombModel(bomb);
                BombController bombController = new BombController(bombModel, bomb.bombPrefab.GetComponent<BombView>(), position, rotation);
                return bombController;
            }
        }
        return null;
    }
}
