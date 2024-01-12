using System.Collections.Generic;
using UnityEngine;

/// 
/// THIS HOLDS ALL OF THE INFORMATION ON THE UNIT GAMEOBJECT
/// 

[System.Serializable]
public class Unit : MonoBehaviour
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public new string name;
    public int id;

    public GameObject unitModel;
    public GameObject unitObject;

    public List<Node> currentPath = null;
    
    public string unitDescription;
    public string unitType;
    public string status;

    public float unitSpeed;
    public int baseMovement;
    public int movementRange;

    public int baseHP;
    public int totalHP;

    public float armorWeight;
    public int armorHP;
    public int armorAP;
    public int attackRange;
    public int visionRadius;

    public int unitTeam;

    public int level;

    public float actionPoints;

    public Vector3 unitPosition;

    public Item.Weapon weapon;
    public Item.Shield shield;
    public List<Item.Armor> armor;

    public List<Item> inventory;

    public List<Unit> attackableUnits;

    public List<Unit> visibleUnits;
    public List<Node> visibleTiles;

    public Dictionary<string, string> inCover;

    public bool isMoving;
    public bool isSelectable;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /// Calculates the stats of the unit based on its equipped items
    public void CalculateStats()
    {
        attackRange = weapon.range;

        foreach (var item in armor)
        {
            armorWeight = armorWeight + item.weight;
            armorAP = armorAP + item.apAmount;
            armorHP = armorHP + item.hpAmount;
        }

        totalHP = baseHP + armorHP;
        movementRange = (int)System.Math.Round(baseMovement - 0.1 * (weapon.weight + armorWeight));
    }

    /// Equips the weapon supplied
    public void AddWeapon(Item.Weapon weaponToAdd)
    {
        weapon = weaponToAdd;
    }

    /// Unequips the weapon
    public void RemoveWeapon()
    {
        weapon = null;
    }

    /// Equips armor supplied
    public void AddArmor(Item.Armor armorToAdd)
    {
        armor.Add(armorToAdd);
    }

    /// Unequips the armor
    public void RemoveArmor(Item.Armor armorToRemove)
    {
        armor.Remove(armorToRemove);
    }
}
