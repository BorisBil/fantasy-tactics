using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MouseController : MonoBehaviour
{
    public GameObject cursor;
    
    public float speed;
    private CharacterInfo character;
    public GameObject characterPrefab;
    private bool isMoving;

    private Pathfinder pathFinder;
    private Rangefinder rangeFinder;
    private List<OverlayTile> path;
    private List<OverlayTile> range;

    private void Start()
    {
        pathFinder = new Pathfinder();
        rangeFinder = new Rangefinder();
        
        path = new List<OverlayTile>();
        range = new List<OverlayTile>();

        isMoving = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
            
            cursor.transform.position = tile.transform.position;
            cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;

            if (Input.GetMouseButtonDown(0))
            {
                if (character == null)
                {
                    character = Instantiate(characterPrefab).GetComponent<CharacterInfo>();
                    character.standingOnTile = tile;
                    PositionCharacterOnLine(tile);
                } 
                else
                {
                    GetInRangeTiles();
                    if (range.Contains(tile) && !isMoving)
                    {
                        path = pathFinder.FindPath(character.standingOnTile, tile);
                        isMoving = true;
                        tile.gameObject.GetComponent<OverlayTile>().HideTile();
                        if (isMoving)
                        {
                            foreach (var item in range)
                            {
                                item.HideTile();
                            }
                        }
                    }
                }
            }
        }

        if (path.Count > 0 && isMoving)
        {
            MoveAlongPath();
        }
        if (path.Count == 0 && character != null)
        {
            GetInRangeTiles();
            isMoving = false;
        }
    }

    private void GetInRangeTiles()
    {
        range = rangeFinder.GetTilesInRange(character.standingOnTile, 3);

        foreach (var item in range)
        {
            item.ShowTile();
        }
    }

    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        float zIndex = path[0].transform.position.z;
        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
        character.transform.position = new Vector3(character.transform.position.x, character.transform.position.y, zIndex);

        if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(path[0]);
            path.RemoveAt(0);
        }
    }

    private static RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }

    private void PositionCharacterOnLine(OverlayTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y, tile.transform.position.z);
        character.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
        character.standingOnTile = tile;
    }
}
