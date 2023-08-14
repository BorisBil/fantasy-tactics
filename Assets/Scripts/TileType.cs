using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 
/// TILETYPE INFORMATION SPECIFIER
/// 
public class TileType
{
    public string name;
    public GameObject tileVisualPrefab;

    public bool isWalkable = true;
    public float movementCost = 1;
}
