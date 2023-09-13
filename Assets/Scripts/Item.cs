public class Item
{
    [System.Serializable]
    public class Weapon : BaseItem
    {
        public int weaponType { get; set; }

        public int minDamage { get; set; }
        public int maxDamage { get; set; }

        public int range { get; set; }

        public float accuracy { get; set; }
        public float actionCost { get; set; }
    }

    [System.Serializable]
    public class Armor : BaseItem
    {
        public int armorType { get; set; }

        public int hpAmount { get; set; }
        public int armorAmount { get; set; }
    }
}
