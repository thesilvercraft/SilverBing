using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.Json;

namespace SilverBingConfigUi
{
    public partial class ShowBings : Form
    {
        public ShowBings()
        {
            InitializeComponent();
        }

        private List<Bingtext> bingtexts;
        private string filePath = string.Empty;

        private void Button1_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (BINGSYEAH.json)|*.json",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
                using StreamReader reader = new StreamReader(filePath);
                bingtexts = JsonSerializer.Deserialize<Bingtext[]>(reader.ReadToEnd()).ToList();
                for (int i = bingtexts.Count - 1; i >= 0; i--)
                {
                    listView1.Items.Add(i.ToString());
                }
                listView1.Items.Add("+");
            }
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (listView1.SelectedItems.Count == 1)
                {
                    if (listView1.SelectedItems[0].Text == "+")
                    {
                        //create new bing??
                        var editbing = new BingEditor();
                        if (editbing.ShowDialog() == DialogResult.OK)
                        {
                            bingtexts.Add(editbing.result);
                            listView1.Items[bingtexts.Count - 1].Text = (bingtexts.Count - 1).ToString();
                            listView1.Items.Add("+");
                        }
                    }
                    else
                    {
                        //bruh moment
                        var editbing = new BingEditor(bingtexts[Convert.ToInt32(listView1.SelectedItems[0].Text)]);
                        if (editbing.ShowDialog() == DialogResult.OK)
                        {
                            bingtexts[Convert.ToInt32(listView1.SelectedItems[0].Text)] = editbing.result;
                        }
                    }
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filePath) || bingtexts == null)
            {
                MessageBox.Show("fucking dumbass why did you click it when there isn't even a file opened -Marcel D 2021", "Error",
    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using StreamWriter writer = new StreamWriter(filePath);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            writer.Write(JsonSerializer.Serialize(bingtexts.ToArray(), options));
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                if (listView1.SelectedItems[0].Text != "+")
                {
                    bingtexts.RemoveAt(listView1.SelectedItems[0].Index);
                    listView1.Items.Clear();
                    for (int i = 0; i < bingtexts.Count; i++)
                    {
                        listView1.Items.Add(i.ToString());
                    }
                    listView1.Items.Add("+");
                }
            }
            else
            {
                ConnectOne hahafunni = new ConnectOne();
                hahafunni.Show();
            }
        }
    }
}