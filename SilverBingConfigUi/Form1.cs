﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SilverBingConfigUi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var showbings = new ShowBings();
            showbings.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
            //SplashEditor editor = new SplashEditor();
            //editor.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}