public class Item
{
    [System.Serializable]
    public class Weapon : BaseItem
    {
        public int weaponType;

        public int minDamage;
        public int maxDamage;

        public int range;

        public float accuracy;
        public float actionCost;
    }

    [System.Serializable]
    public class Armor : BaseItem
    {
        public int armorType;

        public int hpAmount;
        public int armorAmount;
    }
}
