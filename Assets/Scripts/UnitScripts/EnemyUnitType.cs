using UnityEngine;

/// 
/// THIS CLASS HOLDS THE INFORMATION ON A TYPE OF ENEMY UNIT
/// 

public class EnemyUnitType : ScriptableObject
{   
    public new string name;

    public GameObject unitModel;

    public string unitDescription;
    public string unitType;

    public int unitSpeed;
    public int baseMovement;
    public int baseHP;
    public int unitTeam;
    public int visionRadius;
}
