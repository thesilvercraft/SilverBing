﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace SilverBingConfigUi
{
    public partial class SplashEditor : Form
    {
        public SplashEditor()
        {
            InitializeComponent();
        }

        public SplashEditor(Status status)
        {
            InitializeComponent();
            comboBox1.SelectedIndex = status.ActivityType;
            textBox1.Text = status.Name;
            textBox2.Enabled = comboBox1.SelectedIndex == 1;
            textBox2.Text = status.StreamUrl;
        }

        public Status result = new();
        private static readonly string[] games = { "Fartnite", "Minecraft", "Doki Doki literature club" };
        private static readonly string[] music = { "Up b down b", "Pumped up kicks", "Want you gone" };
        private static readonly string[] streamnames = { "Twitch plays minecraft", "EpicSMP day 1", "Scambaiting episode 69", "Coding SilverBing" };

        //Playing
        //Streaming
        //Listening to
        //Watching
        //Custom
        //Competing
        private void SplashEditor_Load(object sender, EventArgs e)
        {
            if (richTextBox1.TextLength == 0)
            {
                richTextBox1.AppendText("Choose the status prefix, For example:");
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
                richTextBox1.AppendText("Playing ");
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
                richTextBox1.AppendText("Minecraft");
            }
        }

        private void UpdatePreview(object sender, EventArgs e)
        {
            if (sender.GetType() == comboBox1.GetType())
            {
                richTextBox1.Clear();
                richTextBox1.AppendText("Choose the status prefix, For example:");
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Bold);
                richTextBox1.AppendText($"{comboBox1.SelectedItem} ");
                richTextBox1.SelectionFont = new Font(richTextBox1.Font, FontStyle.Regular);
                RandomGenerator rg = new();
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        richTextBox1.AppendText(games[rg.Next(0, games.Length)]);
                        break;

                    case 1:
                        richTextBox1.AppendText(streamnames[rg.Next(0, streamnames.Length)]);
                        break;

                    case 2:
                        richTextBox1.AppendText(music[rg.Next(0, music.Length)]);
                        break;

                    case 3:
                        richTextBox1.AppendText(streamnames[rg.Next(0, streamnames.Length)]);
                        break;

                    default:
                        richTextBox1.AppendText("things");
                        break;
                }
            }

            textBox2.Enabled = comboBox1.SelectedIndex == 1;

            richTextBox3.Clear();
            richTextBox3.SelectionFont = new Font(richTextBox3.Font, FontStyle.Bold);
            richTextBox3.AppendText($"{comboBox1.SelectedItem} ");
            richTextBox3.SelectionFont = new Font(richTextBox3.Font, FontStyle.Regular);
            richTextBox3.AppendText(textBox1.Text);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            result.Name = textBox1.Text;
            result.ActivityType = comboBox1.SelectedIndex == -1 ? 0 : comboBox1.SelectedIndex;
            if (comboBox1.SelectedIndex == 1)
            {
                result.StreamUrl = textBox2.Text;
            }
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}