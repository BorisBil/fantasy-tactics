using System.Collections.Generic;
using UnityEngine;

/// 
/// ENEMYMANAGER THAT SPAWNS IN PODS AND KEEPS TABS ON SPAWNED PODS THAT ARE AWAKE OR ASLEEP
/// 

public class EnemyManager : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public UnitManager unitManager;
    public EnemyPodDatabase enemyPodDatabase;

    public List<EnemyPod> enemyPodList;
    public List<SpawnedPod> spawnedPodList;
    public List<SpawnedPod> awakePods;
    public List<SpawnedPod> asleepPods;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /// Constructor
    public void CreateEnemyDatabases()
    {
        enemyPodList = new List<EnemyPod>();

        enemyPodDatabase = new EnemyPodDatabase();

        spawnedPodList = new List<SpawnedPod>();

        awakePods = new List<SpawnedPod>();

        asleepPods = new List<SpawnedPod>();
    }

    /// Determines the pods to spawn based on the environment
    public void DetermineEnemies(int difficulty, string environment)
    {
        enemyPodList.Clear();

        if (environment == "GrassyHills")
        {
            CreateGrassyHillsEnemy(difficulty);
        }
    }

    /// Create pods for grassy hills tileset
    public void CreateGrassyHillsEnemy(int difficulty)
    {
        int numberofPods = 2;

        int j = 10;

        for (int i = 0; i < numberofPods; i++)
        {
            enemyPodList.Add(enemyPodDatabase.enemyPods["ZombiePod"]);
        }

        foreach (EnemyPod enemyPod in enemyPodList)
        {
            SpawnedPod spawnedPod = new SpawnedPod();

            spawnedPod.name = enemyPod.name;
            spawnedPod.status = enemyPod.status;
            
            foreach (EnemyUnitType enemyUnitType in enemyPod.unitsInPod)
            {
                j = j - 1;
                Vector3Int spawnAt = new Vector3Int(j, 9, 0);

                Unit unit = unitManager.SpawnEnemyUnit(enemyUnitType, spawnAt);
                unit.status = spawnedPod.status;
                spawnedPod.unitsInPod.Add(unit);
            }

            spawnedPodList.Add(spawnedPod);

            if (spawnedPod.status == "Awake")
            {
                awakePods.Add(spawnedPod);
            }
            if (spawnedPod.status == "Asleep")
            {
                asleepPods.Add(spawnedPod);
            }
        }

        for (int i = 0; i < unitManager.enemyUnits.Count; i++)
        {
            Unit unit = unitManager.enemyUnits[i];

            unit.id = i;
        }
    }
}
