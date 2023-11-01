using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// 
/// THIS FILE HOLDS ALL THE FUNCTIONS TO SPAWN AND DESPAWN UNITS
/// 
public class UnitManager : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public GameManager gameManager;
    public ItemManager itemManager;

    public GameObject _unitManager;

    public List<Unit> playerUnits;
    public List<Unit> enemyUnits;
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
        unit.actionPoints = 2;

        unit.unitModel = unitModel;
        unit.unitObject = spawnedUnit;

        unit.unitPosition = spawnAt;

        unit.transform.parent = _unitManager.transform.Find("PlayerUnits");

        unit.unitTeam = 1;
        unit.isSelectable = true;

        playerUnits.Add(unit);

        itemManager.GivePlayerItems(unit);

        return unit;
    }

    /*
     * Spawning in a new enemy unit based on type specified
     */
    public Unit SpawnEnemyUnit(EnemyUnitType unitType, Vector3Int spawnAt)
    {
        GameObject unitModel = unitType.unitModel;
        GameObject spawnedEnemy = Instantiate(unitModel, spawnAt, Quaternion.identity);

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

        unit.transform.parent = _unitManager.transform.Find("EnemyUnits");

        unit.unitTeam = 2;
        unit.isSelectable = false;

        enemyUnits.Add(unit);

        itemManager.DetermineItems(unit);

        return unit;
    }

    public void UnitDeath(Unit unit)
    {
        if (unit.unitTeam == 1)
        {
            PlayerUnitDeath(unit);
        }

        if (unit.unitTeam == 2)
        {
            EnemyUnitDeath(unit);
        }
    }

    public void PlayerUnitDeath(Unit unit)
    {
        playerUnits.Remove(unit);

        Destroy(unit.gameObject);

        if (playerUnits.Count == 0)
        {
            gameManager.GameLost();
        }
    }

    public void EnemyUnitDeath(Unit unit)
    {
        enemyUnits.Remove(unit);

        Destroy(unit.gameObject);
    }
}
