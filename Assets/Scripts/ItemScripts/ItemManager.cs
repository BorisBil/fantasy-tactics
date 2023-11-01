using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemDatabase itemDatabase;

    public Dictionary<string, Item> itemList;

    public void CreateItemDatabase()
    {
        itemDatabase = new ItemDatabase();

        itemList = itemDatabase.ReturnItemDatabase();
    }

    public void DetermineItems(Unit unit)
    {
        if (unit.unitType == "Zombie")
        {
            AddWeaponToUnit(unit, (Item.Weapon)itemList["Zombie Fists"]);

            unit.CalculateStats();
        }
    }

    public void GivePlayerItems(Unit unit)
    {
        AddWeaponToUnit(unit, (Item.Weapon)itemList["Goblin Longsword"]);
        AddArmorToUnit(unit, (Item.Armor)itemList["Goblin Chest Armor"]);

        unit.CalculateStats();
    }

    public void AddWeaponToUnit(Unit unit, Item.Weapon weapon)
    {
        unit.AddWeapon(weapon);

        unit.CalculateStats();
    }

    public void AddArmorToUnit(Unit unit, Item.Armor armor)
    {
        unit.AddArmor(armor);

        unit.CalculateStats();
    }

    public void RemoveWeaponFromUnit(Unit unit)
    {
        unit.RemoveWeapon();

        unit.CalculateStats();
    }

    public void RemoveArmorFromUnit(Unit unit, Item.Armor armor)
    {
        unit.RemoveArmor(armor);

        unit.CalculateStats();
    }
}
