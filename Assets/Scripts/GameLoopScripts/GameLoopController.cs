using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

/// 
/// GAME LOOP FILE
/// 

public class GameLoopController : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public PlayerController playerController;
    public UnitManager unitManager;
    public AIBehaviors behaviors;
    public VisionManager visionManager;
    
    public EndTurn endTurnButton;
    public Attack attackButton;

    float chancetohit;

    public List<Unit> visibleUnits;
    public List<Unit> unitsToRemove;
    public List<Unit> enemyVisibleUnits;

    public List<GameObject> spawnedlights;

    public bool AITurn;

    public GameObject map;
    public GameObject tilelights;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /// Ends the player's turn, sends the loop down the AI turn pipeline
    public void EndPlayerTurn()
    {
        playerController.transitionTurn = true;
        attackButton.HideButton();
        behaviors.AITurn();
    }

    /// Start the player's turn, uses a timer to give some processing time to the player
    public void StartPlayerTurn()
    {
        behaviors.isAITurn = false;

        timer();

        foreach (Unit unit in unitManager.playerUnits)
        {
            unit.actionPoints = 2;

            ListPlayerAttackSelectable(unit);
        }

        playerController.transitionTurn = false;
    }

    public void InstantiatePlayerVision()
    {
        visionManager.InstantiateVision();
    }

    public void UpdatePlayerVision(Unit unit)
    {
        visionManager.UpdateVision(unit);
    }

    /// List attackable units for the player's units
    public void ListPlayerAttackSelectable(Unit unit)
    {
        unit.attackableUnits.Clear();

        if (unit.actionPoints - unit.weapon.actionCost >= 0)
        {
            foreach (Unit enemy in unit.visibleUnits)
            {
                float distance = DistanceBetweenUnits(unit, enemy);
                if ((int)enemy.unitPosition.z - (int)unit.unitPosition.z == 1)
                {
                    distance = distance - 1;
                }

                if (distance <= unit.attackRange)
                {
                    Vector3 rayCastUnitCoords = new Vector3(unit.unitPosition.x, unit.unitPosition.y, unit.unitPosition.z + 0.50f);
                    Vector3 rayCastEnemyCoords = new Vector3(enemy.unitPosition.x, enemy.unitPosition.y, enemy.unitPosition.z + 0.50f);

                    Ray ray = new Ray(rayCastUnitCoords, rayCastEnemyCoords);
                    RaycastHit hitInfo;

                    if (!Physics.Raycast(ray, out hitInfo, distance))
                    {
                        unit.attackableUnits.Add(enemy);
                    }
                }
            }
        }
    }

    /// List attackable units for the enemy's units
    public void ListEnemyAttackSelectable(Unit unit)
    {
        unit.attackableUnits.Clear();

        if (unit.actionPoints - unit.weapon.actionCost >= 0)
        {
            foreach (Unit enemy in unit.visibleUnits)
            {
                float distance = DistanceBetweenUnits(unit, enemy);
                if ((int)enemy.unitPosition.z - (int)unit.unitPosition.z == 1)
                {
                    distance = distance - 1;
                }

                if (distance <= unit.attackRange + 0.75f)
                {
                    Vector3 rayCastUnitCoords = new Vector3(unit.unitPosition.x, unit.unitPosition.y, unit.unitPosition.z + 0.50f);
                    Vector3 rayCastEnemyCoords = new Vector3(enemy.unitPosition.x, enemy.unitPosition.y, enemy.unitPosition.z + 0.50f);

                    Ray ray = new Ray(rayCastUnitCoords, rayCastEnemyCoords);
                    RaycastHit hitInfo;

                    if (!Physics.Raycast(ray, out hitInfo, distance))
                    {
                        unit.attackableUnits.Add(enemy);
                    }

                    if (Physics.Raycast(ray, out hitInfo, distance))
                    {
                        if (hitInfo.transform.parent == GameObject.Find("Prop") || hitInfo.transform.parent == GameObject.Find("PlayerUnits"))
                        {
                            unit.attackableUnits.Add(enemy);
                        }
                    }
                }
            }
        }
    }

    /// List visible units for the player's units
    public void ListPlayerVisibleUnits(Unit unit)
    {
        unit.visibleUnits.Clear();

        foreach (Unit enemy in unitManager.enemyUnits)
        {
            foreach (Node node in unit.visibleTiles)
            {
                if (enemy.unitPosition == node.location)
                {
                    unit.visibleUnits.Add(enemy);
                }
            }
        }
    }

    public void UpdatePlayerVisibleUnits()
    {
        visibleUnits.Clear();
        unitsToRemove.Clear();
        
        foreach (Unit unit in unitManager.playerUnits)
        {
            foreach (Unit enemy in unit.visibleUnits)
            {
                if (!visibleUnits.Contains(enemy))
                {
                    visibleUnits.Add(enemy);
                }
            }
        }

        foreach (Unit unit in unitManager.enemyUnits)
        {
            if (!visibleUnits.Contains(unit))
            {
                unitsToRemove.Add(unit);
            }
        }

        foreach (Unit unit in visibleUnits)
        {
            if (unit.unitObject.GetComponentInChildren<MeshRenderer>().enabled == false)
            {
                unit.unitObject.GetComponentInChildren<MeshRenderer>().enabled = true;
            }
        }

        foreach (Unit unit in unitsToRemove)
        {
            if (unit.unitObject.GetComponentInChildren<MeshRenderer>().enabled == true)
            {
                unit.unitObject.GetComponentInChildren<MeshRenderer>().enabled = false;
            }
        }
    }

    public void UpdateEnemyVisibleUnits()
    {
        visibleUnits.Clear();
        unitsToRemove.Clear();

        foreach (Unit unit in unitManager.enemyUnits)
        {
            foreach (Unit enemy in unit.visibleUnits)
            {
                if (!visibleUnits.Contains(enemy))
                {
                    visibleUnits.Add(enemy);
                }
            }
        }

        enemyVisibleUnits = visibleUnits;
    }

    /// List visible units for the enemy's units
    public void ListEnemyVisibleUnits(Unit unit)
    {
        unit.visibleUnits.Clear();

        foreach (Unit enemy in unitManager.playerUnits)
        {
            float distance = DistanceBetweenUnits(unit, enemy);

            if (distance <= unit.visionRadius)
            {
                Vector3 rayCastUnitCoords = new Vector3(unit.unitPosition.x, unit.unitPosition.y, unit.unitPosition.z +1f);
                Vector3 rayCastEnemyCoords = new Vector3(enemy.unitPosition.x, enemy.unitPosition.y, enemy.unitPosition.z + 1f);

                Ray ray = new Ray(rayCastUnitCoords, rayCastEnemyCoords);
                RaycastHit hitInfo;

                if (!Physics.Raycast(ray, out hitInfo, distance) || (hitInfo.transform.parent == GameObject.Find("Prop")))
                {
                    unit.visibleUnits.Add(enemy);
                }
            }
        }
    }

    /// Attack calculations
    public void CalculateAttack(Unit unit, Unit enemy)
    {
        unit.actionPoints = 0;

        chancetohit = unit.weapon.accuracy;

        float hitRoll = Random.Range(0.0f, 1.0f);
        Debug.Log(chancetohit);
        Debug.Log(hitRoll);

        if (hitRoll <= chancetohit)
        {
            int damage = Mathf.RoundToInt(Random.Range(unit.weapon.minDamage, unit.weapon.maxDamage));
            Debug.Log("Hit!");
            Debug.Log(damage);
            enemy.totalHP = enemy.totalHP - (damage - enemy.armorAP);
        }

        if (hitRoll > chancetohit)
        {
            Debug.Log("Miss!");
        }

        CheckIfDead(enemy);
    }

    /// Check if the unit is dead
    public void CheckIfDead(Unit unit)
    {
        if (unit.totalHP <= 0)
        {
            unitManager.UnitDeath(unit);
        }
    }

    /// Timer for turn swap
    IEnumerator timer()
    {
        yield return new WaitForSecondsRealtime(3);
        StartPlayerTurn();
    }

    /// Calculate Manhattan distance between units
    public float DistanceBetweenUnits(Unit sender, Unit reciever)
    {
        return Mathf.Abs(sender.unitPosition.x - reciever.unitPosition.x) +
               Mathf.Abs(sender.unitPosition.y - reciever.unitPosition.y) +
               Mathf.Abs(sender.unitPosition.z - reciever.unitPosition.z);
    }

    public void SetUpLists()
    {
        List<Unit> visibleUnits = new List<Unit>();
        List<Unit> unitsToRemove = new List<Unit>();

        List<Unit> enemyVisibleUnits = new List<Unit>();
    }
}
