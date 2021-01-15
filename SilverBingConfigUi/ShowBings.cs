using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SilverBingConfigUi
{
    public partial class ShowBings : Form
    {
        public ShowBings()
        {
            InitializeComponent();
        }

        private List<Bingtext> bingtexts;

        private void button1_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JSON files (BINGSYEAH.json)|*.json";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;
                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        bingtexts = System.Text.Json.JsonSerializer.Deserialize<Bingtext[]>(reader.ReadToEnd()).ToList();
                        for (int i = 0; i < bingtexts.Count; i++)

                        {
                            listView1.Items.Add(i.ToString());
                        }
                        listView1.Items.Add("+");
                    }
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
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
    }
}