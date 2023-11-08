using System.Collections;
using UnityEngine;

/// 
/// GAME LOOP FILE
/// 

public class GameLoopController : MonoBehaviour
{
    public PlayerController playerController;
    public UnitManager unitManager;
    public AIBehaviors AIbehaviors;
    
    public EndTurn endTurnButton;
    public Attack attackButton;

    float chancetohit;

    public bool AITurn;

    public void EndPlayerTurn()
    {
        playerController.transitionTurn = true;
        attackButton.HideButton();
        AIbehaviors.AITurn();
    }

    public void StartPlayerTurn()
    {
        AIbehaviors.isAITurn = false;

        timer();

        foreach (Unit unit in unitManager.playerUnits)
        {
            unit.actionPoints = 2;

            ListPlayerAttackSelectable(unit);
        }

        playerController.transitionTurn = false;
    }

    public void ListPlayerAttackSelectable(Unit unit)
    {
        /// Want to modify this later to only select targets in vision range
        /// 
        unit.attackableUnits.Clear();

        if (unit.actionPoints - unit.weapon.actionCost >= 0)
        {
            foreach (Unit enemy in unit.visibleUnits)
            {
                float distance = DistanceBetweenUnits(unit, enemy);

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

    public void ListEnemyAttackSelectable(Unit unit)
    {
        /// Want to modify this later to only select targets in vision range
        /// 
        unit.attackableUnits.Clear();

        if (unit.actionPoints - unit.weapon.actionCost >= 0)
        {
            foreach (Unit enemy in unit.visibleUnits)
            {
                float distance = DistanceBetweenUnits(unit, enemy);

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

    public void ListPlayerVisibleUnits(Unit unit)
    {
        unit.visibleUnits.Clear();

        foreach (Unit enemy in unitManager.enemyUnits)
        {
            float distance = DistanceBetweenUnits(unit, enemy);

            if (distance <= unit.visionRadius)
            {
                Vector3 rayCastUnitCoords = new Vector3(unit.unitPosition.x, unit.unitPosition.y, unit.unitPosition.z + 1.00f);
                Vector3 rayCastEnemyCoords = new Vector3(enemy.unitPosition.x, enemy.unitPosition.y, enemy.unitPosition.z + 1.00f);

                Ray ray = new Ray(rayCastUnitCoords, rayCastEnemyCoords);
                RaycastHit hitInfo;

                if (!Physics.Raycast(ray, out hitInfo, distance) || (hitInfo.transform.parent == GameObject.Find("Prop")))
                {
                    unit.visibleUnits.Add(enemy);
                }
            }
        }
    }

    public void ListEnemyVisibleUnits(Unit unit)
    {
        unit.visibleUnits.Clear();

        foreach (Unit enemy in unitManager.playerUnits)
        {
            float distance = DistanceBetweenUnits(unit, enemy);

            if (distance <= unit.visionRadius)
            {
                Vector3 rayCastUnitCoords = new Vector3(unit.unitPosition.x, unit.unitPosition.y, unit.unitPosition.z + 0.50f);
                Vector3 rayCastEnemyCoords = new Vector3(enemy.unitPosition.x, enemy.unitPosition.y, enemy.unitPosition.z + 0.50f);

                Ray ray = new Ray(rayCastUnitCoords, rayCastEnemyCoords);
                RaycastHit hitInfo;

                if (!Physics.Raycast(ray, out hitInfo, distance) || (hitInfo.transform.parent == GameObject.Find("Prop")))
                {
                    unit.visibleUnits.Add(enemy);
                }
            }
        }
    }

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

    public void CheckIfDead(Unit unit)
    {
        if (unit.totalHP <= 0)
        {
            unitManager.UnitDeath(unit);
        }
    }

    IEnumerator timer()
    {
        yield return new WaitForSecondsRealtime(3);
        StartPlayerTurn();
    }

    public float DistanceBetweenUnits(Unit sender, Unit reciever)
    {
        return Mathf.Abs(sender.unitPosition.x - reciever.unitPosition.x) +
               Mathf.Abs(sender.unitPosition.y - reciever.unitPosition.y) +
               Mathf.Abs(sender.unitPosition.z - reciever.unitPosition.z);
    }

}
