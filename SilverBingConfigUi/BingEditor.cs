using System;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Forms;

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

        private static readonly string[] usernames = { "SilverDiamond", "Wbbubier", "Qwerty" };
        private static readonly string[] nicknames = { "SilverDimond", "Wbbubler", "Bong God" };
        public Bingtext result = new();

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
            RandomGenerator rng = new();
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

        private void Button1_Click(object sender, EventArgs e)
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
            if (DateTextBoxValidateAndAddDate())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show("Date isn't a real one you doofus", "Error",
  MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool DateTextBoxValidateAndAddDate()
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                errorProvider1.SetError(textBox2, "Please enter a date");
                return false;
            }
            else
            {
                string date = textBox2.Text.ToLower();
                string[] dates = date.Split(".");
                if (dates.Length != 3)
                {
                    errorProvider1.SetError(textBox2, "Please use the format day.month.year");
                    return false;
                }
                bool[] isnotnull = { dates[0] != "none", dates[1] != "none", dates[2] != "none" };

                if (isnotnull[0])
                {
                    result.Day = Convert.ToInt32(dates[0]);
                }
                if (isnotnull[1])
                {
                    result.Month = Convert.ToInt32(dates[1]);
                }
                if (isnotnull[2])
                {
                    result.Year = Convert.ToInt32(dates[2]);
                }
                if (!isnotnull.Contains(false) && !DateUtils.Is_Valid_Date_Bool((int)result.Day, (int)result.Month, (int)result.Year))
                {
                    errorProvider1.SetError(textBox2, "That isnt a valid date");
                    return false;
                }
            }
            errorProvider1.SetError(textBox2, "");
            return true;
        }

        private void TextBox2_Validating(object sender, CancelEventArgs e)
        {
            DateTextBoxValidateAndAddDate();
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