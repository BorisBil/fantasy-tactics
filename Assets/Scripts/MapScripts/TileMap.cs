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

    private Node[,,] graph;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    public void GenerateGrassyHills(int mapSizeX, int mapSizeY, int mapSizeZ)
    {
        grassyHills.GenerateMapData(mapSizeX, mapSizeY, mapSizeZ);
        grassyHills.GenerateMapVisual();
        graph = grassyHills.GenerateMapGraph(mapSizeX, mapSizeY, mapSizeZ);
    }

    public void GenerateDesertHills(int mapSizeX, int mapSizeY, int mapSizeZ)
    {
        desertHills.GenerateMapData(mapSizeX, mapSizeY, mapSizeZ);
        desertHills.GenerateMapVisual();
        graph = desertHills.GenerateMapGraph(mapSizeX, mapSizeY, mapSizeZ);
    }

    /* MOVEMENT RANGE
     * This function computes the movement range of a unit
     */
    public List<Node> TileRange(Unit character)
    {
        /// BFS until depth character movement range, return all nodes traversed
        Vector3 unitLocation = character.unitPosition;
        Node startNode = graph[(int)unitLocation.x, (int)unitLocation.y, (int)unitLocation.z];
        
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
    public void UpdatePath(Vector3Int gridCoords, Unit unit)
    {
        List<Node> path = GeneratePathTo(gridCoords, unit.unitPosition);

        unit.currentPath = path;
    }

    /* PATHFINDING
     * This function is responsible for the pathfinding behind moving units
     */
    public List<Node> GeneratePathTo(Vector3Int targetLocation, Vector3 unitLocation)
    {
        if (graph[targetLocation.x, targetLocation.y, targetLocation.z].isWalkable == false)
        {
            return null;
        }

        Node startNode = graph[(int)unitLocation.x, (int)unitLocation.y, (int)unitLocation.z];
        Node endNode = graph[targetLocation.x, targetLocation.y, targetLocation.z];

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
    private int GetDistance(Node start, Node neighbor)
    {
        return  Mathf.Abs(start.location.x - neighbor.location.x) + 
                Mathf.Abs(start.location.y - neighbor.location.y) + 
                Mathf.Abs(start.location.z - neighbor.location.z);
    }
}
