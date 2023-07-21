using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<Node> neighbors;
    public int x;
    public int y;
    public int z;

    public Node()
    {
        neighbors = new List<Node>();
    }

    public float DistanceTo(Node node)
    {
        if (node == null)
        {
            Debug.LogError("WTF?");
        }

        return Vector3.Distance(new Vector3(x, y, z), new Vector3(node.x, node.y, node.z));
    }
}
