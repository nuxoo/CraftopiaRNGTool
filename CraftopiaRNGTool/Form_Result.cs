using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CraftopiaRNGTool
{
    public partial class Form_Result : Form
    {
        public Form_Result(List<TreasureData> treDatas, int mode)
        {
            InitializeComponent();
            FormClosed += Form_FormClosed;
            SetData(treDatas, mode);
        }

        private void Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.form1.Activate();
        }

        private void SetData(List<TreasureData> treDatas, int mode)
        {
            string[] names = new string[] {
                "IslandLv", "Seed", "WSeed", "SeedChar", "MapId", "Rarity", "PosHash", "Memo", "X", "Z", "Y"
            };
            string[] names2 = new string[] {
                "島Lv", "Seed", "WSeed", "Char", "MapId", "Type", "宝箱座標", "メモ", "x", "z", "y"
            };
            int[] widths = new int[] {
                40, 44, 44, 40, 50, 44, 100, 100, 60, 60, 60
            };
            int itemWidth = 100;
            int enchantWidth = 64;
            DataGridViewTextBoxColumn[] columns = new DataGridViewTextBoxColumn[names.Length + 4 * 4];
            int count = 0;
            int viewWidth = 0;

            for (int i = 0; i < names.Length; i++)
            {
                DataGridViewTextBoxColumn textColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = names[i],
                    HeaderText = names2[i],
                    Width = widths[i],
                };

                if (i == 7 && mode == 3)
                {
                    textColumn.Visible = false;
                }
                if (i > 7 && (mode == 1 || mode == 3))
                {
                    textColumn.Visible = false;
                }

                columns[count] = textColumn;
                count++;
                if (textColumn.Visible) viewWidth += widths[i];
            }

            for (int i = 1; i <= 4; i++)
            {
                DataGridViewTextBoxColumn textColumn = new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "Item" + i,
                    HeaderText = "アイテム" + i,
                    Width = itemWidth
                };
                textColumn.DefaultCellStyle.BackColor = Color.LavenderBlush;
                columns[count] = textColumn;
                count++;
                viewWidth += itemWidth;
                for (int j = 1; j <= 3; j++)
                {
                    DataGridViewTextBoxColumn textColumn2 = new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Enchant" + i + "_" + j,
                        HeaderText = "op" + j,
                        Width = enchantWidth
                    };
                    columns[count] = textColumn2;
                    count++;
                    viewWidth += enchantWidth;
                }
            }

            Width = viewWidth + 34;
            dataGridView1.Columns.AddRange(columns);
            dataGridView1.DataSource = treDatas;
        }
    }
}
