using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public UnitManager unitManager;
    public EnemyPodDatabase enemyPodDatabase;

    public List<EnemyPod> enemyPodList;

    public void CreateEnemyDatabases()
    {
        enemyPodList = new List<EnemyPod>();

        enemyPodDatabase = new EnemyPodDatabase();
    }

    public void DetermineEnemies(int difficulty, string environment)
    {
        enemyPodList.Clear();

        if (environment == "GrassyHills")
        {
            CreateGrassyHillsEnemy(difficulty);
        }
    }

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
            foreach (EnemyUnitType enemyUnitType in enemyPod.unitsInPod)
            {
                j = j - 1;
                Vector3Int spawnAt = new Vector3Int(j, 9, 0);

                unitManager.SpawnEnemyUnit(enemyUnitType, spawnAt);
            }
        }
    }
}
