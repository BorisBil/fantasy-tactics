using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public GameObject _unitManager;

    public List<Unit> playerUnits;
    public List<Unit> enemyUnits;

    public EnemyUnitType[] enemyUnitType;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    public void NewPlayerUnit(Vector3Int spawnAt)
    {
        GameObject unitModel = Resources.Load("Prefabs/playerUnit") as GameObject;
        GameObject spawnedUnit = Instantiate(unitModel, spawnAt, Quaternion.identity);
        
        Unit unit = spawnedUnit.GetComponent<Unit>();

        unit.name = "Player";
        unit.unitDescription = "Player unit";
        unit.unitType = "Fighter";

        unit.unitSpeed = 5;
        unit.unitRange = 1;

        unit.unitModel = unitModel;
        unit.unitObject = spawnedUnit;

        unit.unitPosition = spawnAt;

        unit.transform.parent = _unitManager.transform;

        playerUnits.Add(unit);
    }
    
    public void SpawnEnemyUnit(EnemyUnitType unitType, Vector3Int spawnAt)
    {
        GameObject unitModel = unitType.unitModel;
        GameObject spawnedEnemy = (GameObject)Instantiate(unitModel, spawnAt, Quaternion.identity);

        Unit unit = spawnedEnemy.GetComponent<Unit>();

        unit.name = unitType.name;
        unit.unitDescription = unitType.unitDescription;
        unit.unitType = unitType.unitType;

        unit.unitSpeed = unitType.unitSpeed;
        unit.unitRange = unitType.unitRange;

        unit.unitModel = unitModel;
        unit.unitObject = spawnedEnemy;

        unit.unitPosition = spawnAt;

        unit.transform.parent = _unitManager.transform;

        enemyUnits.Add(unit);
    }

    [System.Serializable]
    public class EnemyUnitType
    {
        public string name;

        public GameObject unitModel;

        public List<Node> currentPath = null;

        public string unitDescription;
        public string unitType;

        public int unitSpeed;
        public int unitRange;

        public int unitTeam;
    }
}
