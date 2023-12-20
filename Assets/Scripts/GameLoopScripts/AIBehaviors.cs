using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// 
/// DETERMINES THE BEHAVIORS OF THE AI, AND SETS UP THE AI'S TURN MUCH LIKE THE PLAYER'S VIEW
/// 

public class AIBehaviors : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
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
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

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

                    gameLoopController.ListEnemyVisibleUnits(unit);
                    gameLoopController.UpdateEnemyVisibleUnits();
                    gameLoopController.UpdatePlayerVisibleUnits();

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
                }

                if (unit.currentPath != null)
                {
                    var step = unit.unitSpeed * Time.deltaTime;
                    unit.transform.position = Vector3.MoveTowards(unit.transform.position, unit.currentPath[0].location, step);

                    if (unit.transform.position == unit.currentPath[0].location)
                    {
                        unit.unitPosition = unit.currentPath[0].location;
                        unit.transform.position = unit.unitPosition;
                        gameLoopController.UpdateEnemyVisibleUnits();
                        gameLoopController.UpdatePlayerVisibleUnits();
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

    /*
     * AI turn setup, determines if pods see player units, determining if they stay asleep or wake up
     */
    public void AITurn()
    {
        movedUnits.Clear();

        isAITurn = true;

        foreach (SpawnedPod asleepPod in enemyManager.asleepPods)
        {
            foreach (Unit unit in asleepPod.unitsInPod)
            {
                gameLoopController.ListEnemyVisibleUnits(unit);
                gameLoopController.UpdateEnemyVisibleUnits();

                if (unit.visibleUnits.Count > 0)
                {
                    asleepPod.status = "Awoken";
                }
            }
        }

        foreach (SpawnedPod awakePod in enemyManager.awakePods)
        {
            foreach (Unit unit in awakePod.unitsInPod)
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

        foreach (SpawnedPod spawnedPod in enemyManager.spawnedPodList)
        {
            if (spawnedPod.status == "Awoken")
            {
                enemyManager.asleepPods.Remove(spawnedPod);
                enemyManager.awakePods.Add(spawnedPod);

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

        foreach (SpawnedPod asleepPod in enemyManager.asleepPods)
        {
            foreach (Unit unit in asleepPod.unitsInPod)
            {
                movedUnits.Add(unit);
            }
        }
    }

    /*
     * ENEMY UNITS BEHAVIOR
     */
    public void FighterBehavior(Unit unit)
    {
        List<Node> movementRange = tileMap.TileRange(unit);
    }

    public void ArcherBehavior(Unit unit)
    {

    }

    public void ZombieBehavior(Unit unit)
    {
        gameLoopController.ListEnemyAttackSelectable(unit);
        gameLoopController.UpdateEnemyVisibleUnits();

        Debug.Log(unit.id);

        if (unit.attackableUnits.Count > 0)
        {
            gameLoopController.CalculateAttack(unit, unit.attackableUnits[0]);

            movedUnits.Add(unit);
            unit.actionPoints = 0;
        }
        else
        {
            List<List<Node>> generatedPaths = new List<List<Node>>();
            List<Node> inRangeTiles = tileMap.TileRange(unit);

            List<Node> finalPath = new List<Node>();
            Node finalNode = null;

            foreach (Unit enemy in unitManager.playerUnits)
            {
                Vector3Int enemyPosition = new Vector3Int((int)enemy.unitPosition.x, (int)enemy.unitPosition.y, (int)enemy.unitPosition.z);
                tileMap.graph[enemyPosition.x, enemyPosition.y, enemyPosition.z].isWalkable = true;

                List<Node> path = tileMap.GeneratePathTo(enemyPosition, unit.unitPosition);

                generatedPaths.Add(path);
                tileMap.graph[enemyPosition.x, enemyPosition.y, enemyPosition.z].isWalkable = false;
            }

            generatedPaths.Sort((a, b) => a.Count - b.Count);

            List<Node> closestTarget = generatedPaths[0];
            Node targetNode = closestTarget.LastOrDefault();

            generatedPaths.Clear();

            foreach (Node neighbor in targetNode.neighbors)
            {
                if (neighbor.isWalkable)
                {
                    if (inRangeTiles.Contains(neighbor))
                    {
                        finalPath = tileMap.GeneratePathTo(neighbor.location, unit.unitPosition);
                        finalNode = neighbor;

                        break;
                    }
                }
            }

            if (finalPath.Count == 0)
            {
                foreach (Node neighbor in targetNode.neighbors)
                {
                    if (neighbor.isWalkable)
                    {
                        List<Node> checkPath = tileMap.GeneratePathTo(neighbor.location, unit.unitPosition);

                        generatedPaths.Add(checkPath);
                    }
                }

                if (generatedPaths.Count == 0)
                {
                    foreach (Unit enemy in unitManager.playerUnits)
                    {
                        foreach (Node neighbor in tileMap.graph[(int)enemy.unitPosition.x, (int)enemy.unitPosition.y, (int)enemy.unitPosition.z].neighbors)
                        {
                            if (neighbor.isWalkable)
                            {
                                finalPath = tileMap.GeneratePathTo(neighbor.location, unit.unitPosition);
                                finalNode = finalPath.LastOrDefault();
                            }
                        }
                    }
                }
                else
                {
                    generatedPaths.Sort((a, b) => a.Count - b.Count);

                    List<Node> closestPath = generatedPaths[0];

                    int i = 0;

                    foreach (Node node in closestPath)
                    {
                        i = i + (int)node.movementCost;
                        finalPath.Add(node);

                        if (i >= unit.movementRange)
                        {
                            break;
                        }
                    }

                    finalNode = finalPath.LastOrDefault(); 
                }
            }

            toMoveQAI.Add(unit);
            toMoveToAI.Add(finalNode);

            unit.currentPath = finalPath;

            tileMap.graph[finalNode.location.x, finalNode.location.y, finalNode.location.z].isWalkable = false;
            tileMap.graph[(int)unit.unitPosition.x, (int)unit.unitPosition.y, (int)unit.unitPosition.z].isWalkable = true;
        }
    }

    /*
     * ENEMY UNITS AWAKENING
     */
    public void ZombieAwoken(Unit unit)
    {
        List<List<Node>> generatedPaths = new List<List<Node>>();
        List<Node> inRangeTiles = tileMap.TileRange(unit);

        List<Node> finalPath = new List<Node>();
        Node finalNode = null;

        Debug.Log(unit.id);

        foreach (Unit enemy in unitManager.playerUnits)
        {
            Vector3Int enemyPosition = new Vector3Int((int)enemy.unitPosition.x, (int)enemy.unitPosition.y, (int)enemy.unitPosition.z);
            tileMap.graph[enemyPosition.x, enemyPosition.y, enemyPosition.z].isWalkable = true;

            List<Node> path = tileMap.GeneratePathTo(enemyPosition, unit.unitPosition);

            generatedPaths.Add(path);
            tileMap.graph[enemyPosition.x, enemyPosition.y, enemyPosition.z].isWalkable = false;
        }

        generatedPaths.Sort((a, b) => a.Count - b.Count);

        List<Node> closestTarget = generatedPaths[0];
        Node targetNode = closestTarget.LastOrDefault();

        generatedPaths.Clear();

        foreach (Node neighbor in targetNode.neighbors)
        {
            if (neighbor.isWalkable)
            {
                if (inRangeTiles.Contains(neighbor))
                {
                    finalPath = tileMap.GeneratePathTo(neighbor.location, unit.unitPosition);
                    finalNode = neighbor;

                    break;
                }
            }
        }

        if (finalPath.Count == 0)
        {
            foreach (Node neighbor in targetNode.neighbors)
            {
                if (neighbor.isWalkable)
                {
                    List<Node> checkPath = tileMap.GeneratePathTo(neighbor.location, unit.unitPosition);

                    generatedPaths.Add(checkPath);
                }
            }

            if (generatedPaths.Count == 0)
            {
                foreach (Unit enemy in unitManager.playerUnits)
                {
                    foreach (Node neighbor in tileMap.graph[(int)enemy.unitPosition.x, (int)enemy.unitPosition.y, (int)enemy.unitPosition.z].neighbors)
                    {
                        if (neighbor.isWalkable)
                        {
                            finalPath = tileMap.GeneratePathTo(neighbor.location, unit.unitPosition);
                            finalNode = finalPath.LastOrDefault();
                        }
                    }
                }
            }
            else
            {
                generatedPaths.Sort((a, b) => a.Count - b.Count);

                List<Node> closestPath = generatedPaths[0];

                int i = 0;

                foreach (Node node in closestPath)
                {
                    i = i + (int)node.movementCost;
                    finalPath.Add(node);

                    if (i >= unit.movementRange)
                    {
                        break;
                    }
                }

                finalNode = finalPath.LastOrDefault();
            }
        }

        toMoveQAI.Add(unit);
        toMoveToAI.Add(finalNode);

        unit.currentPath = finalPath;

        tileMap.graph[finalNode.location.x, finalNode.location.y, finalNode.location.z].isWalkable = false;
        tileMap.graph[(int)unit.unitPosition.x, (int)unit.unitPosition.y, (int)unit.unitPosition.z].isWalkable = true;

        unit.status = "Awoken";
    }
}