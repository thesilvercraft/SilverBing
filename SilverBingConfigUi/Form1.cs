using System;
using System.Windows.Forms;

namespace SilverBingConfigUi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void ChangeConfigButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("NOT IMPLEMENTED");
        }

        private void ChangeBingTextsButton_Click(object sender, EventArgs e)
        {
            new ShowBings().Show();
        }

        private void ChangeStatusButton_Click(object sender, EventArgs e)
        {
            new ShowSpashes().Show();
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}