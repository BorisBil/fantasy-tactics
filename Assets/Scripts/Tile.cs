using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector3Int tileLocation;
    public bool isClickable = false;
    
    public TileMap map;
}
