using UnityEngine;

/// 
/// MAIN GAME FILE
/// 

public class GameManager : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public TileMap tileMap;
    public UnitManager unitManager;
    public ItemManager itemManager;
    public EnemyManager enemyManager;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /// 
    /// Void Start Unity method that launches on startup
    /// 
    public void Start()
    {
        enemyManager.CreateEnemyDatabases();
        itemManager.CreateItemDatabase();

        tileMap.GenerateGrassyHills(10, 10, 5);

        Unit unit = unitManager.NewPlayerUnit(new Vector3Int(0, 0, 0));

        unit = unitManager.NewPlayerUnit(new Vector3Int(2, 0, 0));

        enemyManager.DetermineEnemies(2, "GrassyHills");
    }

    public void GameLost()
    {
        Debug.Log("Game lost!");
    }
}
