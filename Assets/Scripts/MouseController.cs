using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
                    Tile tile = mouseOver.GetComponent<Tile>();
                    Debug.Log(tile.tileLocation);
                    Unit unit = selectedUnit.GetComponent<Unit>();
                    Debug.Log(unit.unitPosition);
                    tileMap.MoveSelectedUnitTo(tile.tileLocation, unit);
                }
            }
        }
    }
}
