using System.Collections.Generic;
using UnityEngine;

/// 
/// PATHFINDING GRID NODE INFORMATION
/// 

public class Node
{
    public Vector3 location;

    public float G;
    public float H;
    public float F; //{ get { return G + H; } }

    public bool isWalkable;
    public bool isRamp;
    public float movementCost;

    public Node previous;

    public List<Node> neighbors;

    public Node()
    {
        neighbors = new List<Node>();
    }
}
