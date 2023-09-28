using UnityEngine;

/// 
/// MAIN GAME FILE
/// 

public class GameManager : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public TileMap tileMap;
    public UnitManager unitManager;
    public ItemDatabase itemDatabase;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /// 
    /// Void Start Unity method that launches on startup
    /// 
    public void Start()
    {
        itemDatabase = new ItemDatabase();

        itemDatabase.GenerateWeaponsList();
        itemDatabase.GenerateArmorList();
        itemDatabase.ListItemDatabase();

        tileMap.GenerateGrassyHills(10, 10, 5);

        Unit unit = unitManager.NewPlayerUnit(new Vector3Int(0, 0, 0));
        unit.AddWeapon((Item.Weapon)itemDatabase.itemDatabase["Goblin Longsword"]);
        unit.AddArmor((Item.Armor)itemDatabase.itemDatabase["Goblin Chest Armor"]);
        unit.CalculateStats();

        unit = unitManager.NewPlayerUnit(new Vector3Int(2, 0, 0));
        unit.AddWeapon((Item.Weapon)itemDatabase.itemDatabase["Goblin Longsword"]);
        unit.AddArmor((Item.Armor)itemDatabase.itemDatabase["Goblin Chest Armor"]);
        unit.CalculateStats();

        unit = unitManager.SpawnEnemyUnit(unitManager.enemyUnitType[1], new Vector3Int(8, 8, 0));
        unit.AddWeapon((Item.Weapon)itemDatabase.itemDatabase["Zombie Fists"]);
        unit.CalculateStats();
    }

    public void GameLost()
    {
        Debug.Log("Game lost!");
    }
}
