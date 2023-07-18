using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableTile : MonoBehaviour
{
    public int tileX;
    public int tileY;
    public int tileZ;
    public TileMap map;

    private void OnMouseUp()
    {
        map.MoveSelectedUnitTo(tileX, tileY, tileZ);
    }
}
