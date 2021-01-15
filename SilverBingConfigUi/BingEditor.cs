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
    }

    public class bingtext
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