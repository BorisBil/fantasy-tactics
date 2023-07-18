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
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    private void Start()
    {
        GenerateMapData();
        GenerateMapVisual();
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
    }
    
    /* GENERATE THE MAP
     * This function is responsible for generating the actual map visuals itself
     */
    void GenerateMapVisual()
    {
        int tileX, tileY, tileZ;
        foreach (var item in tileMapList)
        {
            tileX = item.x; tileY = item.y; tileZ = item.z;

            TileType type = tileTypes[tileTypeMap[tileX, tileY, tileZ]];
            GameObject cube = (GameObject)Instantiate(type.tileVisualPrefab, new Vector3(tileX, tileY, tileZ), Quaternion.identity);
        }
    }

    /* MAP GRAPH
     * This function creates nodes on all of the tiles and defines neighbors for pathfinding usage
     */
    void GenerateMapGraph()
    {
        graph = new Node[mapSizeX, mapSizeY, mapSizeZ];

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                graph[x, y, 0] = new Node();
                graph[x, y, 0].x = x;
                graph[x, y, 0].y = y;
            }
        }

        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                if (x > 0)
                    graph[x, y, 0].neighbors.Add(graph[x - 1, y, 0]);
                if (x < mapSizeX - 1)
                    graph[x, y, 0].neighbors.Add(graph[x + 1, y, 0]);
                if (y > 0)
                    graph[x, y, 0].neighbors.Add(graph[x, y - 1, 0]);
                if (y < mapSizeY - 1)
                    graph[x, y, 0].neighbors.Add(graph[x, y + 1, 0]);
            }
        }
    }

    /* MOVING THE UNIT
     * This function is responsible for moving units (NOT PATHFINDING)
     */
    public void MoveSelectedUnitTo(int x, int y, int z)
    {
        selectedUnit.GetComponent<Unit>().tileX = x;
        selectedUnit.GetComponent<Unit>().tileY = y;
        selectedUnit.GetComponent<Unit>().tileZ = z;
        selectedUnit.transform.position = TileCoordToWorldCoord(x, y, z);
    }

    /// Just gets a vector3 of world coordinates
    public Vector3 TileCoordToWorldCoord(int x, int y, int z)
    {
        return new Vector3(x, y, z);
    }
}
