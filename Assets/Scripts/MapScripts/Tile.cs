using System.Collections.Generic;
using UnityEngine;

/// 
/// TILE INFORMATION
/// 

public class Tile : MonoBehaviour
{
    public Vector3 tileLocation;
    public bool isClickable = false;

    public bool isRamp;
    public bool hasProp;

    public Dictionary<string, string> coverOnTile;
    
    public TileMap map;
}
