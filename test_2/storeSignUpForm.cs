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
    public partial class storeSignUpForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-4372HT1C\SQLEXPRESS;Initial Catalog=McDonaldData;User ID=Ian89;Password=ian121989;Integrated Security=False;");
        public storeSignUpForm()
        {
            InitializeComponent();

        }

        private void storeSignUpForm_Load(object sender, EventArgs e)
        {
            LoadMainCourseProducts();
            LoadDrinkProducts();
        }

        private void simpleButton1_Click(object sender, EventArgs e)//送出Btn
        {
            string userAcc = Program.Username;
            string store = storeTextEdit.Text;
            string locat = locatTextEdit.Text;
            string phoneNum = phoneNumTextEdit.Text;
            string storeLeader = leaderTextEdit.Text;
            string leaderPhone = leaderPhoneTextEdit.Text;
            string insertStoreQuery = "INSERT INTO storeData (storeName, address, phone, storeLeader, leaderPhone, createBy,updateBy) VALUES (@Store, @Location, @PhoneNum, @StoreLeader, @LeaderPhone, @Create_By, @Update_By)";

            // 選擇的主食項目
            List<string> mainCourses = GetSelectedItems(checkedListBoxControl1);
            // 選擇的飲料項目
            List<string> drinks = GetSelectedItems(checkedListBoxControl2);
            
            try//這裡要新增門市判斷
            {
                con.Open();

                // 開始交易
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    // 建立命令，指定交易
                    using (SqlCommand cmd = new SqlCommand(insertStoreQuery, con, transaction))
                    {
                        cmd.Parameters.AddWithValue("@Store", store);
                        cmd.Parameters.AddWithValue("@Location", locat);
                        cmd.Parameters.AddWithValue("@PhoneNum", phoneNum);
                        cmd.Parameters.AddWithValue("@StoreLeader", storeLeader);
                        cmd.Parameters.AddWithValue("@LeaderPhone", leaderPhone);
                        cmd.Parameters.AddWithValue("@Create_By", userAcc);
                        cmd.Parameters.AddWithValue("@Update_By", userAcc);
                        cmd.ExecuteNonQuery(); // 執行 userData 的命令
                    }

                    // 取得剛插入的店家ID
                    Guid storeID = GetInsertedStoreID(store, transaction);

                    // 將選擇的主食項目插入到 storeProductData 表中
                    foreach (string mainCourse in mainCourses)
                    {
                        InsertProductData(storeID, mainCourse, transaction);
                    }

                    // 將選擇的飲料項目插入到 storeProductData 表中
                    foreach (string drink in drinks)
                    {
                        InsertProductData(storeID, drink, transaction);
                    }

                    // 提交交易
                    transaction.Commit();
                }

                MessageBox.Show("已送出");
            }
            catch (Exception ex)
            {
                throw new Exception("資料庫錯誤: " + ex.Message);
            }
            finally
            {
                con.Close(); // 關閉資料庫連接
            }
        }

        private List<string> GetSelectedItems(CheckedListBoxControl checkedListBox)
        {
            List<string> selectedItems = new List<string>();

            foreach (CheckedListBoxItem item in checkedListBox.CheckedItems)
            {
                selectedItems.Add(item.Value.ToString());
            }

            return selectedItems;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            managerDataCheckFormN manager = new managerDataCheckFormN();
            manager.Show();
            this.Hide();
        }

        // 插入產品資料到 storeProductData 表中
        private void InsertProductData(Guid storeID, string productName, SqlTransaction transaction)
        {
            // 檢查產品是否存在
            string checkProductQuery = "SELECT COUNT(*) FROM product WHERE productName = @ProductName";
            using (SqlCommand checkCmd = new SqlCommand(checkProductQuery, transaction.Connection, transaction))
            {
                checkCmd.Parameters.AddWithValue("@ProductName", productName);
                int count = (int)checkCmd.ExecuteScalar();
                if (count == 0)
                {
                    throw new Exception("找不到名為 '" + productName + "' 的產品。");
                }
            }

            // 插入產品資料
            string insertProductQuery = "INSERT INTO storeProductData (storeID, productID) VALUES (@StoreID, (SELECT productID FROM product WHERE productName = @ProductName))";
            using (SqlCommand cmd = new SqlCommand(insertProductQuery, transaction.Connection, transaction))
            {
                cmd.Parameters.AddWithValue("@StoreID", storeID);
                cmd.Parameters.AddWithValue("@ProductName", productName);
                cmd.ExecuteNonQuery(); // 執行 storeProductData 的命令
            }
        }

        private Guid GetInsertedStoreID(string storeName, SqlTransaction transaction)
        {
            // 從 storeData 表中取得剛插入的店家ID
            string query = "SELECT storeID FROM storeData WHERE storeName = @Store";
            using (SqlCommand cmdID = new SqlCommand(query, transaction.Connection, transaction))
            {
                cmdID.Parameters.AddWithValue("@Store", storeName);
                object result = cmdID.ExecuteScalar();
                if (result != null)
                {
                    return (Guid)result;
                }
                else
                {
                    throw new Exception("無法獲取新店家的ID。");
                }
            }
        }

        private void checkedListBoxControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 事件處理程序的程式碼
        }

        private void LoadDrinkProducts()
        {
            try
            {
                // 清空已存在的項目
                checkedListBoxControl2.Items.Clear();

                // 開啟連接
                con.Open();

                // 定義 SQL 查詢
                string query = "SELECT productName FROM product WHERE category = '飲料'";

                // 建立 SqlCommand 物件
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    // 執行查詢，並取得結果集
                    SqlDataReader reader = command.ExecuteReader();

                    // 如果有資料
                    while (reader.Read())
                    {
                        // 將 productName 加入到 checkedListBoxControl1 中
                        checkedListBoxControl2.Items.Add(reader["productName"]);
                    }

                    // 關閉 SqlDataReader
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("發生錯誤：" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }

        private void LoadMainCourseProducts()
        {
            try
            {
                // 清空已存在的項目
                checkedListBoxControl1.Items.Clear();

                // 開啟連接
                con.Open();

                // 定義 SQL 查詢
                string query = "SELECT productName FROM product WHERE category = '主食'";

                // 建立 SqlCommand 物件
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    // 執行查詢，並取得結果集
                    SqlDataReader reader = command.ExecuteReader();

                    // 如果有資料
                    while (reader.Read())
                    {
                        // 將 productName 加入到 checkedListBoxControl1 中
                        checkedListBoxControl1.Items.Add(reader["productName"]);
                    }

                    // 關閉 SqlDataReader
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("發生錯誤：" + ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
