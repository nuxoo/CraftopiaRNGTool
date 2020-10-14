namespace CraftopiaRNGTool
{
    public class TreasureData
    {
        public int IslandLv { get; set; }
        public int Seed { get; set; }
        public int WSeed { get; set; }
        public string SeedChar { get; set; }
        public int MapId { get; set; }
        public int PosHash { get; set; }
        public int Rarity { get; set; }
        public string Memo { get; set; }
        public string X { get; set; }
        public string Z { get; set; }
        public string Y { get; set; }
        public ItemData[] Items { get; set; }

        public string Item1 { get; set; }
        public string Item2 { get; set; }
        public string Item3 { get; set; }
        public string Item4 { get; set; }
        public string Enchant1_1 { get; set; }
        public string Enchant1_2 { get; set; }
        public string Enchant1_3 { get; set; }
        public string Enchant2_1 { get; set; }
        public string Enchant2_2 { get; set; }
        public string Enchant2_3 { get; set; }
        public string Enchant3_1 { get; set; }
        public string Enchant3_2 { get; set; }
        public string Enchant3_3 { get; set; }
        public string Enchant4_1 { get; set; }
        public string Enchant4_2 { get; set; }
        public string Enchant4_3 { get; set; }

        public TreasureData()
        {
        }

        public TreasureData(int islandLv, int seed, int wseed, int mapId, int rarity, TreasureBoxData treBox, ItemData[] items, int mode)
        {
            Seed = seed;
            WSeed = wseed;
            IslandLv = islandLv;
            MapId = mapId;
            PosHash = treBox.PosHash;
            Rarity = rarity;
            Items = items;
            SeedChar = Form1.charList.Length > wseed ? Form1.charList[wseed] : "";
            Memo = treBox.Memo;
            X = treBox.X;
            Z = treBox.Z;
            Y = treBox.Y;
            SetData(items);
        }

        private void SetData(ItemData[] items)
        {
            Item1 = SetItem(items, 0);
            Item2 = SetItem(items, 1);
            Item3 = SetItem(items, 2);
            Item4 = SetItem(items, 3);
            Enchant1_1 = SetEnchant(items, 0, 0);
            Enchant1_2 = SetEnchant(items, 0, 1);
            Enchant1_3 = SetEnchant(items, 0, 2);
            Enchant2_1 = SetEnchant(items, 1, 0);
            Enchant2_2 = SetEnchant(items, 1, 1);
            Enchant2_3 = SetEnchant(items, 1, 2);
            Enchant3_1 = SetEnchant(items, 2, 0);
            Enchant3_2 = SetEnchant(items, 2, 1);
            Enchant3_3 = SetEnchant(items, 2, 2);
            Enchant4_1 = SetEnchant(items, 3, 0);
            Enchant4_2 = SetEnchant(items, 3, 1);
            Enchant4_3 = SetEnchant(items, 3, 2);
        }

        private string SetItem(ItemData[] items, int num)
        {
            if (items != null && items.Length > num && items[num] != null)
            {
                return items[num].Name;
            }
            return "";
        }

        private string SetEnchant(ItemData[] items, int num, int num2)
        {
            if (items != null && items.Length > num && items[num] != null)
            {
                EnchantData[] encha = items[num].Enchant;
                if (encha != null && encha.Length > num2 && encha[num2] != null)
                {
                    return encha[num2].Name;
                }
            }
            return "";
        }
    }
}
