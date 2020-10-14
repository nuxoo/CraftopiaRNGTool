using System;
using System.Windows.Forms;
using System.Text;
using System.IO;

namespace CraftopiaRNGTool
{
    public partial class Form_GetCharList : Form
    {
        public Form_GetCharList()
        {
            InitializeComponent();
            FormClosed += Form_FormClosed;
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.form1.Activate();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Encoding enc = Encoding.GetEncoding("shift_jis");
                string str = textBox1.Text;

                if (str.Length >= 1 && !IsUnicode(str))
                {
                    int cnt = 0;
                    int count = 0;
                    string[] strs = new string[255];
                    int i = GetStrToNum(str);

                    while (count < 255 && cnt < 10000)
                    {
                        cnt++;
                        byte[] data = BitConverter.GetBytes(i);
                        string s = enc.GetString(data);
                        int seed = TreasureCalc.GetServerSeed(s[0]);

                        if (strs[seed] == null)
                        {
                            strs[seed] = "" + s[0];
                            count++;
                        }
                        i += cnt % 256 == 0 ? 1 : 0x100;

                        //if ((seed == 177 && s[0] != '・'))
                        //{
                        //    MessageBox.Show("seed:" + seed + " : " + s);
                        //}
                    }

                    if (count != 255)
                    {
                        Close();
                        MessageBox.Show("失敗！" + count);
                        return;
                    }

                    string fileName = TreasureCalc.dirName + "/" + "List_Char_Seed.txt";
                    File.WriteAllLines(fileName, strs);
                    Close();
                    MessageBox.Show("完了！");
                }
            }
            catch (Exception ex)
            {
                Close();
                Form1.ErrorEvent("エラーが発生しました", ex);
            }
        }

        private int GetStrToNum(string str)
        {
            Encoding enc = Encoding.GetEncoding("shift_jis");
            byte[] data = enc.GetBytes(str);
            byte[] data4 = new byte[4];
            for (int i = 0; i < data4.Length; i++)
            {
                data4[i] = data.Length > i ? data[i] : new byte();
            }
            int num = BitConverter.ToInt32(data4, 0);
            return num;
        }

        private bool IsUnicode(string checkString)
        {
            Encoding enc = Encoding.GetEncoding("shift_jis");
            byte[] translateBuffer = enc.GetBytes(checkString);
            string translateString = enc.GetString(translateBuffer);

            return (checkString != translateString.ToString());
        }
    }
}
