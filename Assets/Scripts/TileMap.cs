using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.Text;

/// 
/// MAP GENERATOR ALONG WITH PATHFINDING
/// 
public class TileMap : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public TileSets.GrassyHills[] grassyHills;
    public TileSets.DesertHills[] desertHills;

    private Node[,,] graph;

    private int[,,] tileTypeMap;
    private int mapSizeX, mapSizeY, mapSizeZ;

    public List<Vector3Int> tileMapList;
    public List<Vector3Int> graphNodeList;

    public GameObject map;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    public void GenerateTileMap()
    {
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
    void GenerateMapVisual()
    {
        foreach (var item in tileMapList)
        {
            /// Instantiating the tiles after setting their type based on the map graph
            TileSets.GrassyHills type = grassyHills[tileTypeMap[item.x, item.y, item.z]];
            GameObject tile = Instantiate(type.tileVisualPrefab, new Vector3(item.x, item.y, item.z), Quaternion.identity);
            tile.transform.parent = map.transform;

            /// Setting clickable tiles based on type
            Tile clickableTile = tile.GetComponent<Tile>();
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

        /// Generate a node for each tile we have on the map
        foreach (var item in tileMapList)
        {
            graph[item.x, item.y, item.z] = new Node();
            graph[item.x, item.y, item.z].movementCost = 
                grassyHills[tileTypeMap[item.x, item.y, item.z]].movementCost;

            /// Setting unwalkable tiletypes to not be walkable on the node grid
            if (grassyHills[tileTypeMap[item.x, item.y, item.z]].isWalkable == true)
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
    }

    /* MOVING THE UNIT
     * This function is responsible for moving units (NOT PATHFINDING)
     */
    public void MoveSelectedUnitTo(Vector3Int gridCoords, Unit unit)
    {
        List<Node> path = GeneratePathTo(gridCoords, unit.unitPosition);

        unit.currentPath = path;

        foreach (Node node in path)
        {
            Debug.Log(node.location);
        }
    }

    /* PATHFINDING
     * This function is responsible for the pathfinding behind moving units
     */
    public List<Node> GeneratePathTo(Vector3Int targetLocation, Vector3Int unitLocation)
    {
        if (UnitCanEnterTile(targetLocation) == false)
        {
            return null;
        }

        Node startNode = graph[unitLocation.x, unitLocation.y, unitLocation.z];
        Node endNode = graph[targetLocation.x, targetLocation.y, targetLocation.z];

        List<Node> openList = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openList.Add(startNode);

        while (openList.Count > 0) 
        {
            Node currentNode = openList.OrderBy(x => x.F).First();

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == endNode)
            {
                return GetFinishedList(startNode, endNode);
            }

            foreach (Node neighbor in currentNode.neighbors)
            {
                if (!neighbor.isWalkable || closedList.Contains(neighbor))
                {
                    continue;
                }

                neighbor.G = GetDistance(startNode, neighbor) * neighbor.movementCost;
                neighbor.H = GetDistance(endNode, neighbor) * neighbor.movementCost;
                neighbor.previous = currentNode;

                if (!openList.Contains(neighbor))
                {
                    openList.Add(neighbor);
                }
            }
        }

        return new List<Node>();
    }

    /// Flips the pathfinding list around for the final result
    private List<Node> GetFinishedList(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.previous;
        }
        path.Reverse();

        return path;
    }

    /// Test to see if the unit can enter the tile
    public bool UnitCanEnterTile(Vector3Int tileCoords)
    {
        return grassyHills[tileTypeMap[tileCoords.x, tileCoords.y, tileCoords.z]].isWalkable;
    }

    /// Gets the (3D Version?) Manhattan distance between nodes
    private int GetDistance(Node start, Node neighbor)
    {
        return  Mathf.Abs(start.location.x - neighbor.location.x) + 
                Mathf.Abs(start.location.y - neighbor.location.y) + 
                Mathf.Abs(start.location.z - neighbor.location.z);
    }
}
