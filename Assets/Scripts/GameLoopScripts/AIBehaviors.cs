using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class AIBehaviors : MonoBehaviour
{
    public UnitManager unitManager;
    public TileMap tileMap;
    public GameLoopController gameLoopController;
    public EnemyManager enemyManager;

    private List<Unit> toMoveQAI;
    private List<Node> toMoveToAI;

    public List<Unit> movedUnits;

    private Unit unit;
    private Node node;

    private bool isMovingAI;
    public bool isAITurn;

    private void Start()
    {
        toMoveQAI = new List<Unit>();
        toMoveToAI = new List<Node>();
    }

    private void Update()
    {
        if (isAITurn)
        {
            if (toMoveQAI.Count > 0)
            {
                isMovingAI = true;
            }
            
            if (toMoveQAI.Count == 0)
            {
                isMovingAI = false;
            }

            if (isMovingAI)
            {
                unit = toMoveQAI[0];
                node = toMoveToAI[0];

                unit.isMoving = true;

                if (unit.unitPosition == node.location)
                {
                    unit.currentPath = null;
                    unit.isMoving = false;
                    unit.actionPoints -= 1;

                    toMoveQAI.RemoveAt(0);
                    toMoveToAI.RemoveAt(0);

                    if (unit.status == "Awake")
                    {
                        if (unit.actionPoints >= unit.weapon.actionCost)
                        {
                            gameLoopController.ListEnemyAttackSelectable(unit);

                            if (unit.attackableUnits.Count > 0)
                            {
                                gameLoopController.CalculateAttack(unit, unit.attackableUnits[0]);
                            }
                        }
                    }
                    
                    if (unit.status == "Awoken")
                    {
                        unit.status = "Awake";
                    }

                    movedUnits.Add(unit);

                    if (movedUnits.Count == unitManager.enemyUnits.Count)
                    {
                        isMovingAI = false;
                        gameLoopController.StartPlayerTurn();
                    }
                }

                if (unit.currentPath != null)
                {
                    var step = unit.unitSpeed * Time.deltaTime;
                    unit.transform.position = Vector3.MoveTowards(unit.transform.position, unit.currentPath[0].location, step);

                    if (unit.transform.position == unit.currentPath[0].location)
                    {
                        unit.unitPosition = unit.currentPath[0].location;
                        unit.transform.position = unit.unitPosition;
                        unit.currentPath.RemoveAt(0);
                    }
                }
            }

            if (movedUnits.Count == unitManager.enemyUnits.Count)
            {
                isMovingAI = false;
                gameLoopController.StartPlayerTurn();
            }
        }
    }

    public void AITurn()
    {
        movedUnits.Clear();

        isAITurn = true;  

        foreach (SpawnedPod spawnedPod in enemyManager.spawnedPodList)
        {
            if (spawnedPod.status == "Awake")
            {
                foreach (Unit unit in spawnedPod.unitsInPod)
                {
                    unit.actionPoints = 2;
                    
                    if (unit.unitType == "Fighter")
                    {
                        FighterBehavior(unit);
                    }

                    if (unit.unitType == "Archer")
                    {
                        ArcherBehavior(unit);
                    }

                    if (unit.unitType == "Zombie")
                    {
                        ZombieBehavior(unit);
                    }
                }
                
            }
            
            if (spawnedPod.status == "Asleep")
            {
                foreach (Unit unit in spawnedPod.unitsInPod)
                {
                    gameLoopController.ListEnemyVisibleUnits(unit);

                    if (unit.visibleUnits.Count > 0)
                    {
                        spawnedPod.status = "Awoken";
                    }

                    else
                    {
                        movedUnits.Add(unit);
                    }
                }
            }
        }

        foreach (SpawnedPod spawnedPod in enemyManager.spawnedPodList)
        {
            if (spawnedPod.status == "Awoken")
            {
                foreach (Unit unit in spawnedPod.unitsInPod)
                {
                    if (unit.unitType == "Zombie")
                    {
                        ZombieAwoken(unit);
                    }
                }

                spawnedPod.status = "Awake";
            }
        }
    }

    public void FighterBehavior(Unit unit)
    {
        List<Node> movementRange = tileMap.TileRange(unit);
    }

    public void ArcherBehavior(Unit unit)
    {

    }

    public void ZombieBehavior(Unit unit)
    {
        List<Node> movementRange = tileMap.TileRange(unit);
        
        Node closestTargetNode = null;
        Node finalTargetNode = null;

        List<Node> checkList = new List<Node>();

        Node unitNode = tileMap.graph[(int)unit.unitPosition.x, (int)unit.unitPosition.y, (int)unit.unitPosition.z];

        gameLoopController.ListEnemyAttackSelectable(unit);

        if (unit.attackableUnits.Count > 0)
        {
            gameLoopController.CalculateAttack(unit, unit.attackableUnits[0]);
            movedUnits.Add(unit);

            if (movedUnits.Count == unitManager.enemyUnits.Count)
            {
                isMovingAI = false;
                gameLoopController.StartPlayerTurn();
            }
        }
        else
        {
            List<Node> targetMoveList = new List<Node>();
            
            foreach (Unit playerunit in unitManager.playerUnits)
            {
                Node playerUnitNode = tileMap.graph[(int)playerunit.unitPosition.x, (int)playerunit.unitPosition.y, (int)playerunit.unitPosition.z];

                checkList = tileMap.GeneratePathTo(playerUnitNode.location, unitNode.location);

                if (checkList.Count < targetMoveList.Count || targetMoveList.Count == 0)
                {
                    targetMoveList = checkList;
                    closestTargetNode = playerUnitNode;
                }
            }

            List<Node> finalMoveList = new List<Node>();

            foreach (Node neighbor in closestTargetNode.neighbors)
            {
                if (neighbor.isWalkable)
                {
                    if (movementRange.Contains(neighbor))
                    {
                        finalTargetNode = neighbor;
                        finalMoveList = tileMap.GeneratePathTo(finalTargetNode.location, unitNode.location);

                        break;
                    }
                    else
                    {
                        checkList = tileMap.GeneratePathTo(neighbor.location, unitNode.location);

                        if (checkList.Count < finalMoveList.Count || finalMoveList.Count == 0)
                        {
                            finalTargetNode = neighbor;
                            finalMoveList = checkList;
                        }
                    }
                }
            }

            checkList.Clear();

            if (finalMoveList.Count > unit.movementRange)
            {
                for (int i = 0; i < unit.movementRange; i++)
                {
                    checkList.Add(finalMoveList[i]);
                }

                finalMoveList = checkList;
            }
            finalTargetNode = finalMoveList[finalMoveList.Count - 1];

            toMoveQAI.Add(unit);
            toMoveToAI.Add(finalTargetNode);

            unit.currentPath = finalMoveList;

            tileMap.graph[finalTargetNode.location.x, finalTargetNode.location.y, finalTargetNode.location.z].isWalkable = false;
            tileMap.graph[(int)unit.unitPosition.x, (int)unit.unitPosition.y, (int)unit.unitPosition.z].isWalkable = true;
        }
    }

    public void ZombieAwoken(Unit unit)
    {
        List<Node> movementRange = tileMap.TileRange(unit);

        Node closestTargetNode = null;
        Node finalTargetNode = null;

        List<Node> checkList = new List<Node>();

        Node unitNode = tileMap.graph[(int)unit.unitPosition.x, (int)unit.unitPosition.y, (int)unit.unitPosition.z];
        List<Node> targetMoveList = new List<Node>();

        foreach (Unit playerunit in unitManager.playerUnits)
        {
            Node playerUnitNode = tileMap.graph[(int)playerunit.unitPosition.x, (int)playerunit.unitPosition.y, (int)playerunit.unitPosition.z];

            checkList = tileMap.GeneratePathTo(playerUnitNode.location, unitNode.location);

            if (checkList.Count < targetMoveList.Count || targetMoveList.Count == 0)
            {
                targetMoveList = checkList;
                closestTargetNode = playerUnitNode;
            }
        }

        List<Node> finalMoveList = new List<Node>();

        foreach (Node neighbor in closestTargetNode.neighbors)
        {
            if (neighbor.isWalkable)
            {
                if (movementRange.Contains(neighbor))
                {
                    finalTargetNode = neighbor;
                    finalMoveList = tileMap.GeneratePathTo(finalTargetNode.location, unitNode.location);

                    break;
                }
                else
                {
                    checkList = tileMap.GeneratePathTo(neighbor.location, unitNode.location);

                    if (checkList.Count < finalMoveList.Count || finalMoveList.Count == 0)
                    {
                        finalTargetNode = neighbor;
                        finalMoveList = checkList;
                    }
                }
            }
        }

        checkList.Clear();

        if (finalMoveList.Count > unit.movementRange)
        {
            for (int i = 0; i < unit.movementRange; i++)
            {
                checkList.Add(finalMoveList[i]);
            }

            finalMoveList = checkList;
        }
        finalTargetNode = finalMoveList[finalMoveList.Count - 1];

        toMoveQAI.Add(unit);
        toMoveToAI.Add(finalTargetNode);

        unit.currentPath = finalMoveList;

        tileMap.graph[finalTargetNode.location.x, finalTargetNode.location.y, finalTargetNode.location.z].isWalkable = false;
        tileMap.graph[(int)unit.unitPosition.x, (int)unit.unitPosition.y, (int)unit.unitPosition.z].isWalkable = true;

        unit.status = "Awoken";
    }
}
