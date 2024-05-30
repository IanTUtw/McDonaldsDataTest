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
namespace test_2
{
    public partial class loginFormN : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-4372HT1C\SQLEXPRESS;Initial Catalog=McDonaldData;User ID=Ian89;Password=ian121989;Integrated Security=False;");

        public loginFormN()
        {
            InitializeComponent();
            pictureEdit1.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch; // 設為 Stretch mode
        }

        private void loginFormN_Load(object sender, EventArgs e)
        {
            List<string> dataList = new List<string>();
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
                storeComboBox1.Properties.Items.AddRange(dataList);

                // 設置下拉列表樣式
                storeComboBox1.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
            }
            catch (Exception ex)
            {
                throw new Exception("資料庫錯誤: " + ex.Message);
            }
        }

        private void crtBtn_Click(object sender, EventArgs e)
        {
            signUpForm signUp = new signUpForm();
            signUp.Show();
            this.Hide();
        }

        private void loginBtn_Click(object sender, EventArgs e)
        {
            string storeInput = storeComboBox1.Text;
            string accInput = txtAccount.Text;
            string passInput = txtPassword.Text;

            // 創建一個交易對象
            SqlTransaction transaction = null;

            try
            {
                // 如果帳號或密碼為空，顯示錯誤訊息
                if (string.IsNullOrEmpty(accInput) || string.IsNullOrEmpty(passInput))
                {
                    MessageBox.Show("帳號或密碼不能為空");
                    return;
                }
                else if (storeInput == "請選擇門市")
                {
                    MessageBox.Show("請選擇門市");
                    return;
                }
                else
                {
                    string storename = storeComboBox1.Text;
                    Program.StoreName = storename;
                    Program.Username = accInput;

                    // 開啟資料庫連接
                    con.Open();

                    // 開始交易
                    transaction = con.BeginTransaction();

                    // 取得門市對應的ID
                    string getStoreIDQuery = "SELECT storeID FROM storeData WHERE storeName = @StoreInput";
                    SqlCommand getStoreIDCmd = new SqlCommand(getStoreIDQuery, con, transaction);
                    getStoreIDCmd.Parameters.AddWithValue("@StoreInput", storeInput);
                    object storeIDResult = getStoreIDCmd.ExecuteScalar();
                    Guid storeID = (Guid)storeIDResult;

                    // 檢查使用者角色
                    string checkRoleQuery = "SELECT role FROM userData WHERE userAcc = @AccInput and userPass = @PassInput";
                    SqlCommand roleCmd = new SqlCommand(checkRoleQuery, con, transaction);
                    roleCmd.Parameters.AddWithValue("@AccInput", accInput);
                    roleCmd.Parameters.AddWithValue("@PassInput", passInput);
                    object roleResult = roleCmd.ExecuteScalar();
                    string role = (roleResult != null) ? roleResult.ToString() : null;

                    // 如果使用者是管理員，不需要門市，直接登入
                    if (role == "manager")
                    {
                        managerDataCheckFormN mdatacheckForm = new managerDataCheckFormN();
                        mdatacheckForm.Show();
                        this.Hide();
                        // 提交交易
                        transaction.Commit();
                        return;
                    }

                    // 插入新帳號和密碼
                    string checkLoginQuery = "SELECT COUNT(*) FROM userData WHERE storeID = @StoreID AND userAcc = @AccInput AND userPass = @PassInput";
                    // 創建 SqlCommand 對象，用於執行查詢
                    SqlCommand cmd = new SqlCommand(checkLoginQuery, con, transaction);
                    cmd.Parameters.AddWithValue("@StoreID", storeID);
                    cmd.Parameters.AddWithValue("@AccInput", accInput);
                    cmd.Parameters.AddWithValue("@PassInput", passInput);

                    // 執行查詢，返回查詢結果的行數
                    int rowCount = (int)cmd.ExecuteScalar();

                    // 如果查詢結果行數大於 0，表示帳號和密碼正確
                    if (rowCount > 0)
                    {
                        Form1 form1 = new Form1();
                        form1.Show();
                        this.Hide();
                        // 提交交易
                        transaction.Commit();
                    }
                    else
                    {
                        // 否則顯示帳號或密碼輸入錯誤的訊息
                        MessageBox.Show("請確認帳號或密碼是否正確");
                    }
                }
            }
            catch (Exception ex)
            {
                // 發生錯誤，回滾交易
                if (transaction != null)
                    transaction.Rollback();

                // 出現異常時顯示錯誤訊息
                throw new Exception("資料庫錯誤: " + ex.Message);
            }
            finally
            {
                // 關閉資料庫連接
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void storeComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
