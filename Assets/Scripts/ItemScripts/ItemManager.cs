using System.Collections.Generic;
using UnityEngine;

/// 
/// THIS MANAGES THE ITEM DATABASE AND ADDS OR TAKES AWAY ITEMS FROM UNITS AS DETERMINED
///

public class ItemManager : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public ItemDatabase itemDatabase;

    public Dictionary<string, Item> itemList;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /// Create the item database, attach it to this class
    public void CreateItemDatabase()
    {
        itemDatabase = new ItemDatabase();

        itemList = itemDatabase.ReturnItemDatabase();
    }

    /// Determine which items to add to which units
    public void DetermineItems(Unit unit)
    {
        if (unit.unitType == "Zombie")
        {
            AddWeaponToUnit(unit, (Item.Weapon)itemList["Zombie Fists"]);

            unit.CalculateStats();
        }
    }

    /// Adds items to player units
    public void GivePlayerItems(Unit unit)
    {
        AddWeaponToUnit(unit, (Item.Weapon)itemList["Goblin Longsword"]);
        AddArmorToUnit(unit, (Item.Armor)itemList["Goblin Chest Armor"]);

        unit.CalculateStats();
    }

    /// Tells the unit to equip a weapon and calculate its stats
    public void AddWeaponToUnit(Unit unit, Item.Weapon weapon)
    {
        unit.AddWeapon(weapon);

        unit.CalculateStats();
    }

    /// Tells the unit to equip armor and calculate its stats
    public void AddArmorToUnit(Unit unit, Item.Armor armor)
    {
        unit.AddArmor(armor);

        unit.CalculateStats();
    }

    /// Tells a unit to unequip its weapon
    public void RemoveWeaponFromUnit(Unit unit)
    {
        unit.RemoveWeapon();

        unit.CalculateStats();
    }

    /// Tells a unit to unequip its armor
    public void RemoveArmorFromUnit(Unit unit, Item.Armor armor)
    {
        unit.RemoveArmor(armor);

        unit.CalculateStats();
    }
}
