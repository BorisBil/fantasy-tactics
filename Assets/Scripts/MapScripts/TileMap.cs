using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// 
/// MAP GENERATOR ALONG WITH PATHFINDING
/// 

public class TileMap : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public GrassyHills grassyHills;
    public DesertHills desertHills;

    public VisionManager visionManager;

    public Dictionary<Vector3, Node> graph;
    public Dictionary<Vector3, TileLight> lightsGraph;

    public List<TileLight> lights;

    public List<Node> walkableNodes;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    public void GenerateGrassyHills(int mapSizeX, int mapSizeY, int mapSizeZ)
    {
        grassyHills.GenerateMapData(mapSizeX, mapSizeY, mapSizeZ);
        grassyHills.GeneratePropData(mapSizeX, mapSizeX, mapSizeZ);
        grassyHills.GenerateMapVisual();
        
        graph = grassyHills.GenerateMapGraph(mapSizeX, mapSizeY, mapSizeZ);
        lights = grassyHills.GenerateMapLighting(mapSizeX, mapSizeY, mapSizeZ);
        
        walkableNodes = grassyHills.ReturnWalkableNodeList();
        lightsGraph = grassyHills.ReturnLightGraph();
    }

    public void GenerateDesertHills(int mapSizeX, int mapSizeY, int mapSizeZ)
    {
        //desertHills.GenerateMapData(mapSizeX, mapSizeY, mapSizeZ);
        //desertHills.GenerateMapVisual();
        
        //graph = desertHills.GenerateMapGraph(mapSizeX, mapSizeY, mapSizeZ);
        //graphNodeList = desertHills.ReturnGraphList();
    }

    /* MOVEMENT RANGE
     * This function computes the movement range of a unit
     */
    public List<Node> TileRange(Unit character)
    {
        /// BFS until depth character movement range, return all nodes traversed
        Vector3 unitLocation = character.unitPosition;
        Node startNode = graph[new Vector3(unitLocation.x, unitLocation.y, unitLocation.z)];
        
        Dictionary<Node, float> movement_available = new Dictionary<Node, float>();
        
        movement_available[startNode] = character.movementRange;

        List<Node> openList = new List<Node>();
        openList.Add(startNode);

        List<Node> range = new List<Node>();

        while (openList.Count > 0) 
        {
            Node currentNode = openList.First();
            openList.Remove(currentNode);

            foreach (Node neighbor in currentNode.neighbors)
            {
                float new_movement_available = movement_available[currentNode] - neighbor.movementCost;
                
                if ((neighbor.isWalkable) && (!movement_available.ContainsKey(neighbor) || new_movement_available > movement_available[neighbor]) && new_movement_available >= 0)
                {
                    movement_available[neighbor] = new_movement_available;
                    
                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
                    
                    range.Add(neighbor);
                }
            }
        }
        
        return range;
    }

    /* MOVING THE UNIT
     * This function is responsible for moving units (NOT PATHFINDING)
     */
    public void UpdatePath(Vector3 gridCoords, Unit unit)
    {
        List<Node> path = GeneratePathTo(gridCoords, unit.unitPosition);

        unit.currentPath = path;
    }

    /* PATHFINDING
     * This function is responsible for the pathfinding behind moving units
     */
    public List<Node> GeneratePathTo(Vector3 targetLocation, Vector3 unitLocation)
    {
        Node startNode = graph[new Vector3(unitLocation.x, unitLocation.y, unitLocation.z)];
        Node endNode = graph[new Vector3(targetLocation.x, targetLocation.y, targetLocation.z)];

        List<Node> openList = new List<Node>();
        Dictionary<Node, float> cost_so_far = new Dictionary<Node, float>();
        
        openList.Add(startNode);
        cost_so_far[startNode] = 0;

        while (openList.Count > 0) 
        {
            Node currentNode = openList.OrderBy(node => node.F).First();

            openList.Remove(currentNode);

            if (currentNode == endNode)
            {
                return GetFinishedList(startNode, endNode);
            }

            foreach (Node neighbor in currentNode.neighbors)
            {
                float new_cost = cost_so_far[currentNode] + neighbor.movementCost;
                
                if ((neighbor.isWalkable) && (!cost_so_far.ContainsKey(neighbor) || new_cost < cost_so_far[neighbor]))
                {
                    cost_so_far[neighbor] = new_cost;
                    neighbor.F = new_cost + GetDistance(endNode, neighbor);
                    neighbor.previous = currentNode;
                    
                    if (!openList.Contains(neighbor))
                    {
                        openList.Add(neighbor);
                    }
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

    /// Gets the (3D Version?) Manhattan distance between nodes
    public float GetDistance(Node start, Node end)
    {
        return  Mathf.Abs(start.location.x - end.location.x) + 
                Mathf.Abs(start.location.y - end.location.y) + 
                Mathf.Abs(start.location.z - end.location.z);
    }
}
