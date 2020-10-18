using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace CraftopiaRNGTool
{
    public partial class Form1 : Form
    {
        public static string dirName;
        public static string dataDirName;
        private  static string dataFolderName = "data";
        private static string treListFolderName = "data_treasure";
        private static string treDunListFolderName = "data_treasure_dungeon";
        private static string iniName = "CraftopiaRNGTool.ini";

        public static string favoListName = "CraftopiaRNGTool_favorite.ini";
        public static string charListName = "list_char_seed.txt";
        private static string mapIdListName = "list_map_id.txt";
        private static string dungeonListName = "list_dungeon_id.txt";
        private static string searchListName = "CraftopiaRNGTool_search.ini";

        private int[][] mapIds;
        private List<int>[] dungeonIds;
        public static string[] favoList;
        public static string[] charList;
        private TreasureBoxData[][] treBoxList;
        private TreasureBoxData[][] treDunBoxList;

        public static List<string> nonFileNames = new List<string>();
        private static bool isError = false;
        private bool changeFlag = false;
        private RadioButton[] radioButtons1;
        private RadioButton[] radioButtons2;

        public Form1()
        {
            try
            {
                this.FormClosed += Form1_FormClosed;
                this.FormClosing += Form1_FormClosing;
                InitializeComponent();
                dirName = Application.StartupPath + "\\";
                dataDirName = dirName + dataFolderName + "\\";
                TreasureCalc.SetMaxData();
                TreasureCalc.SetDatas();
                SetObjects();
                SetMapIds();
                SetDungeonIds();
                SetTreBoxList();
                SetTreDunBoxList();
                ShowNonFile();
            }
            catch (Exception ex) { ErrorEvent("初期化に失敗しました", ex); }

            try
            {
                ReadIni();
            }
            catch (Exception ex) { ErrorEvent("iniの読み込みに失敗しました", ex); }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isError)
            {
                try
                {
                    WriteIni();
                }
                catch (Exception ex) { ErrorEvent("iniの書き込みに失敗しました", ex); }
            }
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.isExit = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Program.isLoading = false;
        }
        
        private void Search_Click(object sender, EventArgs e)
        {
            try
            {
                Search();
            }
            catch (Exception ex) { ErrorEvent("検索に失敗しました", ex); }
        }

        private void Add_Item_Click(object sender, EventArgs e)
        {
            Form_Add_Item form = new Form_Add_Item(listBox1, true);
            form.ShowDialog();
        }
        private void Add_Equipment_Click(object sender, EventArgs e)
        {
            Form_Add_Equipment form = new Form_Add_Equipment(listBox1, true);
            form.ShowDialog();
        }

        private void List_Up_Click(object sender, EventArgs e)
        {
            ButtonEvent_Up(listBox1);
        }
        private void List_Down_Click(object sender, EventArgs e)
        {
            ButtonEvent_Down(listBox1);
        }
        private void List_Edit_Click(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index >= 0)
            {
                string str = "" + listBox1.Items[index];
                if (str == "") str = " ";

                string[] strs = str.Split(',');
                if (strs.Length < 2)
                {
                    Form_Add_Item form = new Form_Add_Item(listBox1, false);
                    form.ShowDialog();
                }
                else
                {
                    Form_Add_Equipment form = new Form_Add_Equipment(listBox1, false);
                    form.ShowDialog();
                }
            }
        }
        private void List_Delete_Click(object sender, EventArgs e)
        {
            ButtonEvent_Delete(listBox1);
        }
        private void List_TextEdit_Click(object sender, EventArgs e)
        {
            Form_SearchText form = new Form_SearchText(listBox1);
            form.ShowDialog();
        }

        private void FavoEdit_Click(object sender, EventArgs e)
        {
            try
            {
                Form_FavoList form = new Form_FavoList();
                form.ShowDialog();
                UI_SetFavoList();
            }
            catch (Exception ex) { ErrorEvent("エラーが発生しました", ex); }
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            Form_GetCharList form = new Form_GetCharList();
            form.ShowDialog();
        }

        private void WorldSeed_TextChanged(object sender, EventArgs e)
        {
            string text = worldSeedText.Text;
            if (text.Length >= 1 && !changeFlag)
            {
                changeFlag = true;
                worldSeedNum.Value = TreasureCalc.GetServerSeed(text[0]);
                changeFlag = false;
            }
        }
        private void WorldSeed_ValueChanged(object sender, EventArgs e)
        {
            int value = (int)worldSeedNum.Value;
            if (value < charList.Length && !changeFlag)
            {
                changeFlag = true;
                worldSeedText.Text = charList[value];
                changeFlag = false;
            }
        }

        private void MapIdNum1_ValueChanged(object sender, EventArgs e)
        {
            UI_SetMapId();
        }
        private void MapIdNum2_ValueChanged(object sender, EventArgs e)
        {
            UI_SetMapId();
        }
        private void MapIdNum3_ValueChanged(object sender, EventArgs e)
        {
            int id = (int)mapIdNum3.Value;
            int x = id % TreasureCalc.max_MapCell + 1;
            int y = id / TreasureCalc.max_MapCell + 1;

            if (!changeFlag)
            {
                changeFlag = true;
                mapIdNum1.Value = x;
                mapIdNum2.Value = TreasureCalc.max_MapCell - y + 1;
                changeFlag = false;
            }
        }

        public static void ButtonEvent_Up(ListBox box)
        {
            int index = box.SelectedIndex;
            if (index >= 1)
            {
                object obj = box.Items[index - 1];
                box.Items[index - 1] = box.Items[index];
                box.Items[index] = obj;
                box.SelectedIndex -= 1;
            }
        }
        public static void ButtonEvent_Down(ListBox box)
        {
            int index = box.SelectedIndex;
            if (box.Items.Count - index >= 2)
            {
                object obj = box.Items[index + 1];
                box.Items[index + 1] = box.Items[index];
                box.Items[index] = obj;
                box.SelectedIndex += 1;
            }
        }
        public static void ButtonEvent_Delete(ListBox box)
        {
            int index = box.SelectedIndex;
            if (index >= 0)
            {
                box.Items.RemoveAt(index);
                if (box.Items.Count >= 1)
                {
                    box.SelectedIndex = index < box.Items.Count ? index : index - 1;
                }
            }
        }
        
        private void UI_SetMapId()
        {
            int x = (int)mapIdNum1.Value - 1;
            int y = TreasureCalc.max_MapCell - (int)mapIdNum2.Value;
            int id = x + y * TreasureCalc.max_MapCell;

            if (!changeFlag)
            {
                changeFlag = true;
                mapIdNum3.Value = id;
                changeFlag = false;
            }
        }
        private void UI_SetFavoList()
        {
            int count = favoList.Length;
            if (favoList[0] == "" && favoList.Length == 1) count = 0;
            label3.Text = count + "個";
        }

        private int GetRadioButtonChecked(RadioButton[] radio)
        {
            for (int i = 0; i < radio.Length; i++)
            {
                if (radio[i].Checked) return i;
            }

            return 0;
        }
        private void SetRadioButtonChecked(RadioButton[] radio, string str)
        {
            int id = TreasureCalc.IntParse(str);
            if (id < radio.Length)
            {
                radio[id].Checked = true;
            }
        }

        private void SetcomboBoxSelected(ComboBox combo, string str)
        {
            int index = TreasureCalc.IntParse(str);
            int count = combo.Items.Count - 1;
            if (index > count) index = count;
            combo.SelectedIndex = index;
        }

        public static void ErrorEvent(string mes, Exception ex)
        {
            isError = true;
            MessageBox.Show(mes + "\n" + ex.GetType() + "\n" + ex.Message + "\n" + ex.StackTrace);
            Application.Exit();
        }

        private void ShowNonFile()
        {
            if (nonFileNames.Count > 0)
            {
                string text = "";
                foreach (string str in nonFileNames)
                {
                    text += str + "\n";
                }

                text += "が見つかりませんでした";
                nonFileNames = new List<string>();
                MessageBox.Show(text);
            }
        }
        
        public static object[] GetItemList(bool mode)
        {
            List<ItemData> list = new List<ItemData>();
            int index = Program.form1.comboBox1.SelectedIndex;
            int index2 = Program.form1.comboBox2.SelectedIndex;
            int[] vs = TreasureCalc.rarityArray_Item[index][index2];
            for (int i = 0; i < TreasureCalc.data_Item.Length; i++)
            {
                if (vs[i] > 0)
                {
                    foreach (ItemData item in TreasureCalc.data_Item[i])
                    {
                        if (!mode || item.Type == "Equipment" || item.Type == "Material" || item.Type == "Consumption")
                        {
                            list.Add(item);
                        }
                    }
                }
            }

            list.Sort((a, b) => a.Id - b.Id);
            object[] objs = new object[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                objs[i] = list[i].Name;
            }

            return objs;
        }

        private void WriteIni()
        {
            object[] strs = new object[]
            {
                checkBox1.Checked,
                checkBox2.Checked,
                checkBox3.Checked,
                worldSeedText.Text,
                mapIdNum3.Value,
                GetRadioButtonChecked(radioButtons1),
                GetRadioButtonChecked(radioButtons2),
                comboBox1.SelectedIndex,
                comboBox2.SelectedIndex,
                comboBox3.SelectedIndex,
                numericUpDown1.Value,
                numericUpDown2.Value,
                comboBox4.SelectedIndex
            };

            File.WriteAllText(dirName + iniName, string.Join("\n", strs));
            WriteSearchText(dirName + searchListName);
        }
        private void ReadIni()
        {
            string fileName = dirName + iniName;
            if (File.Exists(fileName))
            {
                string[] strs = File.ReadAllLines(fileName, Encoding.UTF8);
                if (strs.Length < 12) return;

                checkBox1.Checked = strs[0] == "True";
                checkBox2.Checked = strs[1] == "True";
                checkBox3.Checked = strs[2] == "True";
                worldSeedText.Text = strs[3];
                mapIdNum3.Text = strs[4];
                SetRadioButtonChecked(radioButtons1, strs[5]);
                SetRadioButtonChecked(radioButtons2, strs[6]);
                SetcomboBoxSelected(comboBox1, strs[7]);
                SetcomboBoxSelected(comboBox2, strs[8]);
                SetcomboBoxSelected(comboBox3, strs[9]);
                numericUpDown1.Text = strs[10];
                numericUpDown2.Text = strs[11];
                if (strs.Length > 12) SetcomboBoxSelected(comboBox4, strs[12]);
            }

            ReadSearchText(dirName + searchListName);
        }
        
        private void WriteSearchText(string fileName)
        {
            List<string> strs = new List<string>();
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                strs.Add(listBox1.Items[i].ToString());
            }

            File.WriteAllLines(fileName, strs);
        }
        private void ReadSearchText(string fileName)
        {
            if (!File.Exists(fileName)) return;

            string[] strs = TreasureCalc.TryReadFile(fileName);
            object[] objs = new object[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                objs[i] = strs[i];
            }
            listBox1.Items.AddRange(objs);
        }

        private void SetMapIds()
        {
            string[] texts = TreasureCalc.TryReadFile(dataDirName + mapIdListName);
            if (texts == null) return;
            int[][] vs = new int[texts.Length][];

            for (int i = 0; i < texts.Length; i++)
            {
                if (texts[i] == "") continue;
                string[] strs = texts[i].Split(',');
                vs[i] = new int[strs.Length];
                for (int j = 0; j < strs.Length; j++)
                {
                    vs[i][j] = TreasureCalc.IntParse(strs[j]);
                }
            }

            mapIds = vs;
        }
        private void SetDungeonIds()
        {
            string[] texts = TreasureCalc.TryReadFile(dataDirName + dungeonListName);
            if (texts == null) return;
            List<int>[] vs = new List<int>[texts.Length];

            for (int i = 0; i < texts.Length; i++)
            {
                if (texts[i] == "") continue;
                string[] strs = texts[i].Split(',');
                vs[i] = new List<int>();
                for (int j = 0; j < strs.Length; j++)
                {
                    vs[i].Add(TreasureCalc.IntParse(strs[j]));
                }
            }

            dungeonIds = vs;
        }
        
        private void SetObjects()
        {
            mapIdNum1.Maximum = TreasureCalc.max_MapCell;
            mapIdNum2.Maximum = TreasureCalc.max_MapCell;
            mapIdNum3.Maximum = TreasureCalc.max_MapCell * TreasureCalc.max_MapCell - 1;

            for (int i = 0; i < TreasureCalc.max_IslandLevel + 1; i++)
            {
                comboBox1.Items.Add(i);
            }
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;

            favoList = TreasureCalc.TryReadFile(dirName + favoListName);
            UI_SetFavoList();
            charList = TreasureCalc.TryReadFile(dataDirName + charListName);
            
            radioButtons1 = new RadioButton[] {
                radioButton0, radioButton1, radioButton2, radioButton3, radioButton7
            };
            radioButtons2 = new RadioButton[] {
                radioButton4, radioButton5, radioButton6
            };
        }

        private void Search()
        {
            List<TreasureData> treDatas = new List<TreasureData>();
            int islandLevel = comboBox1.SelectedIndex;
            int treType = comboBox2.SelectedIndex;
            int radioValue1 = GetRadioButtonChecked(radioButtons1);
            int radioValue2 = GetRadioButtonChecked(radioButtons2);
            bool isAnd = radioValue2 == 1;
            bool isAll = false;
            TreasureBoxData[] treBoxs = GetTreList(radioValue1, treType);
            int[][] searchValue = GetSearchValue(radioValue2);
            int iValue = 0, iLength;
            int[] jValue = mapIds[islandLevel];
            int maxCount = (int)numericUpDown2.Value;
            int count = 0;
            bool isZero = islandLevel == 0;

            if (checkBox1.Checked && checkBox2.Checked)
            {
                isAll = true;
                int min = jValue[0];
                int max = jValue[jValue.Length - 1];
                iLength = max - min + 255;
                jValue = new int[] { min };
            }
            else
            {
                if (checkBox1.Checked)
                {
                    iLength = 255;
                }
                else
                {
                    iValue = (int)worldSeedNum.Value;
                    iLength = iValue + 1;
                }
                if (!checkBox2.Checked)
                {
                    jValue = new int[] { (int)mapIdNum3.Value };
                }
            }
            
            for (int j = 0; j < jValue.Length; j++)
            {
                for (int i = iValue; i < iLength; i++)
                {
                    for (int k = 0; k < treBoxs.Length; k++)
                    {
                        int seed = i + jValue[j];
                        TreasureBoxData treBox = treBoxs[k];
                        ItemData[] items = TreasureCalc.GetTreasureItem(islandLevel, seed + treBox.PosHash, treType);

                        if (Search_IsHit(items, searchValue, isAnd))
                        {
                            int wSeed, iId;
                            if (isAll)
                            {
                                iId = GetSeedToId(seed, islandLevel);
                                wSeed = seed - iId;
                            }
                            else
                            {
                                wSeed = i;
                                iId = jValue[j];
                            }

                            if (treBox.IsDungeon && treBox.Id != 9)
                            {
                                TreasureBoxData tre = GetIsDungeonMemo(treBox, seed, isZero);
                                if (tre == null) continue;
                                treBox = tre;
                            }

                            TreasureData treData = new TreasureData(islandLevel, seed, wSeed, iId, treType, treBox, items, radioValue1);
                            treDatas.Add(treData);

                            count++;
                            if (count >= maxCount)
                            {
                                i = iLength;
                                j = jValue.Length;
                                break;
                            }
                        }
                    }
                }
            }

            Form_Result form = new Form_Result(treDatas, radioValue1);
            form.Show();
        }

        private int[][] GetSearchValue(int value)
        {
            if (value == 0) return null;
            int count = listBox1.Items.Count;
            List<int[]> vs = new List<int[]>();

            for (int i = 0; i < count; i++)
            {
                string str = listBox1.Items[i].ToString();
                if (str == "") continue;
                string[] strs = str.Split(',');
                int[] v = new int[strs.Length];
                v[0] = GetItemId(strs[0]);
                if (strs.Length >= 2) v[1] = TreasureCalc.IntParse(strs[1]);
                for (int j = 2; j < strs.Length; j++)
                {
                    v[j] = GetEnchantId(strs[j]);
                }
                vs.Add(v);
            }

            return vs.ToArray(); ;
        }
        private bool Search_IsHit(ItemData[] items, int[][] searchValue, bool isAnd)
        {
            if (searchValue == null) return true;
            if (isAnd)
            {
                for (int i = 0; i < searchValue.Length; i++)
                {
                    bool check = false;
                    foreach (ItemData item in items)
                    {
                        if (Searh_IsHit_Sub(item, searchValue[i]))
                        {
                            check = true;
                            break;
                        }
                    }
                    if (!check) return false;
                }

                return true;
            }
            else
            {
                foreach (ItemData item in items)
                {
                    for (int i = 0; i < searchValue.Length; i++)
                    {
                        if (Searh_IsHit_Sub(item, searchValue[i]))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }
        private bool Searh_IsHit_Sub(ItemData item, int[] values)
        {
            if (values[0] != item.Id) return false;
            if (values.Length < 2) return true;

            EnchantData[] enchants = item.Enchant;
            if (enchants == null)
            {
                if (values[1] == 0) return true;
                return false;
            }
            if (values[1] != enchants.Length) return false;

            int count = 0;
            foreach (EnchantData enchant in enchants)
            {
                for (int j = 2; j < values.Length; j++)
                {
                    if (values[j] == enchant.Id)
                    {
                        count++;
                        break;
                    }
                }
            }

            if (values[1] > values.Length - 2)
            {
                if (values.Length - 2 <= count) return true;
                return false;
            }
            if (values[1] <= count) return true;

            return false;
        }

        private TreasureBoxData GetIsDungeonMemo(TreasureBoxData treBox, int seed, bool isZero)
        {
            string memo = "";
            if (isZero)
            {
                UnityEngine.Random.InitState(seed);
                int rand = UnityEngine.Random.Range(0, 2);
                if (rand == treBox.Index)
                {
                    return null;
                }
            }
            else
            {
                for (int m = 3; m <= 5; m++)
                {
                    UnityEngine.Random.InitState(seed);
                    int rand = UnityEngine.Random.Range(0, m);
                    if (rand != treBox.Index && m > treBox.Index)
                    {
                        memo += "" + m;
                    }
                }

                if (memo == "") return null;
                if (memo == "345") memo = "All";
                memo += "-";
            }

            return new TreasureBoxData(treBox, treBox.Index + "-" + memo + treBox.Memo);
        }

        private int GetItemId(string name)
        {
            foreach (ItemData[] items in TreasureCalc.data_Item)
            {
                foreach (ItemData item in items)
                {
                    if (name == item.Name)
                    {
                        return item.Id;
                    }
                }
            }

            return -1;
        }
        private int GetEnchantId(string name)
        {
            foreach (EnchantData[] enchants in TreasureCalc.data_Enchant)
            {
                foreach (EnchantData enchant in enchants)
                {
                    if (name == enchant.Name)
                    {
                        return enchant.Id;
                    }
                }
            }

            return -1;
        }

        private int GetSeedToId(int seed, int islandLv)
        {
            if (islandLv != 7)
            {
                int[] vs = mapIds[islandLv];
                int max = vs[vs.Length - 1];
                if (max <= seed)
                {
                    return max;
                }
                else
                {
                    return vs[0];
                }
            }
            else
            {
                int num = 117, num2 = 33;
                if (num + 254 <= seed) return seed - 254;
                if (num2 > seed) return seed - (seed % TreasureCalc.max_MapCell);

                if (num <= seed)
                {
                    return num;
                }
                else
                {
                    return num2;
                }
            }
        }

        private void SetTreBoxList()
        {
            List<TreasureBoxData[]> datas = new List<TreasureBoxData[]>();
            string dir = dirName + treListFolderName;
            if (!Directory.Exists(dir))
            {
                nonFileNames.Add(dir);
                return;
            }
            string[] files = Directory.GetFiles(dir, "*.txt", SearchOption.AllDirectories);
            if (files == null) return;
            string[] fileNames = new string[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                datas.Add(SetTreBoxList_Sub(files[i]));
                fileNames[i] = Path.GetFileNameWithoutExtension(files[i]);
            }

            treBoxList = datas.ToArray();
            comboBox3.Items.AddRange(fileNames);
            comboBox3.SelectedIndex = 0;
        }
        private TreasureBoxData[] SetTreBoxList_Sub(string fileName)
        {
            string[] strs = TreasureCalc.TryReadFile(fileName);
            if (strs == null) return null;
            TreasureBoxData[] treBoxs = new TreasureBoxData[strs.Length];
            string name = Path.GetFileNameWithoutExtension(fileName);

            for (int i = 0; i < strs.Length; i++)
            {
                if (strs[i] == "" || strs[i] == null)
                {
                    treBoxs[i] = new TreasureBoxData();
                    continue;
                }
                string[] s = strs[i].Split(',');
                if (s.Length < 7)
                {
                    treBoxs[i] = new TreasureBoxData();
                    continue;
                }

                double y;
                if (Double.TryParse(s[4], out y) && y < -5000)
                {
                    int index = (int)((y + 5000) / -10000);
                    treBoxs[i] = new TreasureBoxData(s, name, index);
                }
                else
                {
                    treBoxs[i] = new TreasureBoxData(s, name);
                }
            }

            return treBoxs;
        }

        private void SetTreDunBoxList()
        {
            List<TreasureBoxData[]> datas = new List<TreasureBoxData[]>();
            string dir = dirName + treDunListFolderName;
            if (!Directory.Exists(dir))
            {
                nonFileNames.Add(dir);
                return;
            }
            string[] files = Directory.GetFiles(dir, "*.txt", SearchOption.AllDirectories);
            if (files == null) return;
            string[] fileNames = new string[files.Length];

            for (int i = 0; i < files.Length; i++)
            {
                datas.Add(SetTreBoxList_Sub(files[i]));
                fileNames[i] = Path.GetFileNameWithoutExtension(files[i]);
            }

            treDunBoxList = datas.ToArray();
            comboBox4.Items.AddRange(fileNames);
            comboBox4.SelectedIndex = 0;
        }
        
        private TreasureBoxData[] GetTreList(int value, int rarity)
        {
            TreasureBoxData[] treBoxs;
            if (value == 0)
            {
                List<TreasureBoxData> list = GetAllTreBox(rarity);
                list.AddRange(GetAllDunTreBox(rarity));
                treBoxs = list.ToArray();
            }
            else if (value == 1)
            {
                treBoxs = GetFavoListTreBoxs();
            }
            else if (value == 2)
            {
                treBoxs = GetIslandTreBox(rarity);
            }
            else if (value == 4)
            {
                treBoxs = GetDungeonTreBox(rarity);
            }
            else
            {
                treBoxs = new TreasureBoxData[] { new TreasureBoxData((int)numericUpDown1.Value) };
            }

            return treBoxs;
        }
        
        private TreasureBoxData[] GetFavoListTreBoxs()
        {
            TreasureBoxData[] treBoxs = new TreasureBoxData[favoList.Length];
            for (int i = 0; i < favoList.Length; i++)
            {
                string[] strs = new string[2];
                if (favoList[i] == "")
                {
                    treBoxs[i] = new TreasureBoxData();
                    continue;
                }
                string[] s = favoList[i].Split(',');
                if (s.Length < 2)
                {
                    strs[1] = "";
                }
                else
                {
                    strs[1] = s[1];
                }
                strs[0] = s[0];
                treBoxs[i] = new TreasureBoxData(strs);
            }

            return treBoxs;
        }

        private List<TreasureBoxData> GetAllTreBox(int rarity)
        {
            List<TreasureBoxData> list = new List<TreasureBoxData>();
            bool isZero = comboBox1.SelectedIndex == 0;

            for (int i = 0; i < treBoxList.Length; i++)
            {
                for (int j = 0; j < treBoxList[i].Length; j++)
                {
                    TreasureBoxData treBox = treBoxList[i][j];
                    if (rarity == treBox.Rarity)
                    {
                        if (isZero == (treBox.Id == 8))
                        {
                            list.Add(treBox);
                        }
                    }
                }
            }

            return list;
        }
        private List<TreasureBoxData> GetAllDunTreBox(int rarity)
        {
            List<TreasureBoxData> list = new List<TreasureBoxData>();
            int index = comboBox1.SelectedIndex;
            bool isZero = index == 0;

            for (int i = 0; i < treDunBoxList.Length; i++)
            {
                for (int j = 0; j < treDunBoxList[i].Length; j++)
                {
                    TreasureBoxData treBox = treDunBoxList[i][j];
                    if (rarity == treBox.Rarity)
                    {
                        if (isZero)
                        {
                            Double.TryParse(treBox.Y, out double y);
                            if (y < -25000) continue;
                        }

                        if (dungeonIds[index].IndexOf(treBox.Id) >= 0)
                        {
                            list.Add(treBox);
                        }
                    }
                }
            }

            return list;
        }

        private TreasureBoxData[] GetIslandTreBox(int rarity)
        {
            int index = comboBox3.SelectedIndex - 1;
            if (index >= 0)
            {
                List<TreasureBoxData> list = new List<TreasureBoxData>();
                foreach (TreasureBoxData treBox in treBoxList[index])
                {
                    if (rarity == treBox.Rarity) list.Add(treBox);
                }
                return list.ToArray();
            }
            else
            {
                return GetAllTreBox(rarity).ToArray();
            }
        }
        private TreasureBoxData[] GetDungeonTreBox(int rarity)
        {
            int index = comboBox4.SelectedIndex - 1;
            if (index >= 0)
            {
                List<TreasureBoxData> list = new List<TreasureBoxData>();
                foreach (TreasureBoxData treBox in treDunBoxList[index])
                {
                    if (rarity == treBox.Rarity) list.Add(treBox);
                }
                return list.ToArray();
            }
            else
            {
                return GetAllDunTreBox(rarity).ToArray();
            }
        }
    }
}
