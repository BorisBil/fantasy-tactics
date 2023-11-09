using System.Collections.Generic;
using UnityEngine;

/// 
/// THIS IS THE DATABASE FOR ALL ENEMY UNIT TYPES, GETS GENERATED AT RUNTIME
/// 

public class EnemyUnitDatabase
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public Dictionary<string, EnemyUnitType> enemyUnitDatabase;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /// Constructor
    public EnemyUnitDatabase()
    {
        enemyUnitDatabase = new Dictionary<string, EnemyUnitType>();
    }

    /// Add the unit type
    public void AddEnemyUnitType(EnemyUnitType enemyUnitType)
    {
        enemyUnitDatabase.Add(enemyUnitType.name, enemyUnitType);
    }

    /// Generate the dictionary of unit types
    public Dictionary<string, EnemyUnitType> GenerateEnemyUnitDatabase()
    {
        EnemyUnitType zombie = ScriptableObject.CreateInstance<EnemyUnitType>();
        EnemyUnitType goblin = ScriptableObject.CreateInstance<EnemyUnitType>();

        /*
         * ZOMBIE
         */

        zombie.name = "Zombie";
        zombie.unitDescription = "This is a zombie";
        zombie.unitType = "Zombie";

        zombie.unitSpeed = 3;
        zombie.baseMovement = 4;
        zombie.baseHP = 5;
        zombie.visionRadius = 6;
        zombie.unitTeam = 2;

        zombie.unitModel = Resources.Load("Prefabs/Enemies/zombie") as GameObject;

        AddEnemyUnitType(zombie);

        /*
         * GOBLIN
         */

        goblin.name = "Goblin";
        goblin.unitDescription = "This is a goblin";
        goblin.unitType = "Fighter";

        goblin.unitSpeed = 1;
        goblin.baseMovement = 5;
        goblin.baseHP = 4;
        goblin.unitTeam = 2;

        goblin.unitModel = Resources.Load("Prefabs/Enemies/goblin") as GameObject;

        AddEnemyUnitType(goblin);

        return enemyUnitDatabase;
    }

    /// List out all of the elements
    public void ListItemDatabase()
    {
        foreach (KeyValuePair<string, EnemyUnitType> kvp in enemyUnitDatabase)
        {
            Debug.Log(kvp);
        }
    }
}
