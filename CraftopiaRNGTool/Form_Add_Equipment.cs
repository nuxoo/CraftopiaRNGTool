using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CraftopiaRNGTool
{
    public partial class Form_Add_Equipment : Form
    {
        private ListBox form1_ListBox;
        private bool isAddMode;
        private static object[] itemList;

        public Form_Add_Equipment(ListBox box, bool isAdd)
        {
            InitializeComponent();
            FormClosed += Form_FormClosed;
            form1_ListBox = box;
            isAddMode = isAdd;
            if (itemList == null) itemList = Form1.GetItemList(true);

            comboBox1.Items.AddRange(itemList);
            comboBox2.Items.AddRange(GetEnchantList());
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            if (!isAddMode) SetEditMode();
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.form1.Activate();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            string text = comboBox1.Text + "," + GetEnchantCount() + GetEnchantStr();
            if (isAddMode)
            {
                form1_ListBox.Items.Add(text);
            }
            else
            {
                form1_ListBox.Items[form1_ListBox.SelectedIndex] = text;
            }
            Close();
        }

        private void Up_Click(object sender, EventArgs e)
        {
            Form1.ButtonEvent_Up(listBox1);
        }
        private void Down_Click(object sender, EventArgs e)
        {
            Form1.ButtonEvent_Down(listBox1);
        }
        private void Add_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add(comboBox2.Text);
        }
        private void Delete_Click(object sender, EventArgs e)
        {
            Form1.ButtonEvent_Delete(listBox1);
        }
        
        private object[] GetEnchantList()
        {
            List<object> list = new List<object>();
            foreach (EnchantData[] enchants in TreasureCalc.data_Enchant)
            {
                foreach (EnchantData enchant in enchants)
                {
                    list.Add(enchant.Name);
                }
            }

            return list.ToArray();
        }

        private int GetEnchantCount()
        {
            int count;
            if (comboBox3.SelectedIndex <= 0)
            {
                count = listBox1.Items.Count;
                if (count > 3) count = 3;
            }
            else
            {
                count = comboBox3.SelectedIndex - 1;
            }

            return count;
        }
        private string GetEnchantStr()
        {
            string str = "";
            foreach (object item in listBox1.Items)
            {
                str += "," + item;
            }

            return str;
        }

        private void SetEditMode()
        {
            string str = "" + form1_ListBox.Items[form1_ListBox.SelectedIndex];
            string[] strs = str.Split(',');

            comboBox1.Text = strs[0];
            int index = TreasureCalc.IntParse(strs[1]) + 1;
            if (index < 5) comboBox3.SelectedIndex = index;

            for (int i = 2; i < strs.Length; i++)
            {
                listBox1.Items.Add(strs[i]);
            }
        }
    }
}
