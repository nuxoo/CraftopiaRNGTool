namespace CraftopiaRNGTool
{
    public class TreasureData2
    {
        public int IslandLv { get; set; }
        public int Seed { get; set; }
        public int WSeed { get; set; }
        public string SeedChar { get; set; }
        public int MapId { get; set; }
        public int SCount { get; set; }
        //public int Count { get; set; }

        public string Item1 { get; set; }
        public string Enchant1_1 { get; set; }
        public string Enchant1_2 { get; set; }
        public string Enchant1_3 { get; set; }
        
        private ItemData Item;

        public TreasureData2()
        {
        }

        public TreasureData2(int islandLv, int seed, int wseed, int mapId, int sCount, int count, ItemData item)
        {
            IslandLv = islandLv;
            Seed = seed;
            WSeed = wseed;
            MapId = mapId;
            SCount = sCount;
            //Count = count;
            Item = item;
            SeedChar = Form1.charList.Length > wseed ? Form1.charList[wseed] : "";
            SetData(item);
        }

        private void SetData(ItemData item)
        {
            Item1 = item.Name;

            Enchant1_1 = SetEnchant(item, 0);
            Enchant1_2 = SetEnchant(item, 1);
            Enchant1_3 = SetEnchant(item, 2);
        }

        private string SetEnchant(ItemData item, int num)
        {
            EnchantData[] encha = item.Enchant;
            if (encha != null && encha.Length > num && encha[num] != null)
            {
                return encha[num].Name;
            }
            return "";
        }
    }
}
