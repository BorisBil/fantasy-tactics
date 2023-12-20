using System.Collections.Generic;
using UnityEngine;

/// 
/// THIS GENERATES THE DATA AND SPAWNS IN THE MAP FOR THE DESERT HILLS ENVIRONMENT
///

public class DesertHills : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public TileSets.DesertHillTiles[] desertHills;

    private int[,,] tileTypeMap;

    private List<Vector3Int> tileMapList;
    private List<Vector3Int> graphNodeList;

    public GameObject map;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /* MAP DATA GENERATION FUNCTION
     * This function creates and stores map data into several arrays and lists for game map usage
     */
    public void GenerateMapData(int mapSizeX, int mapSizeY, int mapSizeZ)
    {
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

        for (int x = 0; x < 3; x++)
        {
            for (int y = 3; y < 6; y++)
            {
                tileTypeMap[x, y, 0] = 1;
            }
        }

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
                tileTypeMap[x, y, 1] = 2;
                Vector3Int tileLocation = new Vector3Int(x, y, 1);
                tileMapList.Add(tileLocation);
            }
        }
    }

    /* GENERATE THE MAP
     * This function is responsible for generating the actual map visuals itself
     */
    public void GenerateMapVisual()
    {
        foreach (var item in tileMapList)
        {
            /// Instantiating the tiles after setting their type based on the map graph
            TileSets.DesertHillTiles type = desertHills[tileTypeMap[item.x, item.y, item.z]];
            GameObject tile = Instantiate(type.tileVisualPrefab, new Vector3(item.x, item.y, item.z), Quaternion.identity);
            tile.transform.parent = map.transform;

            /// Setting clickable tiles based on type
            Tile clickableTile = tile.GetComponent<Tile>();
            if (tileTypeMap[item.x, item.y, item.z] < 3)
            {
                clickableTile.isClickable = true;
                clickableTile.tileLocation = item;
            }
        }
    }

    /* MAP GRAPH
     * This function creates nodes on all of the tiles and defines neighbors for pathfinding usage
     */
    public Node[,,] GenerateMapGraph(int mapSizeX, int mapSizeY, int mapSizeZ)
    {
        Node[,,] graph;

        graph = new Node[mapSizeX, mapSizeY, mapSizeZ];
        graphNodeList = new List<Vector3Int>();

        /// Generate a node for each tile we have on the map
        foreach (var item in tileMapList)
        {
            graph[item.x, item.y, item.z] = new Node();
            graph[item.x, item.y, item.z].movementCost =
                desertHills[tileTypeMap[item.x, item.y, item.z]].movementCost;

            /// Setting unwalkable tiletypes to not be walkable on the node grid
            if (desertHills[tileTypeMap[item.x, item.y, item.z]].isWalkable == true)
            {
                graph[item.x, item.y, item.z].isWalkable = true;
            }
            else
            {
                graph[item.x, item.y, item.z].isWalkable = false;
            }

            /// Setting "underground" nodes to be unwalkable
            if (tileMapList.Contains(new Vector3Int(item.x, item.y, item.z + 1)))
            {
                graph[item.x, item.y, item.z].isWalkable = false;
            }

            graph[item.x, item.y, item.z].location = new Vector3Int(item.x, item.y, item.z);
            graphNodeList.Add(item);
        }

        foreach (var index in graphNodeList)
        {
            /// Adding all the neighbors that are on the same plane
            if (tileMapList.Contains(new Vector3Int(index.x - 1, index.y, index.z)))
            {
                graph[index.x, index.y, index.z].neighbors.Add(graph[index.x - 1, index.y, index.z]);
            }

            if (tileMapList.Contains(new Vector3Int(index.x + 1, index.y, index.z)))
            {
                graph[index.x, index.y, index.z].neighbors.Add(graph[index.x + 1, index.y, index.z]);
            }

            if (tileMapList.Contains(new Vector3Int(index.x, index.y - 1, index.z)))
            {
                graph[index.x, index.y, index.z].neighbors.Add(graph[index.x, index.y - 1, index.z]);
            }

            if (tileMapList.Contains(new Vector3Int(index.x, index.y + 1, index.z)))
            {
                graph[index.x, index.y, index.z].neighbors.Add(graph[index.x, index.y + 1, index.z]);
            }

            /// Adding "steps" between higher/lower ground to the neighbors
            if (tileMapList.Contains(new Vector3Int(index.x - 1, index.y, index.z - 1)))
            {
                if (graph[index.x - 1, index.y, index.z - 1].isWalkable)
                {
                    graph[index.x, index.y, index.z].neighbors.Add(graph[index.x - 1, index.y, index.z - 1]);
                    graph[index.x - 1, index.y, index.z - 1].neighbors.Add(graph[index.x, index.y, index.z]);
                }
            }

            if (tileMapList.Contains(new Vector3Int(index.x + 1, index.y, index.z - 1)))
            {
                if (graph[index.x + 1, index.y, index.z - 1].isWalkable)
                {
                    graph[index.x, index.y, index.z].neighbors.Add(graph[index.x + 1, index.y, index.z - 1]);
                    graph[index.x + 1, index.y, index.z - 1].neighbors.Add(graph[index.x, index.y, index.z]);
                }
            }

            if (tileMapList.Contains(new Vector3Int(index.x, index.y - 1, index.z - 1)))
            {
                if (graph[index.x, index.y - 1, index.z - 1].isWalkable)
                {
                    graph[index.x, index.y, index.z].neighbors.Add(graph[index.x, index.y - 1, index.z - 1]);
                    graph[index.x, index.y - 1, index.z - 1].neighbors.Add(graph[index.x, index.y, index.z]);
                }
            }

            if (tileMapList.Contains(new Vector3Int(index.x, index.y + 1, index.z - 1)))
            {
                if (graph[index.x, index.y + 1, index.z - 1].isWalkable)
                {
                    graph[index.x, index.y, index.z].neighbors.Add(graph[index.x, index.y + 1, index.z - 1]);
                    graph[index.x, index.y + 1, index.z - 1].neighbors.Add(graph[index.x, index.y, index.z]);
                }
            }
        }

        return graph;
    }

    public List<Vector3Int> ReturnGraphList()
    {
        return graphNodeList;
    }
}