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
    public RotateCamera rotateCameraButton;

    float chancetohit;

    public List<Unit> visibleUnits;
    public List<Unit> unitsToRemove;
    public List<Unit> enemyVisibleUnits;

    public bool AITurn;

    public GameObject map;
    public GameObject tilelights;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /// Ends the player's turn, sends the loop down the AI turn pipeline
    public void EndPlayerTurn()
    {
        playerController.transitionTurn = true;
        attackButton.HideButton();
        rotateCameraButton.HideButton();
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

    public void UpdateUnitCover(Unit unit, Tile tile)
    {
        if (tile == null)
        {
            Dictionary<string, string> emptyCover = new Dictionary<string, string>();

            emptyCover["North"] = "None";
            emptyCover["South"] = "None";
            emptyCover["East"] = "None";
            emptyCover["West"] = "None";

            foreach (KeyValuePair<string, string> coverDirections in emptyCover)
            {
                unit.inCover[coverDirections.Key] = "None";
            }
        }

        if (tile != null)
        {
            foreach (KeyValuePair<string, string> coverDirections in tile.coverOnTile)
            {
                unit.inCover[coverDirections.Key] = coverDirections.Value;
            }

            if (!tile.coverOnTile.ContainsKey("North"))
            {
                unit.inCover["North"] = "None";
            }
            if (!tile.coverOnTile.ContainsKey("South"))
            {
                unit.inCover["South"] = "None";
            }
            if (!tile.coverOnTile.ContainsKey("East"))
            {
                unit.inCover["East"] = "None";
            }
            if (!tile.coverOnTile.ContainsKey("West"))
            {
                unit.inCover["West"] = "None";
            }
        }
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
                Debug.Log(enemy.id);
                float distance = DistanceBetweenUnits(unit, enemy);
                Debug.Log(distance);
                if (enemy.unitPosition.z - unit.unitPosition.z == 1)
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

        List<string> selectedCovers = new List<string>();

        float xDifference = unit.unitPosition.x - enemy.unitPosition.x;
        float yDifference = unit.unitPosition.y - enemy.unitPosition.y;
        float zDifference = enemy.unitPosition.z - unit.unitPosition.z;

        float distance = DistanceBetweenUnits(unit, enemy);

        float xCover = 0;
        float yCover = 0;

        if (xDifference < 0)
        {
            selectedCovers.Add("East");
        }
        else if (xDifference >= 0)
        {
            selectedCovers.Add("West");
        }

        if (yDifference < 0)
        {
            selectedCovers.Add("South");
        }
        else if (yDifference >= 0)
        {
            selectedCovers.Add("North");
        }

        foreach (string cover in selectedCovers)
        {
            string coverType = enemy.inCover[cover];

            if (cover == "East" || cover == "West")
            {
                if (coverType == "None")
                {
                    xCover = 1;
                }
                else if (coverType == "Half")
                {
                    xCover = 0.75f;
                }
                else if (coverType == "Full")
                {
                    xCover = 0.5f;
                }
            }

            if (cover == "North" || cover == "South")
            {
                if (coverType == "None")
                {
                    yCover = 1;
                }
                else if (coverType == "Half")
                {
                    yCover = 0.75f;
                }
                else if (coverType == "Full")
                {
                    yCover = 0.5f;
                }
            }
        }

        foreach (string cover in selectedCovers)
        {
            Debug.Log(cover);
        }

        float weightx = ((Mathf.Abs(xDifference)) / (Mathf.Abs(xDifference) + Mathf.Abs(yDifference)));
        float weighty = ((Mathf.Abs(yDifference)) / (Mathf.Abs(xDifference) + Mathf.Abs(yDifference)));

        chancetohit = ((unit.weapon.accuracy + (zDifference / 10)) * ((weightx * xCover) + (weighty * yCover))) * Mathf.Cos(2*((distance - unit.weapon.range + 3) / 10));

        if (unit.weapon.weaponType == "Longsword" 
            || unit.weapon.weaponType == "Shortsword"
            || unit.weapon.weaponType == "Hands")
        {
            chancetohit = unit.weapon.accuracy;
        }

        if (chancetohit <= 0)
        {
            chancetohit = 0.3f;
        }

        if (chancetohit >= 1)
        {
            chancetohit = 1;
        }

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
