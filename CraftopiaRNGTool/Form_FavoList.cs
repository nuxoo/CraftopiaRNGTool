using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.IO;

namespace CraftopiaRNGTool
{
    public partial class Form_FavoList : Form
    {
        public Form_FavoList()
        {
            InitializeComponent();
            FormClosed += Form_FormClosed;
            SetFavoList();
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.form1.Activate();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            WriteFavoList();
            Close();
        }

        private void Up_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            int index = listView1.SelectedItems[0].Index;

            if (index >= 1)
            {
                ListViewItem item = listView1.Items[index];
                listView1.Items.RemoveAt(index);
                listView1.Items.Insert(index - 1, item);
                listView1.Items[index - 1].Selected = true;
                listView1.Select();
            }
        }
        private void Down_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            int index = listView1.SelectedItems[0].Index;

            if (listView1.Items.Count - index >= 2)
            {
                ListViewItem item = listView1.Items[index];
                listView1.Items.RemoveAt(index);
                listView1.Items.Insert(index + 1, item);
                listView1.Items[index + 1].Selected = true;
                listView1.Select();
            }
        }
        private void Add_Click(object sender, EventArgs e)
        {
            string[] item = new string[2];
            item[0] = "" + (int)numericUpDown1.Value;
            item[1] = textBox1.Text;

            listView1.Items.Add(new ListViewItem(item));
        }
        private void Delete_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            int index = listView1.SelectedItems[0].Index;

            if (index >= 0)
            {
                listView1.Items.RemoveAt(index);
                if (listView1.Items.Count >= 1)
                {
                    int selectedIndex = index < listView1.Items.Count ? index : index - 1;
                    listView1.Items[selectedIndex].Selected = true;
                    listView1.Select();
                }
            }
        }

        private void WriteFavoList()
        {
            int count = listView1.Items.Count;
            if (count >= 1)
            {
                string[] strs = new string[count];
                for (int i = 0; i < count; i++)
                {
                    ListViewItem item = listView1.Items[i];
                    strs[i] = GetItemStr(item);
                }

                Form1.favoList = strs;
                File.WriteAllLines(Form1.dirName + Form1.favoListName, strs);
            }
        }
        private void SetFavoList()
        {
            List<ListViewItem> list = new List<ListViewItem>();
            foreach (string str in Form1.favoList)
            {
                if (str != "")
                {
                    list.Add(new ListViewItem(str.Split(',')));
                }
            }

            listView1.Items.AddRange(list.ToArray());
        }

        private void TextEdit_Click(object sender, EventArgs e)
        {
            Form_SearchText form = new Form_SearchText(listView1, this);
            form.ShowDialog();
        }

        public static string GetItemStr(ListViewItem item)
        {
            if (item == null || item.SubItems.Count < 1)
            {
                return ",";
            }
            if (item.SubItems.Count < 2)
            {
                return item.SubItems[0].Text + ",";
            }

            return item.SubItems[0].Text + "," + item.SubItems[1].Text;
        }
    }
}
