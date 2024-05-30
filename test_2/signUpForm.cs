using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors.Controls;

namespace test_2
{
    public partial class signUpForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-4372HT1C\SQLEXPRESS;Initial Catalog=McDonaldData;User ID=Ian89;Password=ian121989;Integrated Security=False;");
        
        public signUpForm()
        {
            InitializeComponent();
        }

        private void signUpForm_Load(object sender, EventArgs e)
        {
            List<string> dataList = new List<string>();//導入資料進combobox
            try
            {
                con.Open();
                string sqlQuery = "SELECT storeName FROM storeData";
                SqlCommand command = new SqlCommand(sqlQuery, con);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string storeName = reader["storeName"].ToString();
                    dataList.Add(storeName);
                }
                con.Close();
                // 綁定數據源
                storeComboBoxEdit.Properties.Items.AddRange(dataList);

                // 設置列表下拉樣式
                storeComboBoxEdit.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            }
            catch (Exception ex)
            {
                throw new Exception("資料庫錯誤: " + ex.Message);
            }
        }   

        private void storeComboBoxEdit_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show("您選擇了：" + storeComboBoxEdit.SelectedItem.ToString());
        }

        private void signUpSimpleBtn_Click(object sender, EventArgs e)
        {
            string storeInput = storeComboBoxEdit.Text;
            string accInput = textEditAcc.Text;
            string passInput = textEditPaswrd.Text;
            string passAInput = textEditPaswrdA.Text;
            string role = "normal";
            string checkAccQuery = "SELECT COUNT(*) FROM userData WHERE userAcc = @AccInput";

            // 創建一個交易對象
            SqlTransaction transaction = null;

            try
            {
                con.Open();

                // 開始交易
                transaction = con.BeginTransaction();

                // 檢查帳號是否已存在
                SqlCommand checkAccCmd = new SqlCommand(checkAccQuery, con, transaction);
                checkAccCmd.Parameters.AddWithValue("@AccInput", accInput);
                int existingAccCount = (int)checkAccCmd.ExecuteScalar();
                if (existingAccCount > 0)
                {
                    MessageBox.Show("已有相同帳號");
                    return;
                }

                // 檢查密碼核實是否正確
                if (passInput != passAInput)
                {
                    MessageBox.Show("密碼核實錯誤");
                    return;
                }
                else if (storeInput == "請選擇門市")
                {
                    MessageBox.Show("請選擇門市");
                    return;
                }
                // 取得門市對應的ID
                string getStoreIDQuery = "SELECT storeID FROM storeData WHERE storeName = @StoreInput";
                SqlCommand getStoreIDCmd = new SqlCommand(getStoreIDQuery, con, transaction);
                getStoreIDCmd.Parameters.AddWithValue("@StoreInput", storeInput);
                object storeIDResult = getStoreIDCmd.ExecuteScalar();
                if (storeIDResult == null)
                {
                    MessageBox.Show("找不到相應的門市");
                    return;
                }
                Guid storeID = (Guid)storeIDResult;
                // 插入新帳號和密碼
                string insertQuery = "INSERT INTO userData (storeID, userAcc, userPass,role) VALUES (@StoreID, @AccInput, @PassInput, @Role)";
                SqlCommand insertCmd = new SqlCommand(insertQuery, con, transaction);
                insertCmd.Parameters.AddWithValue("@StoreID", storeID);
                insertCmd.Parameters.AddWithValue("@AccInput", accInput);
                insertCmd.Parameters.AddWithValue("@PassInput", passInput);
                insertCmd.Parameters.AddWithValue("@Role", role);
                int rowsAffected = insertCmd.ExecuteNonQuery();

                // 提交交易
                transaction.Commit();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("創建成功");
                }
                else
                {
                    MessageBox.Show("創建失敗");
                }
            }
            catch (Exception ex)
            {
                // 發生錯誤，回滾交易
                if (transaction != null)
                    transaction.Rollback();

                throw new Exception("資料庫錯誤: " + ex.Message);
            }
            finally
            {
                // 關閉資料庫連接
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }


        private void returnBtn_Click(object sender, EventArgs e)
        {
            loginFormN loginForm = new loginFormN();
            loginForm.Show();
            this.Hide();
        }
    }
}
