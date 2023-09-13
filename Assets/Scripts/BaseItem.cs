using System.Collections.Generic;

public class BaseItem
{
    public List<Enchant> enchant { get; set; }

    public string name { get; set; }
    public string description { get; set; }

    public int levelReq { get; set; }

    public float goldCost { get; set; }

    public int id { get; set; }

    public int rarity { get; set; }

    public float weight { get; set; }
}
