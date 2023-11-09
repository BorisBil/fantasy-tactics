using System.Collections.Generic;

/// 
/// THIS CLASS HOLDS INFORMATION ON THE POD'S STATUS AND THE TYPES OF UNITS IN THE POD, FOR USAGE BY THE POD DATABASE
/// 

public class EnemyPod
{
    public string name;

    public string status;

    public List<EnemyUnitType> unitsInPod = new List<EnemyUnitType>();
}
