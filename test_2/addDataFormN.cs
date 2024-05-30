using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;



namespace test_2
{
    public partial class addDataFormN : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-4372HT1C\SQLEXPRESS;Initial Catalog=McDonaldData;User ID=Ian89;Password=ian121989;Integrated Security=False;");
        public addDataFormN()
        {
            InitializeComponent();
        }

        private void addDataFormN_Load(object sender, EventArgs e)
        {
            repositoryItemSpinEdit = new RepositoryItemSpinEdit();
            string store = Program.StoreName;
            string sqlquery = @"SELECT 
                                    p.productName AS 名稱,
                                    p.category AS 類別
                                FROM
                                    storeData sd
                                JOIN
                                    storeProductData spd ON sd.storeID = spd.storeID
                                JOIN
                                    product p ON spd.productID = p.productID
                                WHERE
                                    sd.storeName = '" + store + @"'
                                ORDER BY
                                    p. category, sd.storeID";
            try
            {
                // 開啟資料庫連接
                con.Open();

                // 創建一個 DataTable 來存儲查詢結果
                DataTable dt = new DataTable();

                // 使用 SqlDataAdapter 執行 SQL 查詢，將結果填充到 DataTable 中
                using (SqlDataAdapter adapter = new SqlDataAdapter(sqlquery, con))
                {
                    adapter.Fill(dt);
                }

                // 創建一個新的 DataTable 來存儲所有的 product 名稱
                DataTable newDt = new DataTable();
                // 添加日期欄位
                newDt.Columns.Add("日期", typeof(DateTime));
                // 將所有的 product 名稱添加為新 DataTable 的欄位
                foreach (DataRow row in dt.Rows)
                {
                    string productName = row["名稱"].ToString();
                    newDt.Columns.Add(productName, typeof(int));
                }

                // 在新 DataTable 中添加一個空白列，用於輸入數據
                DataRow newRow = newDt.NewRow();
                newRow["日期"] = DateTime.Today;
                newDt.Rows.Add(newRow);

                // 設置 GridView 的 DataSource
                gridControl1.DataSource = newDt;

                // 自動調整列寬以適應內容
                gridView1.BestFitColumns();
            }
            catch (Exception ex)
            {
                // 顯示錯誤訊息
                MessageBox.Show("在加載資料時發生錯誤：" + ex.Message);
            }
            finally
            {
                // 關閉資料庫連接
                con.Close();
            }
        }

        private void rtnBtn_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            this.Hide();
        }

        private void addBtn_Click(object sender, EventArgs e)
        {
            string store = Program.StoreName;
            string storeID = ""; // 在 try 區塊之外宣告 storeID 變數，並初始化為空字串
            SqlTransaction transaction = null; // 宣告交易物件

            try
            {
                // 開啟資料庫連接
                con.Open();

                // 開始交易
                transaction = con.BeginTransaction();

                string getIDQuery = "SELECT storeID FROM storeData WHERE storeName = @storeName";
                SqlCommand getIDcmd = new SqlCommand(getIDQuery, con, transaction);
                getIDcmd.Parameters.AddWithValue("@storeName", store);
                object result = getIDcmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    storeID = result.ToString();
                }

                // 遍歷 gridControl 的每一列
                foreach (DataRowView rowView in gridView1.DataSource as DataView)
                {
                    DataRow row = rowView.Row;

                    // 檢查 numData 表中是否已存在相同 storeID 和 numDate 的記錄
                    string checkExistenceQuery = "SELECT COUNT(*) FROM numData WHERE storeID = @storeID AND numDate = @numDate";
                    using (SqlCommand checkExistenceCmd = new SqlCommand(checkExistenceQuery, con, transaction))
                    {
                        checkExistenceCmd.Parameters.AddWithValue("@storeID", storeID);
                        checkExistenceCmd.Parameters.AddWithValue("@numDate", row["日期"]);
                        int count = Convert.ToInt32(checkExistenceCmd.ExecuteScalar());
                        if (count > 0)
                        {
                            // 如果已經存在相同記錄，則顯示提示訊息並跳出此次循環
                            string message = string.Format("日期 {0} 的數據已建立。", row["日期"]);
                            MessageBox.Show(message);
                            return;
                        }
                    }

                    // 將 productname 與數量新增至對應的 product table 中
                    foreach (DataColumn col in row.Table.Columns)
                    {
                        if (col.ColumnName != "日期")
                        {
                            string productName = col.ColumnName;
                            int num = Convert.ToInt32(row[col]);

                            // 取得 productID
                            Guid productID;
                            using (SqlCommand getProductIDCmd = new SqlCommand("SELECT productID FROM product WHERE productName = @productName", con, transaction))
                            {
                                getProductIDCmd.Parameters.AddWithValue("@productName", productName);
                                object idResult = getProductIDCmd.ExecuteScalar();
                                if (idResult != null && idResult != DBNull.Value)
                                {
                                    productID = (Guid)idResult;
                                }
                                else
                                {
                                    // 處理找不到 productID 的情況
                                    continue;
                                }
                            }

                            // 新增至 numData
                            using (SqlCommand addNumDataCmd = new SqlCommand("INSERT INTO numData (numDate, storeID, productID, num) VALUES (@numDate, @storeID, @productID, @num); SELECT SCOPE_IDENTITY();", con, transaction))
                            {
                                addNumDataCmd.Parameters.AddWithValue("@numDate", row["日期"]);
                                addNumDataCmd.Parameters.AddWithValue("@storeID", storeID);
                                addNumDataCmd.Parameters.AddWithValue("@productID", productID);
                                addNumDataCmd.Parameters.AddWithValue("@num", num);
                                addNumDataCmd.ExecuteScalar();
                            }
                        }
                    }
                }

                // 提交交易
                transaction.Commit();

                // 顯示成功訊息
                MessageBox.Show("資料已成功新增");
            }
            catch (Exception ex)
            {
                // 回滾交易
                if (transaction != null)
                    transaction.Rollback();

                // 顯示錯誤訊息
                MessageBox.Show("在新增資料時發生錯誤：" + ex.Message);
            }
            finally
            {
                // 關閉資料庫連接
                con.Close();
            }
        }


        private RepositoryItemSpinEdit repositoryItemSpinEdit;

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            // 檢查是否為數字欄位
            if (e.Column.FieldName != "日期")
            {
                // 獲取上一個儲存的值
                object oldValue = gridView1.GetRowCellValue(e.RowHandle, e.Column);

                // 確保輸入值為整數
                int value;
                if (!int.TryParse(e.Value.ToString(), out value))
                {
                    // 如果輸入值不是整數,則恢復原值
                    gridView1.SetRowCellValue(e.RowHandle, e.Column, oldValue);
                }
            }
        }

        private void gridView1_InitNewRow(object sender, DevExpress.XtraGrid.Views.Grid.InitNewRowEventArgs e)
        {
            // 為數字欄位設置 SpinEdit 編輯器
            foreach (GridColumn col in gridView1.Columns)
            {
                if (col.FieldName != "日期")
                {
                    col.ColumnEdit = repositoryItemSpinEdit;
                }
            }
        }

        private void gridView1_CustomRowCellEdit(object sender, DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventArgs e)
        {
            // 將 SpinEdit 編輯器應用到數字欄位
            if (e.Column.FieldName != "日期")
            {
                e.RepositoryItem = repositoryItemSpinEdit;
            }
        }
    }
}
