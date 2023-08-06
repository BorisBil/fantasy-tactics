using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public List<PlayerUnits> playerUnits;
    public EnemyUnits[] enemyUnits;

    public void NewPlayerUnit()
    {
        PlayerUnits unit = new PlayerUnits();

        unit.name = "Player 1";

        unit.unitLocation = new Vector3Int(0, 0, 0);

        unit.unitModel = Resources.Load("Prefabs/playerUnit") as GameObject;
        unit.unitDescription = "Test";
        unit.unitType = "Swordsman";

        unit.unitTeam = 1;

        playerUnits.Add(unit);
    }

    public void SpawnPlayerUnit(int squadIndex)
    {
        PlayerUnits unit = playerUnits[squadIndex];
        GameObject spawnedUnit = (GameObject)Instantiate(unit.unitModel, unit.unitLocation, Quaternion.identity);
    }
    
    public void SpawnEnemyUnit(EnemyUnits unit, Vector3Int spawnAt)
    {
        unit.unitLocation = spawnAt;
        GameObject enemyUnit = (GameObject)Instantiate(unit.unitModel, spawnAt, Quaternion.identity);
    }

    [System.Serializable]
    public class PlayerUnits : Unit
    {
        
    }

    [System.Serializable]
    public class EnemyUnits : Unit
    {

    }
}
