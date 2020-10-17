using System;

namespace CraftopiaRNGTool
{
    public class TreasureBoxData
    {
        public int Id { get; set; }
        public int PosHash { get; set; }
        public int Rarity { get; set; }
        public string Memo { get; set; }
        public string Spawn { get; set; }
        public string X { get; set; }
        public string Y { get; set; }
        public string Z { get; set; }

        public TreasureBoxData()
        {
            PosHash = 0;
        }

        public TreasureBoxData(int posHash)
        {
            PosHash = posHash;
        }

        public TreasureBoxData(string[] strs)
        {
            PosHash = TreasureCalc.IntParse(strs[0]);
            Memo = strs[1];
        }

        public TreasureBoxData(string[] strs, string memo)
        {
            Id = TreasureCalc.IntParse(strs[0]);
            Rarity = GetTreBoxRarity(strs[1]);
            PosHash = TreasureCalc.IntParse(strs[2]);
            string format = "{0:#.##}";
            X = String.Format(format, strs[3]);
            Y = String.Format(format, strs[4]);
            Z = String.Format(format, strs[5]);
            Spawn = strs[6];
            Memo = memo;
        }
        
        private int GetTreBoxRarity(string str)
        {
            if (str == "Legendary")
            {
                return 2;
            }
            else if (str == "Epic")
            {
                return 1;
            }

            return 0;
        }
    }
}
