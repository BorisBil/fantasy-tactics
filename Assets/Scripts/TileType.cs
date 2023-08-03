using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileType
{
    public string name;
    public GameObject tileVisualPrefab;

    public bool isWalkable = true;
    public float movementCost = 1;
}
