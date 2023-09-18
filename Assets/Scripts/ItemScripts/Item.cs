using UnityEngine;

public class Item : ScriptableObject
{
    public int id;

    public new string name;
    public string description;

    public int levelReq;

    public float goldCost;

    public int rarity;

    public float weight;

    public class Weapon : Item
    {
        public string weaponType;

        public int minDamage;
        public int maxDamage;

        public int range;

        public float accuracy;
        public float actionCost;
        public int hand = 1;

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
    }

    public class Armor : Item
    {
        public string armorType;

        public int hpAmount;
        public int apAmount;

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

    public class Shield : Item
    {
        public string shieldType;

        public int actionCost;

        public int hpAmount;
        public int apAmount;
    }
}
