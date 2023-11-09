using UnityEngine;

/// 
/// ITEM OBJECT WITH ITS STATS, ALSO OF DIFFERENT TYPES SO AS TO SPECIFIY DIFFERENT KINDS OF STATS FOR DIFFERENT ITEMS
/// 

public class Item : ScriptableObject
{
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    public int id;

    public new string name;
    public string description;

    public int levelReq;

    public float goldCost;

    public int rarity;

    public float weight;
    // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

    /*
     * Weapons item class with several subclasses
     */
    public class Weapon : Item
    {
        // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
        public string weaponType;

        public int minDamage;
        public int maxDamage;

        public int range;

        public float accuracy;
        public float actionCost;
        public int hand = 1;
        // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

        public class Shortbow : Weapon
        {

        }

        public class Longbow : Weapon
        {

        }

        public class Crossbow : Weapon
        {

        }

        public class Shortsword : Weapon
        {

        }

        public class Longsword : Weapon
        {

        }

        public class Unarmed : Weapon
        {

        }
    }

    /*
     * Armor item class with several subclasses
     */
    public class Armor : Item
    {
        // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
        public string armorType;

        public int hpAmount;
        public int apAmount;
        // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS

        public class ChestArmor : Armor
        {

        }

        public class Helmet : Armor
        {

        }

        public class Boots : Armor
        {

        }

        public class Chausses : Armor
        {

        }
    }

    /*
     * Shield item class
     */ 
    public class Shield : Item
    {
        // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
        public string shieldType;

        public int actionCost;

        public int hpAmount;
        public int apAmount;
        // NECESSARY PUBLIC/PRIVATE VARIABLES, LISTS, AND ARRAYS
    }
}
