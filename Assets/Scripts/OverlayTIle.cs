using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    public float G;
    public float H;
    public float F { get { return G + H; } }

    public bool isBlocked;

    public OverlayTile previous;

    public Vector3Int gridLocation;
    public Vector2Int grid2DLocation {  get {  return new Vector2Int(gridLocation.x, gridLocation.y); } }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0);
    }

    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HideTile();
        }
    }
}
