using System.Collections.Generic;
using UnityEngine;

/// 
/// THIS FILE HOLDS ALL THE FUNCTIONS TO SPAWN AND DESPAWN UNITS
/// 
public class UnitManager : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public GameObject _unitManager;

    public List<Unit> playerUnits;
    public List<Unit> enemyUnits;

    public EnemyUnitType[] enemyUnitType;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /*
     * Spawning in a new player unit
     */
    public Unit NewPlayerUnit(Vector3Int spawnAt)
    {
        GameObject unitModel = Resources.Load("Prefabs/Players/playerUnit") as GameObject;
        GameObject spawnedUnit = Instantiate(unitModel, spawnAt, Quaternion.identity);
        
        Unit unit = spawnedUnit.GetComponent<Unit>();

        unit.name = "Player";
        unit.unitDescription = "Player unit";
        unit.unitType = "Fighter";

        unit.unitSpeed = 5;
        unit.baseHP = 5;
        unit.baseMovement = 5;
        unit.level = 1;

        unit.unitModel = unitModel;
        unit.unitObject = spawnedUnit;

        unit.unitPosition = spawnAt;

        unit.transform.parent = _unitManager.transform;

        unit.unitTeam = 1;

        playerUnits.Add(unit);

        return unit;
    }

    /*
     * Spawning in a new enemy unit based on type specified
     */
    public Unit SpawnEnemyUnit(EnemyUnitType unitType, Vector3Int spawnAt)
    {
        GameObject unitModel = unitType.unitModel;
        GameObject spawnedEnemy = (GameObject)Instantiate(unitModel, spawnAt, Quaternion.identity);

        Unit unit = spawnedEnemy.GetComponent<Unit>();

        unit.name = unitType.name;
        unit.unitDescription = unitType.unitDescription;
        unit.unitType = unitType.unitType;

        unit.level = 1;

        unit.unitSpeed = unitType.unitSpeed;
        unit.baseMovement = unitType.baseMovement;
        unit.baseHP = unitType.baseHP;

        unit.unitModel = unitModel;
        unit.unitObject = spawnedEnemy;

        unit.unitPosition = spawnAt;

        unit.transform.parent = _unitManager.transform;

        unit.unitTeam = 2;

        enemyUnits.Add(unit);

        return unit;
    }

    ///  Enemy unit type table
    [System.Serializable]
    public class EnemyUnitType
    {
        public string name;

        public GameObject unitModel;

        public List<Node> currentPath = null;

        public string unitDescription;
        public string unitType;

        public int unitSpeed;
        public int baseMovement;
        public int baseHP;
        public int unitTeam;
    }
}
