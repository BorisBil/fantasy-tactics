using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    public TileMap tileMap;
    public UnitManager unitManager;

    // Start is called before the first frame update
    void Start()
    {
        tileMap.GenerateTileMap();

        unitManager.NewPlayerUnit();
        unitManager.SpawnPlayerUnit(0, new Vector3Int(0, 0, 0));
        unitManager.SpawnEnemyUnit(unitManager.enemyUnitType[0], new Vector3Int(9, 9, 0));
    }
}
