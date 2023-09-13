using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.Json;

public class ItemManager : MonoBehaviour
{
    public Dictionary<int, Item.Weapon> weaponDict;

    public void populateDictionary()
    {
        public string text = File.ReadAllText("./weapons.json");
        public var weapons = JsonSerializer.Deserialize<List<Item.Weapon>>(text);
        public var dict = weapons.ToDictionary(x => x.id);
        Debug.Log($"Description:" + weaponDict[0].description.ToString());
    }
}
