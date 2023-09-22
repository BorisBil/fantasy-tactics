using System.Collections;
using UnityEngine;

/// 
/// GAME LOOP FILE
/// 

public class GameLoopController : MonoBehaviour
{
    public PlayerController playerController;
    public UnitManager unitManager;
    
    public EndTurn endTurnButton;
    public Attack attackButton;

    float chancetohit;

    public void EndPlayerTurn()
    {
        /// Clean up, lead into AI Behavior
        /// 
        StartCoroutine(timer());
    }

    public void StartPlayerTurn()
    {
        foreach (Unit unit in unitManager.playerUnits)
        {
            unit.actionPoints = 2;

            ListAttackSelectable(unit);
        }

        endTurnButton.ShowButton();
    }

    public void ListAttackSelectable(Unit unit)
    {
        /// Want to modify this later to only select targets in vision range
        /// 
        unit.attackableUnits.Clear();

        if (unit.actionPoints - unit.weapon.actionCost >= 0)
        {
            foreach (Unit enemy in unitManager.enemyUnits)
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
                        Debug.Log("Can attack");
                        Debug.Log(enemy.unitPosition);
                        unit.attackableUnits.Add(enemy);
                    }
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
    }

    IEnumerator timer()
    {
        yield return new WaitForSecondsRealtime(3);
        StartPlayerTurn();
    }

    private float DistanceBetweenUnits(Unit sender, Unit reciever)
    {
        return Mathf.Abs(sender.unitPosition.x - reciever.unitPosition.x) +
               Mathf.Abs(sender.unitPosition.y - reciever.unitPosition.y) +
               Mathf.Abs(sender.unitPosition.z - reciever.unitPosition.z);
    }

}
