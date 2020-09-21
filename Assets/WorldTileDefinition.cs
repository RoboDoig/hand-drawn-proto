using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WorldTile", menuName = "ScriptableObjects/WorldTileDefinition", order = 1)]
public class WorldTileDefinition : ScriptableObject
{
    public string nameID;
    public GameObject prefab;
    public bool walkable;
}
