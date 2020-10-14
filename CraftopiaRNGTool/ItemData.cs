using System;

namespace CraftopiaRNGTool
{
    public class ItemData
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string RarityType { get; set; }
        public int Id { get; set; }
        public int Rarity { get; set; }
        public int Prob { get; set; }
        public EnchantData[] Enchant { get; set; }

        public ItemData()
        {
        }

        public ItemData(string[] strs)
        {
            if (strs.Length < 6)
            {
                strs = new string[6];
            }
            Id = TreasureCalc.IntParse(strs[0]);
            Name = strs[1];
            Type = strs[2];
            RarityType = strs[3];
            Rarity = TreasureCalc.IntParse(strs[4]);
            Prob = TreasureCalc.IntParse(strs[5]);
            Enchant = null;
        }

        public ItemData(ItemData item)
        {
            Id = item.Id;
            Name = item.Name;
            Type = item.Type;
            RarityType = item.RarityType;
            Rarity = item.Rarity;
            Prob = item.Prob;
            Enchant = null;
        }
    }
}
