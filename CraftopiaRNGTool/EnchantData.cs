using System;

namespace CraftopiaRNGTool
{
    public class EnchantData
    {
        public string Name { get; set; }
        public string RarityType { get; set; }
        public bool IsDrop { get; set; }
        public int Id { get; set; }
        public int Rarity { get; set; }
        public int Prob { get; set; }

        public EnchantData()
        {
        }

        public EnchantData(string[] strs)
        {
            if (strs.Length < 6)
            {
                strs = new string[6];
            }
            Id = TreasureCalc.IntParse(strs[0]);
            Name = strs[1];
            RarityType = strs[2];
            Rarity = TreasureCalc.IntParse(strs[3]);
            IsDrop = strs[4] == "0";
            Prob = TreasureCalc.IntParse(strs[5]);
        }
    }
}
