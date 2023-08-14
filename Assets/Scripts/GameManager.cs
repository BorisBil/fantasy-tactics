using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 
/// MAIN GAME LOOP FILE
/// 

public class GameManager : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public TileMap tileMap;
    public UnitManager unitManager;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /// 
    /// Void Start Unity method that launches on startup
    /// 
    void Start()
    {
        tileMap.GenerateTileMap();

        unitManager.NewPlayerUnit(new Vector3Int(0, 0, 0));
        unitManager.SpawnEnemyUnit(unitManager.enemyUnitType[0], new Vector3Int(9, 9, 0));
    }
}
