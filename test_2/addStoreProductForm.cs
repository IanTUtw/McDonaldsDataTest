using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using System.Configuration;

namespace test_2
{
    public partial class addStoreProductForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-4372HT1C\SQLEXPRESS;Initial Catalog=McDonaldData;User ID=Ian89;Password=ian121989;Integrated Security=False;");
        public addStoreProductForm()
        {
            InitializeComponent();
        }

        private void comboBoxEdit1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 清空 checkedListBoxControl 中的項目
            checkedListBoxControl1.Items.Clear();

            // 取得所選的 storeName
            string selectedStoreName = comboBoxEdit1.SelectedItem.ToString();

            string sqlQuery = @"SELECT p.productName
                                FROM product p
                                WHERE p.productID NOT IN 
                                (
                                    SELECT sp.productID
                                    FROM storeProductData sp
                                    JOIN storeData sd ON sp.storeID = sd.storeID
                                    WHERE sd.storeName = @StoreName
                                )";
        

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                cmd.Parameters.AddWithValue("@StoreName", selectedStoreName);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string productName = reader["productName"].ToString();
                    checkedListBoxControl1.Items.Add(productName); // 將 productName 加入 checkedListBoxControl
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // 處理例外狀況
                MessageBox.Show("資料庫錯誤: " + ex.Message);
            }
            finally
            {
                con.Close(); // 關閉資料庫連接
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            managerDataCheckFormN m = new managerDataCheckFormN();
            m.Show();
            this.Hide();
        }

        private void addStoreProductForm_Load(object sender, EventArgs e)
        {
            string sqlQuery = "SELECT storeName From storeData";
            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    string storeName = reader["storeName"].ToString();
                    comboBoxEdit1.Properties.Items.Add(storeName); // 將 storeName 加入 ComboBoxEdit
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                // 處理例外狀況
                MessageBox.Show("資料庫錯誤: " + ex.Message);
            }
            finally
            {
                con.Close(); // 關閉資料庫連接
            }
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            // 取得所選的 storeName
            string selectedStoreName = comboBoxEdit1.SelectedItem.ToString();

            // 建立 SQL INSERT 語句
            string insertQuery = @"
                                INSERT INTO storeProductData (storeID, productID)
                                VALUES (
                                    (SELECT storeID FROM storeData WHERE storeName = @StoreName),
                                    (SELECT productID FROM product WHERE productName = @ProductName)
                                )
                            ";
            try
            {
                con.Open();

                // 遍歷CheckedListBox中的項目，進行插入操作
                foreach (var item in checkedListBoxControl1.CheckedItems)
                {
                    string productName = item.ToString();
                    SqlCommand cmd = new SqlCommand(insertQuery, con);
                    cmd.Parameters.AddWithValue("@StoreName", selectedStoreName);
                    cmd.Parameters.AddWithValue("@ProductName", productName);
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("資料已成功新增至 storeProductData 表中。");
            }
            catch (Exception ex)
            {
                // 處理例外狀況
                MessageBox.Show("資料庫錯誤: " + ex.Message);
            }
            finally
            {
                con.Close(); // 關閉資料庫連接
            }
        }
    }
}
