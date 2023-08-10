using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;

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
        unit.unitPosition = spawnAt;
        GameObject spawnedUnit = (GameObject)Instantiate(unit.unitModel, spawnAt, Quaternion.identity);
        unit.unit = spawnedUnit;

        Unit unitInfo = spawnedUnit.GetComponent<Unit>();

        unitInfo.name = unit.name;
        unitInfo.unitModel = unit.unitModel;
        unitInfo.unitDescription = unit.unitDescription;
        unitInfo.unitType = unit.unitType;
        unitInfo.unitSpeed = unit.unitSpeed;
        unitInfo.unitRange = unit.unitRange;
        unitInfo.unitTeam = unit.unitTeam;
        unitInfo.unitPosition = spawnAt;

        spawnedUnit.transform.parent = _unitManager.transform;
    }
    
    public void SpawnEnemyUnit(EnemyUnitType unit, Vector3Int spawnAt)
    {
        EnemyUnits enemy = new EnemyUnits();

        enemy.name = unit.name;
        enemy.unitModel = unit.unitModel;
        enemy.unitDescription = unit.unitDescription;
        enemy.unitType = unit.unitType;
        enemy.unitSpeed = unit.unitSpeed;
        enemy.unitRange = unit.unitRange;
        enemy.unitTeam = unit.unitTeam;
        enemy.unitPosition = spawnAt;

        GameObject spawnedEnemy = (GameObject)Instantiate(enemy.unitModel, spawnAt, Quaternion.identity);
        enemy.unit = spawnedEnemy;
        Unit unitInfo = spawnedEnemy.GetComponent<Unit>();

        unitInfo.name = unit.name;
        unitInfo.unitModel = unit.unitModel;
        unitInfo.unitDescription = unit.unitDescription;
        unitInfo.unitType = unit.unitType;
        unitInfo.unitSpeed = unit.unitSpeed;
        unitInfo.unitRange = unit.unitRange;
        unitInfo.unitTeam = unit.unitTeam;
        unitInfo.unitPosition = spawnAt;

        spawnedEnemy.transform.parent = _unitManager.transform;

        enemyUnits.Add(enemy);
    }

    [System.Serializable]
    public class PlayerUnits
    {
        public string name;

        public GameObject unitModel;

        public List<Node> currentPath = null;

        public string unitDescription;
        public string unitType;

        public int unitSpeed;
        public int unitRange;

        public int unitTeam;

        public Vector3Int unitPosition;

        public GameObject unit;
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

    [System.Serializable]
    public class EnemyUnits : EnemyUnitType
    {
        public Vector3Int unitPosition;
        public GameObject unit;
    }
}
