using System;
using System.Windows.Forms;

namespace CraftopiaRNGTool
{
    public partial class Form_Add_Item : Form
    {
        private ListBox form1_ListBox;
        private bool isAddMood;
        private static object[] itemList;

        public Form_Add_Item(ListBox box, bool isAdd)
        {
            InitializeComponent();
            FormClosed += Form_FormClosed;
            form1_ListBox = box;
            isAddMood = isAdd;
            if (itemList == null) itemList = Form1.GetItemList(false);

            comboBox1.Items.AddRange(itemList);
            comboBox1.SelectedIndex = 0;
            if (!isAddMood) comboBox1.Text = "" + box.Items[box.SelectedIndex];
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.form1.Activate();
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            if (isAddMood)
            {
                form1_ListBox.Items.Add(comboBox1.Text);
            }
            else
            {
                form1_ListBox.Items[form1_ListBox.SelectedIndex] = comboBox1.Text;
            }
            Close();
        }
    }
}
