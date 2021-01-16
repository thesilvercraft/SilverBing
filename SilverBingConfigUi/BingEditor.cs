using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SilverBingConfigUi
{
    public partial class BingEditor : Form
    {
        public BingEditor()
        {
            InitializeComponent();
        }

        public BingEditor(Bingtext text)
        {
            InitializeComponent();
            textBox1.Text = text.Text;
            numericUpDown1.Value = text.Number_of_bings_of_user != null ? (decimal)text.Number_of_bings_of_user : -1;
            comboBox1.SelectedItem = text.Hour != null ? text.Hour : "None";
            comboBox2.SelectedItem = text.Minute != null ? text.Minute : "None";
            comboBox4.SelectedIndex = text.Day_of_week == null ? 0 : (int)text.Day_of_week + 1;
            textBox2.Text = $"{(text.Day == null ? "none" : text.Day)}.{(text.Month == null ? "none" : text.Month)}.{(text.Year == null ? "none" : text.Year)}";
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
        }

        private void BingEditor_Load(object sender, EventArgs e)
        {
        }

        private static readonly string[] usernames = { "SilverDiamond", "Wbbubier", "Qwerty" };
        private static readonly string[] nicknames = { "SilverDimond", "Wbbubler", "Bong God" };
        public Bingtext result;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            RandomGenerator rng = new RandomGenerator();
            int random_person = rng.Next(0, usernames.Length);
            try
            {
                prevtext.Text = string.Format(textBox1.Text, "@" + usernames[random_person], nicknames[random_person]);
            }
            catch (FormatException)
            {
                if (prevtext.Text != "failed to preview")
                {
                    prevtext.Text = "failed to preview";
                }
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            result = new Bingtext
            {
                Text = textBox1.Text
            };
            if (numericUpDown1.Value != -1)
            {
                result.Number_of_bings_of_user = (ulong?)numericUpDown1.Value;
            }
            if ((string)comboBox1.SelectedItem != "None")
            {
                result.Hour = Convert.ToInt32((string)comboBox1.SelectedItem);
            }
            if ((string)comboBox2.SelectedItem != "None")
            {
                result.Minute = Convert.ToInt32((string)comboBox2.SelectedItem);
            }
            if ((string)comboBox4.SelectedItem != "None")
            {
                result.Day_of_week = comboBox4.SelectedIndex - 1;
            }
            string date = textBox2.Text.ToLower();
            string[] dates = date.Split(".");
            if (dates[0] != "none")
            {
                result.Day = Convert.ToInt32(dates[0]);
            }
            if (dates[1] != "none")
            {
                result.Month = Convert.ToInt32(dates[1]);
            }
            if (dates[2] != "none")
            {
                result.Year = Convert.ToInt32(dates[2]);
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }

    public class Bingtext
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = "{0} reacted to the Microsoft bing first.";

        [JsonPropertyName("day")]
        public int? Day { get; set; } = null;

        [JsonPropertyName("month")]
        public int? Month { get; set; } = null;

        [JsonPropertyName("year")]
        public int? Year { get; set; } = null;

        [JsonPropertyName("hour")]
        public int? Hour { get; set; } = null;

        [JsonPropertyName("minute")]
        public int? Minute { get; set; } = null;

        [JsonPropertyName("day_of_week")]
        public int? Day_of_week { get; set; } = null;

        [JsonPropertyName("number_of_bings_of_user")]
        public ulong? Number_of_bings_of_user { get; set; } = null;
    }
}