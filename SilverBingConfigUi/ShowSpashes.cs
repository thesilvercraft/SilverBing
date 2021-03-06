﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Forms;

namespace SilverBingConfigUi
{
    public partial class ShowSpashes : Form
    {
        public ShowSpashes()
        {
            InitializeComponent();
        }

        private string filePath;
        private List<Status> activities = new();

        private void Button1_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new()
            {
                Filter = "JSON files (splashes.json)|*.json",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
                using StreamReader reader = new(filePath);
                activities = JsonSerializer.Deserialize<Status[]>(reader.ReadToEnd()).ToList();
                for (int i = 0; i < activities.Count; i++)
                {
                    listView1.Items.Add(activities[i].Name);
                }
                listView1.Items.Add("+");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(filePath) || activities == null)
            {
                MessageBox.Show("fucking dumbass why did you click it when there isn't even a file opened -Marcel D 2021", "Error",
    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using StreamWriter writer = new(filePath);
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            writer.Write(JsonSerializer.Serialize(activities.ToArray(), options));
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listView1.SelectedItems.Count == 1)
            {
                if (listView1.SelectedItems[0].Text == "+")
                {
                    //create new bing??
                    var editsplash = new SplashEditor();
                    if (editsplash.ShowDialog() == DialogResult.OK)
                    {
                        activities.Add(editsplash.result);
                        listView1.Items[activities.Count - 1].Text = editsplash.result.Name;
                        listView1.Items.Add("+");
                    }
                }
                else
                {
                    //bruh moment
                    var editsplash = new SplashEditor(activities[listView1.SelectedItems[0].Index]);
                    if (editsplash.ShowDialog() == DialogResult.OK)
                    {
                        activities[listView1.SelectedItems[0].Index] = editsplash.result;
                        listView1.Items[listView1.SelectedItems[0].Index].Text = editsplash.result.Name;
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                if (listView1.SelectedItems[0].Text != "+")
                {
                    activities.RemoveAt(listView1.SelectedItems[0].Index);
                    listView1.Items.Clear();
                    for (int i = 0; i < activities.Count; i++)
                    {
                        listView1.Items.Add(i.ToString());
                    }
                    listView1.Items.Add("+");
                }
            }
            else
            {
                ConnectOne hahafunni = new();
                hahafunni.Show();
            }
        }
    }
}