using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPodDatabase
{
    public GameManager gameManager;
    public EnemyUnitDatabase enemyUnitDatabase;

    public Dictionary<string, EnemyPod> enemyPods;
    public Dictionary<string, EnemyUnitType> enemyUnitTypes;

    public EnemyPodDatabase()
    {
        enemyPods = new Dictionary<string, EnemyPod>();
        enemyUnitDatabase = new EnemyUnitDatabase();

        GeneratePodList();
        ListItemDatabase();
    }

    public void GeneratePodList()
    {
        enemyUnitTypes = enemyUnitDatabase.GenerateEnemyUnitDatabase();
        enemyUnitDatabase.ListItemDatabase();

        EnemyPod newPod = new EnemyPod();

        newPod.name = "ZombiePod";
        newPod.status = "Asleep";

        foreach (KeyValuePair<string, EnemyUnitType> kvp in enemyUnitTypes)
        {
            Debug.Log(kvp);
        }

        newPod.unitsInPod.Add(enemyUnitTypes["Zombie"]);
        newPod.unitsInPod.Add(enemyUnitTypes["Zombie"]);

        enemyPods.Add(newPod.name, newPod);
    }

    public void ListItemDatabase()
    {
        foreach (KeyValuePair<string, EnemyPod> kvp in enemyPods)
        {
            Debug.Log(kvp);
        }
    }
}
