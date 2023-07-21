using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TileMap : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public TileType[] tileTypes;
    public GameObject selectedUnit;

    Node[,,] graph;

    int[,,] tileTypeMap;
    int mapSizeX, mapSizeY, mapSizeZ;

    public List<Vector3Int> tileMapList;
    public List<Vector3Int> graphNodeList;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /// 
    /// Void Start Unity method that launches on startup
    /// 
    private void Start()
    {
        selectedUnit.GetComponent<Unit>().tileMap = this;

        GenerateMapData();
        GenerateMapVisual();
        GenerateMapGraph();
    }

    /* MAP DATA GENERATION FUNCTION
     * This function creates and stores map data into several arrays and lists for game map usage
     */
    void GenerateMapData()
    {
        mapSizeX = 10; mapSizeY = 10; mapSizeZ = 5;

        tileTypeMap = new int[mapSizeX, mapSizeY, mapSizeZ];
        tileMapList = new List<Vector3Int>();

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                tileTypeMap[x, y, 0] = 0;
                Vector3Int tileLocation = new Vector3Int(x, y, 0);
                tileMapList.Add(tileLocation);
            }
        }
        
        tileTypeMap[4, 4, 0] = 2;
        tileTypeMap[5, 4, 0] = 2;
        tileTypeMap[6, 4, 0] = 2;
        tileTypeMap[7, 4, 0] = 2;
        tileTypeMap[8, 4, 0] = 2;

        tileTypeMap[4, 5, 0] = 2;
        tileTypeMap[4, 6, 0] = 2;
        tileTypeMap[8, 5, 0] = 2;
        tileTypeMap[8, 6, 0] = 2;

        for (int x = 3; x < 6; x++)
        {
            for (int y = 3; y < 6; y++)
            {
                tileTypeMap[x, y, 1] = 0;
                Vector3Int tileLocation = new Vector3Int(x, y, 1);
                tileMapList.Add(tileLocation);
            }
        }
        for (int x = 6; x < 9; x++)
        {
            for (int y = 3; y < 6; y++)
            {
                tileTypeMap[x, y, 1] = 3;
                Vector3Int tileLocation = new Vector3Int(x, y, 1);
                tileMapList.Add(tileLocation);
            }
        }
    }
    
    /* GENERATE THE MAP
     * This function is responsible for generating the actual map visuals itself
     */
    void GenerateMapVisual()
    {
        foreach (var item in tileMapList)
        {
            TileType type = tileTypes[tileTypeMap[item.x, item.y, item.z]];
            GameObject cube = (GameObject)Instantiate(type.tileVisualPrefab, new Vector3(item.x, item.y, item.z), Quaternion.identity);

            Tile clickableTile = cube.GetComponent<Tile>();
            if (tileTypeMap[item.x, item.y, item.z] < 3)
            {
                clickableTile.isClickable = true;
                clickableTile.tileLocation = item;
                clickableTile.map = this;
            }
        }
    }

    /* MAP GRAPH
     * This function creates nodes on all of the tiles and defines neighbors for pathfinding usage
     */
    void GenerateMapGraph()
    {
        graph = new Node[mapSizeX, mapSizeY, mapSizeZ];
        graphNodeList = new List<Vector3Int>();

        foreach (var item in tileMapList)
        {
            graph[item.x, item.y, item.z] = new Node();
            graph[item.x, item.y, item.z].x = item.x;
            graph[item.x, item.y, item.z].y = item.y;
            graph[item.x, item.y, item.z].z = item.z;
            graphNodeList.Add(item);
        }

        foreach (var index in graphNodeList)
        {
            if (index.x > 0)
                graph[index.x, index.y, index.z].neighbors.Add(graph[index.x - 1, index.y, index.z]);
            if (index.x < mapSizeX - 1)
                graph[index.x, index.y, index.z].neighbors.Add(graph[index.x + 1, index.y, index.z]);
            if (index.y > 0)
                graph[index.x, index.y, index.z].neighbors.Add(graph[index.x, index.y - 1, index.z]);
            if (index.y < mapSizeY - 1)
                graph[index.x, index.y, index.z].neighbors.Add(graph[index.x, index.y + 1, index.z]);
        }
    }

    /* PATHFINDING
     * This function is responsible for the pathfinding behind moving units
     */

    public void GeneratePathTo(Vector3Int tileLocation)
    {
        // Clear out our unit's old path.
        selectedUnit.GetComponent<Unit>().currentPath = null;

        Vector3Int unitLocation = selectedUnit.GetComponent<Unit>().unitLocation = 
            new Vector3Int((int)selectedUnit.transform.position.x,
            (int)selectedUnit.transform.position.y,
            (int)selectedUnit.transform.position.z);

        if (UnitCanEnterTile(tileLocation) == false)
        {
            return;
        }
    }

    /* MOVING THE UNIT
     * This function is responsible for moving units (NOT PATHFINDING)
     */
    public void MoveSelectedUnitTo(Vector3Int gridCoords)
    {
        selectedUnit.GetComponent<Unit>().unitLocation = gridCoords;
        selectedUnit.transform.position = gridCoords;
    }

    /// Test to see if the unit can enter the tile
    public bool UnitCanEnterTile(Vector3Int tileCoords)
    {
        return tileTypes[tileTypeMap[tileCoords.x, tileCoords.y, tileCoords.z]].isWalkable;
    }

    /// Return the movement cost of a tile
    public float CostToEnterTile(Vector3Int tileCoords)
    {
        return tileTypes[tileTypeMap[tileCoords.x, tileCoords.y, tileCoords.z]].movementCost;
    }
}
