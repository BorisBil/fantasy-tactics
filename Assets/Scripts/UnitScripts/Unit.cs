using System.Collections.Generic;
using UnityEngine;

/// 
/// THIS HOLDS ALL OF THE INFORMATION ON THE UNIT GAMEOBJECT
/// 
[System.Serializable]
public class Unit : MonoBehaviour
{
    public new string name;

    public GameObject unitModel;
    public GameObject unitObject;

    public List<Node> currentPath = null;
    
    public string unitDescription;
    public string unitType;

    public float unitSpeed;
    public int movementRange;
    public int attackRange;

    public int unitTeam;

    public Vector3 unitPosition;
}
