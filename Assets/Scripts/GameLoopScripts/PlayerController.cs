using JetBrains.Annotations;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

/// 
/// MOUSE INPUT CONTROLS
/// 
public class PlayerController : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public TileMap tileMap;

    public UnitManager unitManager;

    public GameObject mouseOver;
    public GameObject selectedUnit;

    private Unit unit;
    private Tile tile;

    private List<Node> inRange;

    private List<Unit> toMoveQ;
    private List<Tile> toMoveTo;

    private bool isMoving;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    private void Start()
    {
        toMoveQ = new List<Unit>();
        toMoveTo = new List<Tile>();
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject hitObject = hitInfo.transform.parent.gameObject;
            if (hitObject == GameObject.Find("Map"))
            {
                hitObject = hitInfo.transform.gameObject;
            }
            mouseOver = hitObject;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (mouseOver.transform.parent.gameObject == GameObject.Find("PlayerUnits"))
            {
                selectedUnit = mouseOver;
                unit = selectedUnit.GetComponent<Unit>();
                
                if (!unit.isMoving && unit.actionPoints > 1)
                {
                    inRange = tileMap.TileRange(unit);
                }
                else
                {
                    selectedUnit = null;
                }
            }
            else
            {
                selectedUnit = null;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (selectedUnit)
            {
                if (mouseOver.transform.parent.gameObject == GameObject.Find("Map"))
                {
                    tile = mouseOver.GetComponent<Tile>();
                    unit = selectedUnit.GetComponent<Unit>();
                    
                    bool contains = false;
                   
                    for (int i = 0; i < inRange.Count; i++)
                    {
                        if (inRange[i].location == tile.tileLocation)
                            contains = true;
                    }
                    
                    if (contains && !unit.isMoving)
                    {
                        tileMap.UpdatePath(tile.tileLocation, unit);
                        
                        toMoveQ.Add(unit);
                        toMoveTo.Add(tile);

                        inRange = null;
                        selectedUnit = null;

                        tileMap.graph[(int)unit.unitPosition.x, (int)unit.unitPosition.y, (int)unit.unitPosition.z].isWalkable = true;
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

                tileMap.graph[tile.tileLocation.x, tile.tileLocation.y, tile.tileLocation.z].isWalkable = false;
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
    }
}