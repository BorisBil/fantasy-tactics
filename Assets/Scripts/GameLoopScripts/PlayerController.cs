using System.Collections.Generic;
using UnityEngine;

/// 
/// PLAYER'S VIEW OF THE GAME
/// 
public class PlayerController : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public TileMap tileMap;

    public UnitManager unitManager;
    public GameLoopController gameLoopController;

    public GameObject mouseOver;
    public GameObject selectedUnit;
    public GameObject selectedEnemy;

    private Unit unit;
    private Tile tile;

    private List<Node> inRangeTiles;

    private List<Unit> toMoveQ;
    private List<Tile> toMoveTo;

    private bool attackMode;
    private bool moveMode;

    public bool isMoving;
    public bool transitionTurn;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    private void Start()
    {
        toMoveQ = new List<Unit>();
        toMoveTo = new List<Tile>();

        SetMoveMode();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject hitObject = hitInfo.transform.parent.gameObject;
            
            if (hitObject == GameObject.Find("Tiles"))
            {
                hitObject = hitInfo.transform.gameObject;
            }
            
            mouseOver = hitObject;
        }

        if (moveMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (mouseOver.transform.parent.gameObject == GameObject.Find("PlayerUnits"))
                {
                    selectedUnit = mouseOver;
                    unit = selectedUnit.GetComponent<Unit>();

                    if (unit.attackableUnits.Count > 0 && unit.actionPoints >= 0)
                    {
                        gameLoopController.attackButton.ShowButton();
                    }
                    else
                    {
                        gameLoopController.attackButton.HideButton();
                    }

                    if (!unit.isMoving && unit.actionPoints > 1)
                    {
                        inRangeTiles = tileMap.TileRange(unit);

                        gameLoopController.ListPlayerAttackSelectable(unit);

                        if (unit.attackableUnits.Count > 0)
                        {
                            gameLoopController.attackButton.ShowButton();
                        }
                        else
                        {
                            gameLoopController.attackButton.HideButton();
                        }
                    }
                    else if (unit.actionPoints <= 0)
                    {
                        selectedUnit = null;
                        gameLoopController.attackButton.HideButton();
                    }
                }
                else if (!mouseOver.transform.parent.gameObject == GameObject.Find("Canvas"))
                {
                    selectedUnit = null;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (selectedUnit && selectedUnit.GetComponent<Unit>().actionPoints > 1 && !selectedUnit.GetComponent<Unit>().isMoving)
                {
                    if (mouseOver.transform.parent.gameObject == GameObject.Find("Tiles"))
                    {
                        tile = mouseOver.GetComponent<Tile>();
                        unit = selectedUnit.GetComponent<Unit>();

                        if (tileMap.graph[tile.tileLocation.x, tile.tileLocation.y, tile.tileLocation.z].isWalkable)
                        {
                            bool contains = false;

                            for (int i = 0; i < inRangeTiles.Count; i++)
                            {
                                if (inRangeTiles[i].location == tile.tileLocation)
                                    contains = true;
                            }

                            if (contains && !unit.isMoving)
                            {
                                tileMap.UpdatePath(tile.tileLocation, unit);

                                toMoveQ.Add(unit);
                                toMoveTo.Add(tile);

                                inRangeTiles = null;

                                tileMap.graph[(int)unit.unitPosition.x, (int)unit.unitPosition.y, (int)unit.unitPosition.z].isWalkable = true;
                                tileMap.graph[tile.tileLocation.x, tile.tileLocation.y, tile.tileLocation.z].isWalkable = false;

                            }
                        }
                    }
                }
            }

            if (toMoveQ.Count > 0)
            {
                isMoving = true;
            }
            else if (toMoveQ.Count == 0)
            {
                isMoving = false;
            }

            if (isMoving)
            {
                gameLoopController.endTurnButton.HideButton();

                unit = toMoveQ[0];
                tile = toMoveTo[0];

                unit.isMoving = true;

                if (unit.unitPosition == tile.tileLocation)
                {
                    unit.currentPath = null;
                    unit.isMoving = false;
                    unit.actionPoints -= 1;

                    toMoveQ.RemoveAt(0);
                    toMoveTo.RemoveAt(0);

                    gameLoopController.ListPlayerAttackSelectable(unit);

                    if (selectedUnit != null && selectedUnit.GetComponent<Unit>() == unit)
                    { 
                        if (unit.attackableUnits.Count > 0)
                        {
                            gameLoopController.attackButton.ShowButton();
                        }
                        else
                        {
                            gameLoopController.attackButton.HideButton();
                        }
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
            else if (!isMoving && !transitionTurn)
            {
                gameLoopController.endTurnButton.ShowButton();
            }
        }

        if (attackMode)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (mouseOver.transform.parent.gameObject == GameObject.Find("EnemyUnits"))
                {
                    selectedEnemy = mouseOver;
                    Unit enemy = selectedEnemy.GetComponent<Unit>();
                    
                    if (unit.attackableUnits.Contains(enemy))
                    {
                        Unit unit = selectedUnit.GetComponent<Unit>();
                        gameLoopController.CalculateAttack(unit, enemy);
                        
                        selectedUnit = null;
                        selectedEnemy = null;

                        gameLoopController.attackButton.HideButton();
                        SetMoveMode();
                    }
                }
                else
                {
                    SetMoveMode();
                    selectedUnit = null;
                }
            }
        }
    }

    public void SetMoveMode()
    {
        attackMode = false;
        moveMode = true;
    }

    public void SetAttackMode()
    {
        attackMode = true;
        moveMode = false;
    }
}