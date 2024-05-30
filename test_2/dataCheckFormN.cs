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
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;

namespace test_2
{
    public partial class dataCheckFormN : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-4372HT1C\SQLEXPRESS;Initial Catalog=McDonaldData;User ID=Ian89;Password=ian121989;Integrated Security=False;");
        List<DevExpress.XtraVerticalGrid.Rows.BaseRow> selectedRows = new List<DevExpress.XtraVerticalGrid.Rows.BaseRow>();
        string connectionString = @"Data Source=LAPTOP-4372HT1C\SQLEXPRESS;Initial Catalog=McDonaldData;User ID=Ian89;Password=ian121989;Integrated Security=False;";

        public dataCheckFormN()
        {
            InitializeComponent();
        }

        private void dataCheckFormN_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadData2();
            xtraTabControl1.SelectedTabPage = xtraTabPage1;
            gridView1.Columns["月份"].Width = 35; // 設定指定欄位的寬度
        }
        private void LoadData()
        {
            DataTable finalTable = GetCombinedData();
            gridControl1.DataSource = finalTable;
        }
        string storeNameG = Program.StoreName;
        private List<string> allProductNames = new List<string>(); // 儲存所有產品名稱
        private DataTable GetCombinedData()//合併dataTable
        {
            DataTable finalTable = new DataTable();
            finalTable.Columns.Add("日期", typeof(DateTime));
            finalTable.Columns.Add("月份", typeof(int)); // 新增欄位
            finalTable.Columns.Add("門市", typeof(string));
            finalTable.Columns.Add("總銷售成本", typeof(decimal));
            finalTable.Columns.Add("總銷售金額", typeof(decimal));

            Dictionary<string, Dictionary<string, decimal>> productSales = new Dictionary<string, Dictionary<string, decimal>>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT n.numDate, s.storeName, p.productName, n.num, p.cost, p.price
                         FROM numData n 
                         JOIN storeData s ON n.storeID = s.storeID
                         JOIN product p ON n.productID = p.productID
                         WHERE s.storeName = @StoreName
                         ORDER BY n.numDate";

                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
                command.Parameters.AddWithValue("@StoreName", storeNameG);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    DateTime numDate = reader.GetDateTime(0);
                    int month = numDate.Month; // 獲取月份
                    string storeName = reader.GetString(1);
                    string productName = reader.GetString(2);
                    int num = reader.GetInt32(3);
                    decimal cost = reader.GetDecimal(4);
                    decimal price = reader.GetDecimal(5);

                    // 檢查是否已經存在具有相同 numDate 和 storeName 的行
                    DataRow[] existingRows = finalTable.Select(string.Format("日期 = '{0}' AND 門市 = '{1}'", numDate.ToString("yyyy-MM-dd"), storeName));

                    DataRow newRow;
                    newRow = finalTable.NewRow();

                    if (existingRows.Length > 0)
                    {
                        // 如果存在,則更新該行
                        newRow = existingRows[0];
                    }
                    else
                    {
                        // 如果不存在,則新增一行
                        newRow["日期"] = numDate;
                        newRow["月份"] = month; // 設置月份欄位
                        newRow["門市"] = storeName;
                        finalTable.Rows.Add(newRow);
                    }

                    // 更新或新增產品銷售數量
                    if (!productSales.ContainsKey(numDate.ToString("yyyy-MM-dd") + "-" + storeName))
                    {
                        productSales[numDate.ToString("yyyy-MM-dd") + "-" + storeName] = new Dictionary<string, decimal>();
                    }

                    if (productSales[numDate.ToString("yyyy-MM-dd") + "-" + storeName].ContainsKey(productName))
                    {
                        productSales[numDate.ToString("yyyy-MM-dd") + "-" + storeName][productName] += num;
                    }
                    else
                    {
                        productSales[numDate.ToString("yyyy-MM-dd") + "-" + storeName][productName] = num;
                        if (!finalTable.Columns.Contains(productName))
                        {
                            finalTable.Columns.Add(productName, typeof(decimal));
                        }
                    }

                    // 設定產品銷售數量
                    foreach (var sale in productSales[numDate.ToString("yyyy-MM-dd") + "-" + storeName])
                    {
                        newRow[sale.Key] = sale.Value;
                        // 計算 totalCost 和 totalSale
                        newRow["總銷售成本"] = (newRow["總銷售成本"] == DBNull.Value ? 0m : (decimal)newRow["總銷售成本"]) + (sale.Value * cost);
                        newRow["總銷售金額"] = (newRow["總銷售金額"] == DBNull.Value ? 0m : (decimal)newRow["總銷售金額"]) + (sale.Value * price);
                    }
                }

                reader.Close();
            }

            return finalTable;
        }

        private void devDelBtn_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("確定要刪除資料嗎？", "確認刪除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                SqlTransaction transaction = null; // 創建一個交易對象
                try
                {
                    con.Open();
                    transaction = con.BeginTransaction();

                    // 獲取所選取的所有資料行的索引
                    int[] selectedRowHandles = gridView1.GetSelectedRows();

                    foreach (int focusedRowHandle in selectedRowHandles)
                    {
                        // 獲取所選取資料行的日期和門市
                        DateTime numDate = Convert.ToDateTime(gridView1.GetRowCellValue(focusedRowHandle, "日期"));
                        string storeName = gridView1.GetRowCellValue(focusedRowHandle, "門市").ToString();

                        // 執行 SQL 刪除命令
                        string deleteUserDataCommandText = @"DELETE FROM numData 
                                                            WHERE storeID IN (SELECT storeID FROM storeData WHERE storeName = @StoreName) 
                                                            AND numDate = @NumDate";

                        // 刪除 numData 表格中的資料
                        using (SqlCommand deleteUserDataCommand = new SqlCommand(deleteUserDataCommandText, con, transaction)) // 將交易傳遞給命令
                        {
                            deleteUserDataCommand.Parameters.AddWithValue("@NumDate", numDate);
                            deleteUserDataCommand.Parameters.AddWithValue("@StoreName", storeName);
                            deleteUserDataCommand.ExecuteNonQuery();
                        }
                    }

                    // 提交交易
                    transaction.Commit();
                    MessageBox.Show("選取的資料已成功刪除。");
                }
                catch (Exception ex)
                {
                    // 發生錯誤，回滾交易
                    if (transaction != null)
                        transaction.Rollback();
                    // 使用 throw new Exception 拋出錯誤
                    throw new Exception("資料庫錯誤: " + ex.Message);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
                // 重新載入資料
                LoadData();
            }
        }

        private void searchBtn_Click(object sender, EventArgs e)
        {
            if (dateEdit1.DateTime != DateTime.MinValue && dateEdit2.DateTime != DateTime.MinValue)
            {
                DateTime startDate = dateEdit1.DateTime.Date;
                DateTime endDate = dateEdit2.DateTime.Date;

                // 構建日期篩選條件
                string dateFilter = string.Format("[日期] >= #{0}# AND [日期] <= #{1}#", startDate.ToString("yyyy/MM/dd"), endDate.ToString("yyyy/MM/dd"));

                // 將日期篩選條件應用到篩選字串中
                string filter = dateFilter;

                // 將篩選條件應用到 GridView
                gridView1.ActiveFilterString = filter;
            }
            else
            {
                MessageBox.Show("請選擇日期");
            }
        }

        private void returnBtn_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }
        private void girdControlOutput_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            // 設置對話框的標題
            saveFileDialog1.Title = "儲存 Excel 檔案";

            // 設置檔案類型
            saveFileDialog1.Filter = "Excel 檔案|*.xlsx";

            // 設置預設檔案名稱
            saveFileDialog1.FileName = "報表.xlsx";

            // 設置預設儲存位置為桌面
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // 如果使用者按下了儲存按鈕
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 獲取當前gridControl
                    GridView gridView = gridControl1.MainView as GridView;

                    // 將 gridControl 中的資料輸出到 Excel
                    gridView.ExportToXlsx(saveFileDialog1.FileName);

                    // 顯示成功訊息
                    MessageBox.Show("已成功儲存 Excel 檔案到桌面", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    // 顯示錯誤訊息
                    throw new Exception("發生錯誤：" + ex.Message);
                }
            }
        }

        private void LoadData2()
        {
            string accName = Program.Username;
            string userAccQuery = @"SELECT ud.userID,
                                    sd.storeName AS 門市,
                                    ud.userAcc AS 帳號,
                                    ud.userPass AS 密碼,
                                    ud.role AS 角色,
                                    ud.create_date AS 創立時間,
                                    ud.update_date AS 更新時間
                            FROM userData ud
                            INNER JOIN storeData sd ON ud.storeID = sd.storeID
                            WHERE ud.userAcc = @AccName";

            try
            {
                con.Open();

                // 建立 SqlCommand 並設置參數
                SqlCommand cmd = new SqlCommand(userAccQuery, con);
                cmd.Parameters.AddWithValue("@AccName", accName);

                // 建立 SqlDataAdapter 並填充 DataTable
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // 設定 gridControl2 的資料來源
                gridControl2.DataSource = dataTable;

                // 將 gridView2 設置為 gridControl2 的 MainView
                gridView2.PopulateColumns();
                foreach (GridColumn column in gridView2.Columns)//不能編輯
                {
                    column.OptionsColumn.ReadOnly = true;
                }
                gridView2.Columns["帳號"].OptionsColumn.ReadOnly = false;
                gridView2.Columns["密碼"].OptionsColumn.ReadOnly = false;
                gridView2.Columns["userID"].Visible = false;
            }
            catch (Exception ex)
            {
                // 處理錯誤
                MessageBox.Show("資料庫錯誤: " + ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("確定要更新資料嗎？", "確認更新", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();
                        // 開始交易
                        SqlTransaction transaction = conn.BeginTransaction();
                        try
                        {
                            UpdateDataToSQL(conn, transaction);
                            // 提交交易
                            transaction.Commit();
                            MessageBox.Show("資料更新成功！", "更新成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            // 發生錯誤時回滾交易
                            transaction.Rollback();
                            MessageBox.Show("更新資料時發生錯誤：" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("連接資料庫時發生錯誤：" + ex.Message, "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // 重新載入資料
                    LoadData();
                }
            }
        }

        private void UpdateDataToSQL(SqlConnection conn, SqlTransaction transaction)
        {
            // 遍歷 gridControl 中的每一行資料
            foreach (DataRow row in ((DataTable)gridControl1.DataSource).Rows)
            {
                DateTime date = (DateTime)row["日期"];
                string storeName = (string)row["門市"];
                // 遍歷該行中的每一個欄位
                foreach (DataColumn col in ((DataTable)gridControl1.DataSource).Columns)
                {
                    // 排除非資料欄位，如日期、門市、月份、總銷售成本和總銷售金額
                    if (col.ColumnName != "日期" && col.ColumnName != "門市" && col.ColumnName != "月份" && col.ColumnName != "總銷售成本" && col.ColumnName != "總銷售金額")
                    {
                        string productName = col.ColumnName;
                        // 檢查值是否為 DBNull，如果是的話，將其設定為預設值 0
                        int num = Convert.IsDBNull(row[col]) ? 0 : Convert.ToInt32(row[col]);
                        // 呼叫方法更新該產品的銷售數量
                        UpdateProductNum(conn, transaction, date, storeName, productName, num);
                    }
                }
            }
        }

        // 更新指定產品的銷售數量
        private void UpdateProductNum(SqlConnection conn, SqlTransaction transaction, DateTime date, string storeName, string productName, int num)
        {
            string query = @"UPDATE numData 
                     SET num = @Num
                     WHERE numDate = @NumDate 
                     AND storeID = (SELECT storeID FROM storeData WHERE storeName = @StoreName)
                     AND productID = (SELECT productID FROM product WHERE productName = @ProductName)";

            SqlCommand command = new SqlCommand(query, conn, transaction);
            // 設定參數
            command.Parameters.AddWithValue("@Num", num); // 設定新的銷售數量
            command.Parameters.AddWithValue("@NumDate", date); // 設定日期
            command.Parameters.AddWithValue("@StoreName", storeName); // 設定門市名稱
            command.Parameters.AddWithValue("@ProductName", productName); // 設定產品名稱
            command.ExecuteNonQuery(); // 執行 SQL 指令
        }


        private bool CheckRowChanged(DataRow row, DataColumnCollection columns)
        {
            // 比较所有的列是否发生了更改
            foreach (DataColumn column in columns)
            {
                if (row.Table.Columns.Contains(column.ColumnName) && row.RowState != DataRowState.Deleted)
                {
                    // 检查该列是否存在且是否发生了变化
                    if (!row[column.ColumnName, DataRowVersion.Original].Equals(row[column.ColumnName]))
                    {
                        return true; // 有列值发生变更
                    }
                }
            }

            return false; // 数据未更改
        }

        void panelControl2_Paint(object sender, PaintEventArgs e)
        {
            
        }

        private void updateBtn2_Click(object sender, EventArgs e)//Tab2更新Btn
        {
            DialogResult result = MessageBox.Show("確定要更新資料嗎？", "確認更新", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                SqlTransaction transaction = null; // 創建一個交易對象
                int i = 0;
                try
                {
                    // 打開資料庫連接
                    con.Open();

                    // 開始交易
                    transaction = con.BeginTransaction();

                    // 取得當前 GridControl 中的資料
                    DataTable dataTable2 = (DataTable)gridControl2.DataSource;

                    // 取得原始資料表
                    DataTable originalTable = GetCombinedData();
                    // 遍歷每一列，進行更新
                    foreach (DataRow row in dataTable2.Rows)
                    {
                        // 檢查該行資料是否有更改
                        if (CheckRowChanged(row, "帳號","密碼"))
                        {
                            i++;
                            // 更新語句，使用參數化查詢以避免 SQL 注入攻擊
                            string checkAccQuery = "SELECT COUNT(*) FROM userData WHERE userAcc = @UserAcc AND userID != @ID"; // 檢查是否有其他相同帳號
                            string updateQuery = "UPDATE userData SET userAcc = @UserAcc, userPass = @UserPass, update_date = GETDATE() WHERE userID = @ID";

                            using (SqlCommand command = new SqlCommand(checkAccQuery, con, transaction))
                            {
                                command.Parameters.AddWithValue("@UserAcc", row["帳號"]);
                                command.Parameters.AddWithValue("@ID", row["userID"]);
                                int count = (int)command.ExecuteScalar();
                                if (count > 0)
                                {
                                    MessageBox.Show("已有相同帳號存在，請更改帳號。");
                                    return; // 如果已有相同帳號存在，則不進行更新
                                }
                            }

                            using (SqlCommand command = new SqlCommand(updateQuery, con, transaction))
                            {
                                // 設定參數值
                                command.Parameters.AddWithValue("@UserAcc", row["帳號"]);
                                command.Parameters.AddWithValue("@UserPass", row["密碼"]);
                                command.Parameters.AddWithValue("@ID", row["userID"]);
                                // 執行更新
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                    // 檢查是否有更改
                    if (i == 0)
                    {
                        MessageBox.Show("資料未更改");
                    }
                    else
                    {
                        // 提交交易
                        transaction.Commit();
                        // 更新成功，顯示訊息或執行其他操作
                        MessageBox.Show("資料已成功更新");
                    }
                }
                catch (Exception ex)
                {
                    // 發生錯誤，回滾交易
                    if (transaction != null)
                        transaction.Rollback();
                    // 處理錯誤
                    throw new Exception("資料庫錯誤: " + ex.Message);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
                LoadData2();
            }
        }
        private bool CheckRowChanged(DataRow row, params string[] columnNames)//判斷資料是否更改函式
        {
            foreach (string columnName in columnNames)
            {
                // 比较原始数据和新数据是否相同，如果不同则返回true，表示数据已更改
                if (!row[columnName, DataRowVersion.Original].Equals(row[columnName]))
                {
                    return true;
                }
            }
            return false; // 数据未更改
        }
    }
}
