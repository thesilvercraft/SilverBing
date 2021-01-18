using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
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
        private List<Status> activities = new List<Status>();

        private void Button1_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "JSON files (splashes.json)|*.json",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog.FileName;
                using StreamReader reader = new StreamReader(filePath);
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
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }
    }
}