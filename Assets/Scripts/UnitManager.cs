using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public List<PlayerUnits> playerUnits;
    public List<EnemyUnits> enemyUnits;

    public EnemyUnitType[] enemyUnitType;

    public GameObject _unitManager;

    public void NewPlayerUnit()
    {
        PlayerUnits unit = new PlayerUnits();

        unit.name = "Player 1";

        unit.unitModel = Resources.Load("Prefabs/playerUnit") as GameObject;
        unit.unitDescription = "Test";
        unit.unitType = "Swordsman";

        unit.unitTeam = 1;

        playerUnits.Add(unit);
    }

    public void SpawnPlayerUnit(int squadIndex, Vector3Int spawnAt)
    {
        PlayerUnits unit = playerUnits[squadIndex];
        GameObject spawnedUnit = (GameObject)Instantiate(unit.unitModel, spawnAt, Quaternion.identity);
        spawnedUnit.transform.parent = _unitManager.transform;
        unit.unitLocation = spawnAt;
    }
    
    public void SpawnEnemyUnit(EnemyUnitType unit, Vector3Int spawnAt)
    {
        GameObject enemyUnit = (GameObject)Instantiate(unit.unitModel, spawnAt, Quaternion.identity);
        enemyUnit.transform.parent = _unitManager.transform;
        AddEnemyUnitToList(unit, spawnAt);
    }

    public void AddEnemyUnitToList(EnemyUnitType unit, Vector3Int location)
    {
        EnemyUnits enemy = new EnemyUnits();
        enemy.name = unit.name;
        enemy.unitModel = unit.unitModel;
        enemy.unitDescription = unit.unitDescription;
        enemy.unitType = unit.unitType;
        enemy.unitSpeed = unit.unitSpeed;
        enemy.unitRange = unit.unitRange;
        enemy.unitTeam = unit.unitTeam;
        enemy.unitLocation = location;
        enemyUnits.Add(enemy);
    }

    [System.Serializable]
    public class PlayerUnits : Unit
    {
        public Vector3Int unitLocation;
    }

    [System.Serializable]
    public class EnemyUnitType : Unit
    {

    }

    [System.Serializable]
    public class EnemyUnits : EnemyUnitType
    {
        public Vector3Int unitLocation;
    }
}
