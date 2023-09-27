using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehaviors : MonoBehaviour
{
    public UnitManager unitManager;
    public TileMap tileMap;

    public void AITurn()
    {
        foreach (Unit unit in unitManager.enemyUnits)
        {
            if (unit.unitType == "Fighter")
            {
                FighterBehavior(unit);
            }
            if (unit.unitType == "Archer")
            {
                ArcherBehavior(unit);
            }
        }
    }

    public void FighterBehavior(Unit unit)
    {
        List<Node> movementRange = tileMap.TileRange(unit);

        foreach (Node node in movementRange)
        {

        }
    }

    public void ArcherBehavior(Unit unit)
    {
        
    }
}
