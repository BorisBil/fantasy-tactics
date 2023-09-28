using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase
{
    public Dictionary<string, Item> itemDatabase;

    public ItemDatabase()
    {
        itemDatabase = new Dictionary<string, Item>();
    }

    public void addWeaponToDatabase(Item.Weapon weapon)
    {
        itemDatabase.Add(weapon.name, weapon);
    }

    public void addArmorToDatabase(Item.Armor armor)
    {
        itemDatabase.Add(armor.name, armor);
    }

    public void GenerateWeaponsList()
    {
        Item.Weapon.Shortbow shortbow = ScriptableObject.CreateInstance<Item.Weapon.Shortbow>();
        Item.Weapon.Longbow longbow = ScriptableObject.CreateInstance<Item.Weapon.Longbow>();
        Item.Weapon.Crossbow crossbow = ScriptableObject.CreateInstance<Item.Weapon.Crossbow>();
        Item.Weapon.Shortsword shortsword = ScriptableObject.CreateInstance<Item.Weapon.Shortsword>();
        Item.Weapon.Longsword longsword = ScriptableObject.CreateInstance<Item.Weapon.Longsword>();
        Item.Weapon.Unarmed unarmed = ScriptableObject.CreateInstance<Item.Weapon.Unarmed>();

        /*
         * UNARMED
         */

        unarmed.name = "Zombie Fists";
        unarmed.description = "You have no weapon!";
        unarmed.weaponType = "Hands";

        unarmed.minDamage = 1;
        unarmed.maxDamage = 3;
        unarmed.range = 1;
        unarmed.accuracy = 0.4f;
        unarmed.actionCost = 1;

        unarmed.levelReq = 1;
        unarmed.goldCost = 0;
        unarmed.rarity = 0;

        unarmed.weight = 0;

        unarmed.id = 1;
        addWeaponToDatabase(unarmed);

        /*
         * UNARMED
         */

        /*
         * GOBLIN WEAPONS
         */

        /// Longsword
        longsword.name = "Goblin Longsword";
        longsword.description = "This is a goblin's longsword";
        longsword.weaponType = "Longsword";

        longsword.minDamage = 2;
        longsword.maxDamage = 5;
        longsword.range = 1;
        longsword.accuracy = 0.7f;
        longsword.actionCost = 1;

        longsword.levelReq = 1;
        longsword.goldCost = 100;
        longsword.rarity = 1;

        longsword.weight = 2;

        longsword.id = 1;
        addWeaponToDatabase(longsword);

        /// Shortsword
        shortsword.name = "Goblin Shortsword";
        shortsword.description = "This is a goblin's shortsword";
        shortsword.weaponType = "Shortsword";

        shortsword.minDamage = 1;
        shortsword.maxDamage = 4;
        shortsword.range = 1;
        shortsword.accuracy = 0.8f;
        shortsword.actionCost = 1;

        shortsword.levelReq = 1;
        shortsword.goldCost = 100;
        shortsword.rarity = 1;

        shortsword.weight = 1;

        shortsword.id = 2;
        addWeaponToDatabase(shortsword);

        /// Crossbow
        crossbow.name = "Goblin Crossbow";
        crossbow.description = "This is a goblin's crossbow";
        crossbow.weaponType = "Crossbow";

        crossbow.minDamage = 3;
        crossbow.maxDamage = 6;
        crossbow.range = 6;
        crossbow.accuracy = 0.5f;
        crossbow.actionCost = 2;

        crossbow.levelReq = 1;
        crossbow.goldCost = 100;
        crossbow.rarity = 1;

        crossbow.weight = 5;

        crossbow.id = 3;
        addWeaponToDatabase(crossbow);

        /// Longbow
        longbow.name = "Goblin Longbow";
        longbow.description = "This is a goblin's longbow";
        longbow.weaponType = "Longbow";

        longbow.minDamage = 2;
        longbow.maxDamage = 5;
        longbow.range = 5;
        longbow.accuracy = 0.4f;
        longbow.actionCost = 2;

        longbow.levelReq = 1;
        longbow.goldCost = 100;
        longbow.rarity = 1;

        longbow.weight = 4;

        longbow.id = 4;
        addWeaponToDatabase(longbow);

        /*
         * GOBLIN WEAPONS
         */
    }

    public void GenerateArmorList()
    {
        Item.Armor.Helmet helmet = ScriptableObject.CreateInstance<Item.Armor.Helmet>();
        Item.Armor.ChestArmor armor = ScriptableObject.CreateInstance<Item.Armor.ChestArmor>();
        Item.Armor.Boots boots = ScriptableObject.CreateInstance<Item.Armor.Boots>();
        Item.Armor.Chausses chausses = ScriptableObject.CreateInstance<Item.Armor.Chausses>();

        /* 
         * GOBLIN ARMOR SET
         */

        /// Chest Armor
        armor.name = "Goblin Chest Armor";
        armor.description = "This is a goblin's armor";
        armor.armorType = "Light";

        armor.hpAmount = 1;
        armor.apAmount = 0;

        armor.levelReq = 1;
        armor.goldCost = 150;
        armor.rarity = 1;

        armor.weight = 5;

        armor.id = 1;
        addArmorToDatabase(armor);

        /// Helmet
        helmet.name = "Goblin Helmet";
        helmet.description = "This is a goblin's armor";
        helmet.armorType = "Light";

        helmet.hpAmount = 1;
        helmet.apAmount = 0;

        helmet.levelReq = 1;
        helmet.goldCost = 120;
        helmet.rarity = 1;

        helmet.weight = 5;

        helmet.id = 2;
        addArmorToDatabase(helmet);
        
        /// Boots
        boots.name = "Goblin Boots";
        boots.description = "This is a goblin's armor";
        boots.armorType = "Light";

        boots.hpAmount = 1;
        boots.apAmount = 0;

        boots.levelReq = 1;
        boots.goldCost = 120;
        boots.rarity = 1;

        boots.weight = 5;

        boots.id = 3;
        addArmorToDatabase(boots);
        
        /// Legs
        chausses.name = "Goblin Chausses";
        chausses.description = "This is a goblin's armor";
        chausses.armorType = "Light";

        chausses.hpAmount = 1;
        chausses.apAmount = 0;

        chausses.levelReq = 1;
        chausses.goldCost = 120;
        chausses.rarity = 1;

        chausses.weight = 5;

        chausses.id = 4;
        addArmorToDatabase(chausses);
        
        /* 
         * GOBLIN ARMOR SET
         */
    }

    public void ListItemDatabase()
    {
        foreach (KeyValuePair<string, Item> kvp in itemDatabase)
        {
            Debug.Log(kvp);
        }
    }
}
