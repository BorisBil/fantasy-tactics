using UnityEngine;

/// 
/// TILE INFORMATION
/// 
public class Tile : MonoBehaviour
{
    public Vector3Int tileLocation;
    public bool isClickable = false;

    public bool isRamp;
    public bool hasProp;
    
    public TileMap map;
}
