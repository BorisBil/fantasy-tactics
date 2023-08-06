using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    public TileMap tileMap;
    public UnitManager unitManager;

    public UnitManager.PlayerUnits playerUnits;
    public UnitManager.EnemyUnits enemyUnits;

    public GameObject selectedUnit;

    // Start is called before the first frame update
    void Start()
    {
        tileMap.GenerateTileMap();

        unitManager.NewPlayerUnit();
        unitManager.SpawnPlayerUnit(0);
        unitManager.SpawnEnemyUnit(unitManager.enemyUnits[0], new Vector3Int(9, 9, 0));
    }
}
