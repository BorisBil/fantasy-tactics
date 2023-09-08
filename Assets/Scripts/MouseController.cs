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
            Debug.Log("WAT");
            if (mouseOver == GameObject.Find("playerUnit(Clone)"))
            {
                selectedUnit = mouseOver;
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
                Debug.Log("Selected?");
                if (mouseOver.transform.parent.gameObject == GameObject.Find("Map"))
                {
                    tile = mouseOver.GetComponent<Tile>();
                    unit = selectedUnit.GetComponent<Unit>();

                    tileMap.UpdatePath(tile.tileLocation, unit);
                    
                    isMoving = true;
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
