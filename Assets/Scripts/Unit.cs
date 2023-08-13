using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Unit : MonoBehaviour
{
    public new string name;

    public GameObject unitModel;
    public GameObject unitObject;

    public List<Node> currentPath = null;
    
    public string unitDescription;
    public string unitType;

    public int unitSpeed;
    public int unitRange;

    public int unitTeam;

    public Vector3Int unitPosition;
}
