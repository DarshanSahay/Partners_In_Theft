using UnityEngine;

[CreateAssetMenu(fileName = "BombScriptableObject", menuName = "BombScriptableObjectList")]
public class BombSOList : ScriptableObject
{
    public BombScriptableObject[] bombTypes;
}
