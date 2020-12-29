using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace CraftopiaRNGTool
{
    public static class TreasureCalc
    {
        private static readonly string otherName = "other_data.txt";
        private static readonly string rarityProbName = "rarity_prob.txt";
        private static readonly string rarityArrayName = "rarity_array_";
        private static readonly string rarityChashName = "rarity_cash_";
        private static readonly string enchantRarityName = "enchant_rarity_";
        private static readonly string itemRarityName = "item_rarity_";

        public static ItemData[][] data_Item;
        public static EnchantData[][] data_Enchant;
        private static float[] probs_Item;
        private static float[] probs_Enchant;
        public static int[][][] rarityArray_Item;
        private static int[][][] rarityArray_Enchant;
        private static int[][] rarityCash_Item;
        private static int[][] rarityCash_Enchant;

        private static int max_Rarity_Item = 14;
        private static int max_Rarity_Enchant = 4;
        private static int max_TreasureType = 3;
        public static int max_IslandLevel = 7;
        public static int max_MapCell = 11;

        public static void SetMaxData()
        {
            string fileName = Form1.dataDirName + otherName;
            string[] strs = TryReadFile(fileName);
            string[] texts = strs[0].Split(',');
            if (texts.Length < 5) return;

            max_Rarity_Item = IntParse(texts[0]);
            max_Rarity_Enchant = IntParse(texts[1]);
            max_TreasureType = IntParse(texts[2]);
            max_IslandLevel = IntParse(texts[3]);
            max_MapCell = IntParse(texts[4]);
        }

        public static void SetDatas()
        {
            SetItemData();
            SetEnchantData();
            SetRarityProbs();
            rarityArray_Item = SetRarityArray_Sub("item");
            rarityArray_Enchant = SetRarityArray_Sub("enchant");
            rarityCash_Item = SetRarityCash_Sub("item");
            rarityCash_Enchant = SetRarityCash_Sub("enchant");
        }

        private static void SetRarityProbs()
        {
            string fileName = Form1.dataDirName + rarityProbName;
            float[][] probs = new float[2][];
            string[] strs = TryReadFile(fileName);
            if (strs.Length < 2) return;

            for (int i = 0; i < 2; i++)
            {
                string[] texts = strs[i].Split(',');
                probs[i] = new float[texts.Length];
                for (int j = 0; j < texts.Length; j++)
                {
                    probs[i][j] = IntParse(texts[j]);
                }
            }
            probs_Item = probs[0];
            probs_Enchant = probs[1];
        }

        private static int[][][] SetRarityArray_Sub(string name)
        {
            string fileName = Form1.dataDirName + rarityArrayName + name + ".txt";
            string[] strs = TryReadFile(fileName);
            int num = strs.Length / max_TreasureType;
            int[][][] rarityArray = new int[num][][];
            if (strs.Length % max_TreasureType != 0)
            {
                return null;
            }

            for (int i = 0; i < strs.Length; i++)
            {
                int num2 = i / 3;
                int num3 = i % 3;
                string[] texts = strs[i].Split(',');

                if (num3 == 0)
                {
                    rarityArray[num2] = new int[max_TreasureType][];
                }
                rarityArray[num2][num3] = new int[texts.Length];

                for (int j = 0; j < texts.Length; j++)
                {
                    rarityArray[num2][num3][j] = IntParse(texts[j]);
                }
            }

            return rarityArray;
        }
        private static int[][] SetRarityCash_Sub(string name)
        {
            string fileName = Form1.dataDirName + rarityChashName + name + ".txt";
            string[] strs = TryReadFile(fileName);
            int[][] rarityCash = new int[strs.Length][];

            for (int i = 0; i < strs.Length; i++)
            {
                string[] texts = strs[i].Split(',');
                rarityCash[i] = new int[texts.Length];
                for (int j = 0; j < texts.Length; j++)
                {
                    rarityCash[i][j] = IntParse(texts[j]);
                }
            }

            return rarityCash;
        }

        private static void SetItemData()
        {
            data_Item = new ItemData[max_Rarity_Item][];
            for (int i = 0; i < max_Rarity_Item; i++)
            {
                string name = $"{(i + 1):00}" + ".txt";
                string fileName = Form1.dataDirName + itemRarityName + name;
                string[] strs = TryReadFile(fileName);
                data_Item[i] = new ItemData[strs.Length];
                for (int j = 0; j < strs.Length; j++)
                {
                    data_Item[i][j] = new ItemData(strs[j].Split(','));
                }
            }
        }
        private static void SetEnchantData()
        {
            data_Enchant = new EnchantData[max_Rarity_Enchant][];
            for (int i = 0; i < max_Rarity_Enchant; i++)
            {
                string name = $"{(i + 1):00}" + ".txt";
                string fileName = Form1.dataDirName + enchantRarityName + name;
                string[] strs = TryReadFile(fileName);
                data_Enchant[i] = new EnchantData[strs.Length];
                for (int j = 0; j < strs.Length; j++)
                {
                    data_Enchant[i][j] = new EnchantData(strs[j].Split(','));
                }
            }
        }

        public static int GetServerSeed(char c)
        {
            UnityEngine.Random.InitState(c);
            int rand = UnityEngine.Random.Range(0, 255);
            return rand;
        }

        private static int GetRarity(int islandLevel, int rarity, int[][] rarityCash, int[][][] rarityArray)
        {
            int rand = UnityEngine.Random.Range(0, rarityCash[islandLevel][rarity]);
            int itemtRarity = 0;
            int num = 0;
            for (int i = 0; i < rarityArray[islandLevel][rarity].Length; i++)
            {
                int num2 = rarityArray[islandLevel][rarity][i];
                if (num2 > 0)
                {
                    num += num2;
                    if (rand < num)
                    {
                        itemtRarity = i;
                        break;
                    }
                }
            }

            return itemtRarity;
        }

        private static ItemData GetItem(int itemRarity)
        {
            float rarityProbsSum = probs_Item[itemRarity];
            if (rarityProbsSum <= 0f)
            {
                return new ItemData();
            }
            float rand = UnityEngine.Random.Range(0f, rarityProbsSum);
            float num = 0f;
            for (int i = 0; i < data_Item[itemRarity].Length; i++)
            {
                num += data_Item[itemRarity][i].Prob;
                if (num >= rand)
                {
                    return new ItemData(data_Item[itemRarity][i]);
                }
            }
            return new ItemData();
        }
        private static EnchantData GetEnchant(int enchantRarity)
        {
            float rarityProbsSum = probs_Enchant[enchantRarity];
            if (rarityProbsSum <= 0f)
            {
                return new EnchantData();
            }
            float rand = UnityEngine.Random.Range(0f, rarityProbsSum);
            float num = 0f;
            for (int i = 0; i < data_Enchant[enchantRarity].Length; i++)
            {
                if (data_Enchant[enchantRarity][i].IsDrop)
                {
                    num += data_Enchant[enchantRarity][i].Prob;
                    if (num >= rand)
                    {
                        return data_Enchant[enchantRarity][i];
                    }
                }
            }

            return new EnchantData();
        }

        private static ItemData GetItemData(int islandLevel, int rarity)
        {
            int itemRarity = GetRarity(islandLevel, rarity, rarityCash_Item, rarityArray_Item);
            ItemData item = GetItem(itemRarity);
            //ItemType.Equipment : ItemType.Material : ItemType.Consumption
            if (item.Type == "Equipment" || item.Type == "Material" || item.Type == "Consumption")
            {
                int enchantCount = 1;
                float value = UnityEngine.Random.value;
                if (value < 0.05f)
                {
                    enchantCount = 3;
                }
                else if (value < 0.2f)
                {
                    enchantCount = 2;
                }
                item.Enchant = new EnchantData[enchantCount];
                for (int j = 0; j < enchantCount; j++)
                {
                    int enchantRarity = GetRarity(islandLevel, rarity, rarityCash_Enchant, rarityArray_Enchant);
                    EnchantData enchant = GetEnchant(enchantRarity);
                    item.Enchant[j] = enchant;
                }
            }

            return item;
        }

        public static ItemData[] GetTreasureItem(int islandLevel, int seed, int rarity)
        {
            UnityEngine.Random.InitState(seed);
            float rand = UnityEngine.Random.Range(0f, 1f);
            int itemCount = 1;
            if (rand < 0.05f)
            {
                itemCount = 4;
            }
            else if (rand < 0.2f)
            {
                itemCount = 3;
            }
            else if (rand < 0.65f)
            {
                itemCount = 2;
            }
            ItemData[] treasureItem = new ItemData[itemCount];

            for (int i = 0; i < itemCount; i++)
            {
                treasureItem[i] = GetItemData(islandLevel, rarity);

                //乱数消費
                RandSyouhi(5);
            }

            return treasureItem;
        }

        public static ItemData[] GetMerchantItem(int islandLevel, int count)
        {
            ItemData[] itemDatas = new ItemData[count];
            for (int i = 0; i < count; i++)
            {
                itemDatas[i] = GetItemData(islandLevel, 0);
            }

            RandSyouhi(1);

            return itemDatas;
        }

        public static void RandSyouhi(int value)
        {
            for (int i = 0; i < value; i++)
            {
                UnityEngine.Random.Range(0, 1);
            }
        }
        
        public static int IntParse(string str)
        {
            if (Int32.TryParse(str, out int i))
            {
                return i;
            }

            return 0;
        }
        public static string[] TryReadFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                string[] strs = File.ReadAllLines(fileName, Encoding.UTF8);
                return strs;
            }
            else
            {
                Form1.nonFileNames.Add(fileName);
                return new string[] { "" };
            }
        }

    }
}
