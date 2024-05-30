using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace test_2
{
    public partial class Form1 : Form //startForm
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void addDataBtn_Click(object sender, EventArgs e) //跳轉新增資料頁面Btn
        {
            addDataFormN addForm = new addDataFormN();

            addForm.Visible = true;
            this.Visible = false;
        }

        private void dataCheckBtn_Click(object sender, EventArgs e) //資料查看頁面Btn
        {
            dataCheckFormN check = new dataCheckFormN();

            check.Show();
            this.Hide();
        }

        private void logOutBtn_Click(object sender, EventArgs e)
        {
            loginFormN loginfrom = new loginFormN();
            loginfrom.Show();
            this.Hide();
        }
    }
}
