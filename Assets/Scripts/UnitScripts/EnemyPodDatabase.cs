using System.Collections.Generic;
using UnityEngine;

/// 
/// THIS DATABASE HOLDS ALL OF THE ENEMY PODS THAT CAN SPAWN, GENERATED AT RUNTIME
/// 

public class EnemyPodDatabase
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public GameManager gameManager;
    public EnemyUnitDatabase enemyUnitDatabase;

    public Dictionary<string, EnemyPod> enemyPods;
    public Dictionary<string, EnemyUnitType> enemyUnitTypes;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /// Constructor
    public EnemyPodDatabase()
    {
        enemyPods = new Dictionary<string, EnemyPod>();
        enemyUnitDatabase = new EnemyUnitDatabase();

        GeneratePodList();
    }

    /// Generate the list of possible pods at runtime
    public void GeneratePodList()
    {
        enemyUnitTypes = enemyUnitDatabase.GenerateEnemyUnitDatabase();

        EnemyPod newPod = new EnemyPod();

        newPod.name = "ZombiePod";
        newPod.status = "Asleep";

        newPod.unitsInPod.Add(enemyUnitTypes["Zombie"]);
        newPod.unitsInPod.Add(enemyUnitTypes["Zombie"]);

        enemyPods.Add(newPod.name, newPod);
    }

    /// List out the contents
    public void ListItemDatabase()
    {
        foreach (KeyValuePair<string, EnemyPod> kvp in enemyPods)
        {
            Debug.Log(kvp);
        }
    }
}
