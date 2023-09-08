using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// 
/// MOUSE INPUT CONTROLS
/// 
public class MouseController : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public TileMap tileMap;

    public UnitManager unitManager;

    public GameObject mouseOver;
    public GameObject selectedUnit;

    private Unit unit;
    private Tile tile;
    private List<Node> inRange;
    bool isMoving = false;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

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
            if (mouseOver == GameObject.Find("playerUnit(Clone)"))
            {
                selectedUnit = mouseOver;
                inRange = tileMap.TileRange(selectedUnit.GetComponent<Unit>());
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
                    if (contains)
                    {
                        tileMap.UpdatePath(tile.tileLocation, unit); 
                        isMoving = true;
                        inRange = null;
                        selectedUnit = null;
                    }
                }
            }
        }

        if (isMoving)
        {
            if (unit.unitPosition == tile.tileLocation)
            {
                unit.currentPath = null;
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
