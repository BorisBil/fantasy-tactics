using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Vector3Int unitLocation;

    public List<Node> currentPath = null;

    public string unitName;
    public string unitDescription;
    public string unitType;

    public int unitSpeed;
    public int unitRange;
}
