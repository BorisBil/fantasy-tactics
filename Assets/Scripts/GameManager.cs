using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TileMap tileMap;

    // Start is called before the first frame update
    void Start()
    {
        tileMap.GenerateTileMap();
    }
}
