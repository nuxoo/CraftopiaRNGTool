using System;
using System.Windows.Forms;

namespace CraftopiaRNGTool
{
    public partial class Form_SearchText : Form
    {
        private ListBox form1_ListBox;
        private ListView form_ListView;
        private Form_FavoList form_Favo;

        public Form_SearchText(ListBox box)
        {
            InitializeComponent();
            FormClosed += Form_FormClosed;
            form1_ListBox = box;
            string str = "";
            for (int i = 0; i < box.Items.Count; i++)
            {
                if (i != 0) str += "\n";
                str += "" + box.Items[i];
            }
            textBox1.Text = str;
        }
        public Form_SearchText(ListView view, Form_FavoList form)
        {
            InitializeComponent();
            FormClosed += Form_FormClosed;
            form_ListView = view;
            form_Favo = form;
            string str = "";
            for (int i = 0; i < view.Items.Count; i++)
            {
                if (i != 0) str += "\n";
                str += Form_FavoList.GetItemStr(view.Items[i]);
            }
            textBox1.Text = str;
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (form_Favo == null)
            {
                Program.form1.Activate();
            }
            else
            {
                form_Favo.Activate();
            }
        }

        private void Ok_Click(object sender, EventArgs e)
        {
            if (form1_ListBox != null)
            {
                SetForm1ListBox();
            }
            else if (form_ListView != null)
            {
                SetFormListView();
            }
            Close();
        }

        private void SetForm1ListBox()
        {
            form1_ListBox.Items.Clear();
            if (textBox1.Text == "") return;

            string[] strs = textBox1.Text.Split('\n');
            object[] objs = new object[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                objs[i] = strs[i];
            }
            form1_ListBox.Items.AddRange(objs);
        }
        private void SetFormListView()
        {
            form_ListView.Items.Clear();
            if (textBox1.Text == "") return;

            string[] strs = textBox1.Text.Split('\n');
            ListViewItem[] items = new ListViewItem[strs.Length];
            for (int i = 0; i < strs.Length; i++)
            {
                items[i] = new ListViewItem(strs[i].Split(','));
            }
            form_ListView.Items.AddRange(items);
        }
    }
}
