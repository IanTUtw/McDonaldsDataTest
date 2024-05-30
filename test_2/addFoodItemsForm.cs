using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;

namespace test_2
{
    public partial class addFoodItemsForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-4372HT1C\SQLEXPRESS;Initial Catalog=McDonaldData;User ID=Ian89;Password=ian121989;Integrated Security=False;");
        public addFoodItemsForm()
        {
            InitializeComponent();
        }

        private void addFoodItemsForm_Load(object sender, EventArgs e)
        {
            comboBoxEdit.Properties.Items.Add("主食");
            comboBoxEdit.Properties.Items.Add("飲料");
        }

        private void returnBtn_Click(object sender, EventArgs e)
        {
            managerDataCheckFormN managerForm = new managerDataCheckFormN();
            managerForm.Show();
            this.Hide();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            SqlTransaction transaction = null; // 宣告 transaction 變數
            string category = comboBoxEdit.Text;
            string name = txtProductName.Text;
            string cost = txtCost.Text;
            string price = txtPrice.Text;
            // 檢查產品名稱是否重複
            string checkNameQuery = "SELECT COUNT(*) FROM product WHERE productName = @Name";

            try
            {
                // 檢查產品名稱是否已存在
                using (SqlCommand checkNameCmd = new SqlCommand(checkNameQuery, con))
                {
                    con.Open();
                    checkNameCmd.Parameters.AddWithValue("@Name", name);
                    int count = (int)checkNameCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("該產品已存在！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // 如果產品名稱已存在，不執行後續動作
                    }
                }

                // 檢查 cost 和 price 是否為數字和正數
                decimal parsedCost, parsedPrice;
                if (!decimal.TryParse(cost, out parsedCost) || !decimal.TryParse(price, out parsedPrice) || parsedCost <= 0 || parsedPrice <= 0)
                {
                    MessageBox.Show("請輸入正確的成本和價格！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // 檢查成本是否低於售價
                if (parsedCost >= parsedPrice)
                {
                    MessageBox.Show("成本必須低於售價！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 開始交易
                transaction = con.BeginTransaction();

                // 定義要插入資料到 product 表的 SQL 查詢
                string insertQuery = @"INSERT INTO product (category, productName, cost, price) 
                               VALUES (@Category, @Name, @Cost, @Price)";

                // 建立 SqlCommand 物件
                using (SqlCommand cmd = new SqlCommand(insertQuery, con, transaction))
                {
                    // 添加參數到命令中
                    cmd.Parameters.AddWithValue("@Category", category);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Cost", parsedCost);
                    cmd.Parameters.AddWithValue("@Price", parsedPrice);

                    // 執行命令
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // 檢查插入是否成功
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("資料插入成功！");
                        // 提交交易
                        transaction.Commit();
                    }
                    else
                    {
                        MessageBox.Show("資料插入失敗！");
                    }
                }
            }
            catch (Exception ex)
            {
                // 如果發生錯誤，則回滾交易
                if (transaction != null)
                    transaction.Rollback();

                throw new Exception("發生錯誤：" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}