using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public string name;

    public Vector3Int unitLocation;

    public GameObject unitModel;

    public List<Node> currentPath = null;
    
    public string unitDescription;
    public string unitType;

    public int unitSpeed;
    public int unitRange;

    public int unitTeam;
}
