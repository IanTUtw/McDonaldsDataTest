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
    public partial class managerDataCheckFormN : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-4372HT1C\SQLEXPRESS;Initial Catalog=McDonaldData;User ID=Ian89;Password=ian121989;Integrated Security=False;");
        string connectionString = @"Data Source=LAPTOP-4372HT1C\SQLEXPRESS;Initial Catalog=McDonaldData;User ID=Ian89;Password=ian121989;Integrated Security=False;";
        public managerDataCheckFormN()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 資料查看tab
        /// </summary>
        private void managerDataCheckFormN_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            DataTable finalTable = GetCombinedData();
            gridControlM.DataSource = finalTable;
            gridViewM.Columns["月份"].Width = 35; // 設定指定欄位的寬度

        }
        private DataTable GetCombinedData()
        {
            DataTable finalTable = new DataTable();
            finalTable.Columns.Add("日期", typeof(DateTime));
            finalTable.Columns.Add("月份", typeof(int)); // 新增月份欄位
            finalTable.Columns.Add("門市", typeof(string));
            finalTable.Columns.Add("創建時間", typeof(DateTime));
            finalTable.Columns.Add("更新時間", typeof(DateTime));
            finalTable.Columns.Add("總銷售成本", typeof(decimal));
            finalTable.Columns.Add("總銷售金額", typeof(decimal));

            Dictionary<string, Dictionary<string, decimal>> productSales = new Dictionary<string, Dictionary<string, decimal>>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT n.numDate, s.storeName, p.productName, n.num, p.cost, p.price, n.createDate, n.updateDate
                         FROM numData n 
                         JOIN storeData s ON n.storeID = s.storeID
                         JOIN product p ON n.productID = p.productID
                         ORDER BY p.category";

                conn.Open();
                SqlCommand command = new SqlCommand(query, conn);
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
                    DateTime createDate = reader.GetDateTime(6);
                    DateTime updateDate = reader.GetDateTime(7);

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
                        newRow["創建時間"] = createDate;
                        newRow["更新時間"] = updateDate;
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

        private void girdControlOutput_Click(object sender, EventArgs e)//資料查看 匯出資料按鈕
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

            // 如果按下了儲存按鈕
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // 獲取當前gridControl
                    GridView gridView = gridControlM.MainView as GridView;

                    // 將 gridControl 中的資料輸出到 Excel
                    gridView.ExportToXlsx(saveFileDialog1.FileName);

                    MessageBox.Show("已成功儲存到桌面", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    throw new Exception("發生錯誤：" + ex.Message);
                }
            }
        }
        private void devDelBtn_Click(object sender, EventArgs e) //資料查看 刪除資料按鍵
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
                    int[] selectedRowHandles = gridViewM.GetSelectedRows();

                    foreach (int focusedRowHandle in selectedRowHandles)
                    {
                        // 獲取所選取資料行的日期和門市
                        DateTime numDate = Convert.ToDateTime(gridViewM.GetRowCellValue(focusedRowHandle, "日期"));
                        string storeName = gridViewM.GetRowCellValue(focusedRowHandle, "門市").ToString();

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

        private void searchBtn_Click(object sender, EventArgs e)//資料查看 搜尋鍵
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
                gridViewM.ActiveFilterString = filter;
            }
            else
            {
                MessageBox.Show("請選擇日期");
            }
        }

        private void monthRank_Click(object sender, EventArgs e)//資料查看 月排行鍵
        {
            gridViewM.SortInfo.Clear(); // 先清除之前的排序設定
            // 先按日期升序排序
            // 再按總銷售金額降序排序
            gridViewM.SortInfo.AddRange(new GridColumnSortInfo[]
            {
                new GridColumnSortInfo(gridViewM.Columns["月份"], DevExpress.Data.ColumnSortOrder.Ascending),
                new GridColumnSortInfo(gridViewM.Columns["總銷售金額"], DevExpress.Data.ColumnSortOrder.Descending)
            });
        }
        private void dayRank_Click(object sender, EventArgs e)//資料查看 日排行鍵
        {
            gridViewM.SortInfo.Clear(); // 先清除之前的排序設定
            gridViewM.SortInfo.Add(new GridColumnSortInfo(gridViewM.Columns["總銷售金額"], DevExpress.Data.ColumnSortOrder.Descending));//設定排序選擇
        }

        private void returnBtn_Click(object sender, EventArgs e)//資料查看 返回鍵
        {
            loginFormN loginform = new loginFormN();
            loginform.Show();
            this.Hide();
        }

        /// <summary>
        /// 基本資料tab
        /// </summary>
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

        int panelNow = 4;//判斷目前使用資料庫//門市1 使用者0 價格2
        private void storeButton_Click(object sender, EventArgs e)//門市按鍵
        {
            loadData1();
            addStoreProductBtn.Visible = true;
            gridView1.Columns["類別"].Width = 35; // 設定指定欄位的寬度
        }
        private void loadData1()//顯示門市資料
        {
            panelNow = 1;
            gridView1.Columns.Clear();
            delBtn.Visible = true;
            labelControl2.Visible = true;
            string sqlQuery = @"SELECT
                                    sd.storeID,
                                    sd.storeName AS 門市,
                                    sd.phone AS 門市電話,
                                    sd.address AS 地址,
                                    sd.storeLeader AS 負責人,
                                    sd.leaderPhone AS 負責人電話,
                                    p.category AS 類別,
                                    p.productName AS 產品,
                                    sd.createDate AS 創建時間,
                                    sd.createBy AS 創建者,
                                    sd.updateDate AS 更新時間,
                                    sd.updateBy AS 更新者
                               FROM storeData sd
                               JOIN storeProductData sp ON sd.storeID = sp.storeID
                               JOIN product p ON sp.productID = p.productID
                               ORDER BY sd.storeName, p.category";
            try
            {
                con.Open();

                // 建立資料讀取器
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                SqlDataReader reader = cmd.ExecuteReader();

                // 建立 DataTable
                DataTable dataTableS = new DataTable();
                dataTableS.Load(reader);

                // 將 DataTable 資料繫結到 gridControlM
                setGridControl.DataSource = dataTableS;
                // 將 gridViewM 設置為 gridControlM 的 MainView
                setGridControl.MainView = gridView1;

                con.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("資料庫錯誤: " + ex.Message);
            }

            gridView1.Columns["storeID"].Visible = false;
            gridView1.Columns["門市"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["類別"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["創建時間"].OptionsColumn.ReadOnly = true;//是否能編輯
            gridView1.Columns["創建者"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["更新時間"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["更新者"].OptionsColumn.ReadOnly = true;
        }

        private void productBasicBtn_Click(object sender, EventArgs e)//產品基本資料Btn
        {
            addStoreProductBtn.Visible = false;
            loadData2();
        }
        private void loadData2()//顯示產品基本資料
        {
            panelNow = 2;
            gridView1.Columns.Clear();
            string priceQuery = @"SELECT 
	                                productID,
	                                category AS 分類,
	                                productName AS 產品名稱,
	                                cost AS 成本,
	                                price AS 售價,
	                                create_date AS 創建時間,
	                                update_date AS 更新時間
                                FROM 
	                                product
                                ORDER BY
                                    category";

            try//導入資料進gridControl
            {
                con.Open();

                // 建立資料讀取器
                SqlCommand cmd = new SqlCommand(priceQuery, con);
                SqlDataReader reader = cmd.ExecuteReader();

                // 建立 DataTable
                DataTable dataTableS = new DataTable();
                dataTableS.Load(reader);

                // 將 DataTable 資料繫結到 gridControlM
                setGridControl.DataSource = dataTableS;
                // 將 gridViewM 設置為 gridControlM 的 MainView
                setGridControl.MainView = gridView1;

                // 將單個欄位內容靠右對齊
                gridView1.Columns["成本"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                gridView1.Columns["售價"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                con.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("資料庫錯誤: " + ex.Message);
            }
            gridView1.Columns["productID"].Visible = false;
            gridView1.Columns["分類"].OptionsColumn.ReadOnly = true;//是否能編輯
            gridView1.Columns["創建時間"].OptionsColumn.ReadOnly = true;//是否能編輯
            gridView1.Columns["更新時間"].OptionsColumn.ReadOnly = true;//是否能編輯
        }

        private void userBtn1_Click(object sender, EventArgs e)//用戶按鍵
        {
            addStoreProductBtn.Visible = false;
            loadData0();
        }
        private void loadData0()
        {
            panelNow = 0;
            gridView1.Columns.Clear();
            delBtn.Visible = true;
            labelControl2.Visible = true;
            string sqlQuery = @"SELECT 
                                    ud.userID,
                                    sd.storeName AS 門市,
                                    ud.userAcc AS 帳號,
                                    ud.userPass AS 密碼,
                                    ud.role AS 角色,
                                    ud.create_date AS 創建時間,
                                    ud.update_date AS 更新時間
                                FROM
                                    userData ud
                                LEFT JOIN
                                    storeData sd ON ud.storeID = sd.storeID
                                ORDER BY
                                    ud.create_date, sd.storeName";

            try//導入資料進gridControl
            {
                con.Open();

                // 建立資料讀取器
                SqlCommand cmd = new SqlCommand(sqlQuery, con);
                SqlDataReader reader = cmd.ExecuteReader();

                // 建立 DataTable
                DataTable dataTableS = new DataTable();
                dataTableS.Load(reader);

                // 將 DataTable 資料繫結到 gridControlM
                setGridControl.DataSource = dataTableS;
                // 將 gridViewM 設置為 gridControlM 的 MainView
                setGridControl.MainView = gridView1;

                con.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("資料庫錯誤: " + ex.Message);
            }
            gridView1.Columns["userID"].Visible = false;
            gridView1.Columns["門市"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["創建時間"].OptionsColumn.ReadOnly = true;
            gridView1.Columns["更新時間"].OptionsColumn.ReadOnly = true;
        }

        private void updateBtn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("確定要更新資料嗎？", "確認更新", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                SqlTransaction transaction = null; // 創建交易對象

                try
                {
                    // 取得當前 GridControl 中的資料
                    DataTable dataTableS = (DataTable)setGridControl.DataSource;
                    con.Open();
                    transaction = con.BeginTransaction(); // 開始交易

                    if (panelNow == 0)//使用者0
                    {
                        int i = 0;
                        // 遍歷每一列，進行更新
                        foreach (DataRow row in dataTableS.Rows)
                        {
                            // 檢查該行資料是否有更改
                            if (CheckRowChanged(row, "帳號", "密碼", "角色"))
                            {
                                i++;
                                // 更新語句，使用參數化查詢以避免 SQL 注入攻擊
                                string checkAccQuery = "SELECT COUNT(*) FROM userData WHERE userAcc = @UserAcc AND userID != @ID"; // 檢查是否有其他相同帳號
                                string updateQuery = "UPDATE userData SET userAcc = @UserAcc, userPass = @UserPass, role = @Role, update_date = GETDATE() WHERE userID = @ID";

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
                                    command.Parameters.AddWithValue("@Role", row["角色"]);
                                    command.Parameters.AddWithValue("@ID", row["userID"]);
                                    // 執行更新
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                        if (i == 0)
                        {
                            MessageBox.Show("資料未更改");
                        }
                        else
                        {
                            // 更新成功，顯示訊息或執行其他操作
                            MessageBox.Show("資料已成功更新至 SQL Server。");
                        }
                    }
                    else if (panelNow == 1)//門市1
                    {
                        int i = 0;
                        foreach (DataRow row in dataTableS.Rows)
                        {
                            // 檢查該行資料是否有更改
                            if (CheckRowChanged(row, "地址", "門市電話", "負責人", "負責人電話"))
                            {
                                i++;
                                // 更新語句，使用參數化查詢以避免 SQL 注入攻擊
                                string updateQuery = "UPDATE storeData SET storeName = @StoreName, address = @Address , phone = @Phone, storeLeader = @StoreLeader, leaderPhone = @LeaderPhone, updateDate = GETDATE(),updateBy = @UpdateBy WHERE storeID = @StoreID";
                                string update_by = Program.Username;

                                using (SqlCommand command = new SqlCommand(updateQuery, con, transaction))
                                {
                                    // 設定參數值
                                    command.Parameters.AddWithValue("@StoreName", row["門市"]);
                                    command.Parameters.AddWithValue("@Address", row["地址"]);
                                    command.Parameters.AddWithValue("@Phone", row["門市電話"]);
                                    command.Parameters.AddWithValue("@StoreLeader", row["負責人"]);
                                    command.Parameters.AddWithValue("@LeaderPhone", row["負責人電話"]);
                                    command.Parameters.AddWithValue("@UpdateBy", row["更新者"]);
                                    command.Parameters.AddWithValue("@StoreID", row["storeID"]);
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                        if (i == 0)
                        {
                            MessageBox.Show("資料未更改");
                        }
                        else
                        {
                            // 更新成功，顯示訊息或執行其他操作
                            MessageBox.Show("資料已成功更新至 SQL Server。");
                        }
                    }
                    else// 價格2
                    {
                        int i = 0;
                        // 遍歷每一列，進行更新
                        foreach (DataRow row in dataTableS.Rows)
                        {
                            // 檢查該行資料是否有更改
                            if (CheckRowChanged(row,"產品名稱", "成本", "售價"))
                            {
                                i++;
                                // 更新語句，使用參數化查詢以避免 SQL 注入攻擊
                                string updateQuery = "UPDATE product SET productName = @ProductName, cost = @Cost, price = @Price, update_date = GETDATE() WHERE productID = @ID";
                                string update_by = Program.Username;

                                using (SqlCommand command = new SqlCommand(updateQuery, con, transaction))
                                {
                                    // 設定參數值
                                    command.Parameters.AddWithValue("@ProductName", row["產品名稱"]);
                                    command.Parameters.AddWithValue("@Cost", row["成本"]);
                                    command.Parameters.AddWithValue("@Price", row["售價"]);
                                    command.Parameters.AddWithValue("@ID", row["productID"]);
                                    // 執行更新
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                        if (i == 0)
                        {
                            MessageBox.Show("資料未更改");
                        }
                        else
                        {
                            // 更新成功，顯示訊息或執行其他操作//不要用
                            MessageBox.Show("資料已成功更新");
                        }
                    }

                    transaction.Commit(); // 提交交易
                }
                catch (Exception ex)
                {
                    // 發生錯誤，回滾交易
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

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

                // 根據不同的 panelNow 加載不同的資料
                if (panelNow == 0)
                    loadData0();
                else if (panelNow == 1)
                    loadData1();
                else
                    loadData2();
            }
        }



        private void addUserBtn_Click(object sender, EventArgs e)//基本資料頁面 增加用戶鍵
        {
            signUpForm signUp = new signUpForm();
            signUp.Show();
            this.Hide();
        }

        private void addStoreBtn_Click(object sender, EventArgs e)//基本資料頁面 增加門市鍵
        {
            storeSignUpForm store = new storeSignUpForm();
            store.Show();
            this.Hide();
        }

        private void simpleButton1_Click(object sender, EventArgs e)//基本資料 刪除鍵
        {
            if (panelNow != 4)
            {
                string confirmMessage = "";
                if (panelNow == 0)
                    confirmMessage = "確定要刪除使用者?";
                else if (panelNow == 1)
                    confirmMessage = "確認刪除該門市的產品?";
                else//panel2
                    confirmMessage = "確認刪除產品？產品相關的資料將會被刪除";

                DialogResult result = MessageBox.Show(confirmMessage, "確認刪除", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    SqlTransaction transaction = null; // 創建交易對象

                    if (gridView1.FocusedRowHandle >= 0)
                    {
                        try
                        {
                            con.Open();
                            transaction = con.BeginTransaction();

                            // 獲取所選取的所有資料行的索引
                            int[] selectedRowHandles = gridView1.GetSelectedRows();

                            foreach (int focusedRowHandle in selectedRowHandles)
                            {
                                Guid idToDelete;
                                if (panelNow == 0)//0使用者
                                {
                                    idToDelete = (Guid)gridView1.GetRowCellValue(focusedRowHandle, "userID");
                                    // 執行 SQL 刪除命令
                                    string deleteCmd = "DELETE FROM userData WHERE userID = @IdToDelete";
                                    // 刪除 userData 表格中的資料
                                    using (SqlCommand deleteUserDataCommand = new SqlCommand(deleteCmd, con, transaction)) // 將交易傳遞給命令
                                    {
                                        deleteUserDataCommand.Parameters.AddWithValue("@IdToDelete", idToDelete);
                                        deleteUserDataCommand.ExecuteNonQuery();
                                    }
                                }
                                else if (panelNow == 1)//門市1
                                {
                                    idToDelete = (Guid)gridView1.GetRowCellValue(focusedRowHandle, "productID");

                                    // 刪除與該門市相關的 storeProductData 記錄
                                    string deleteStoreProductCmd = "DELETE FROM storeProductData WHERE productID = @IdToDelete";
                                    using (SqlCommand deleteStoreProductCommand = new SqlCommand(deleteStoreProductCmd, con, transaction))
                                    {
                                        deleteStoreProductCommand.Parameters.AddWithValue("@IdToDelete", idToDelete);
                                        deleteStoreProductCommand.ExecuteNonQuery();
                                    }
                                    string deleteNumDataCmd = "DELETE FROM numData WHERE storeIDproductID = @idTodelete";
                                    using (SqlCommand deleteNumDataCommand = new SqlCommand(deleteNumDataCmd, con, transaction))
                                    {
                                        deleteNumDataCommand.Parameters.AddWithValue("@IdToDelete", idToDelete);
                                        deleteNumDataCommand.ExecuteNonQuery();
                                    }
                                }
                                else//價格2
                                {
                                    idToDelete = (Guid)gridView1.GetRowCellValue(focusedRowHandle, "productID");

                                    // 先刪除 storeProductData 表格中的相關資料
                                    string deleteStoreProductDataCmd = "DELETE FROM storeProductData WHERE productID = @IdToDelete";
                                    using (SqlCommand deleteStoreProductDataCommand = new SqlCommand(deleteStoreProductDataCmd, con, transaction))
                                    {
                                        deleteStoreProductDataCommand.Parameters.AddWithValue("@IdToDelete", idToDelete);
                                        deleteStoreProductDataCommand.ExecuteNonQuery();
                                    }

                                    // 再刪除 product 表格中的資料
                                    string deleteProductCmd = "DELETE FROM product WHERE productID = @IdToDelete";
                                    using (SqlCommand deleteProductCommand = new SqlCommand(deleteProductCmd, con, transaction))
                                    {
                                        deleteProductCommand.Parameters.AddWithValue("@IdToDelete", idToDelete);
                                        deleteProductCommand.ExecuteNonQuery();
                                    }
                                    string deleteNumDataCmd = "DELETE FROM numData WHERE storeID = @idTodelete";
                                    using (SqlCommand deleteNumDataCommand = new SqlCommand(deleteNumDataCmd, con, transaction))
                                    {
                                        deleteNumDataCommand.Parameters.AddWithValue("@IdToDelete", idToDelete);
                                        deleteNumDataCommand.ExecuteNonQuery();
                                    }
                                }
                            }

                            // 提交交易
                            transaction.Commit();
                            MessageBox.Show("選取的資料已成功刪除");
                        }
                        catch (Exception ex)
                        {
                            // 發生錯誤，回滾交易
                            if (transaction != null)
                            {
                                transaction.Rollback();
                            }

                            // 處理錯誤
                            throw new Exception("資料庫錯誤: " + ex.Message);
                        }
                        finally
                        {
                            con.Close();
                        }
                        if (panelNow == 0)
                            loadData0();
                        else if (panelNow == 1)
                            loadData1();
                        else
                            loadData2();
                    }
                    else
                    {
                        MessageBox.Show("請選擇要刪除的資料行。");
                    }
                }
            }
            else
            {
                MessageBox.Show("請先選擇呈現的資料");
            }
        }

        private void productBtn_Click(object sender, EventArgs e)
        {
            addFoodItemsForm addItems = new addFoodItemsForm();
            addItems.Show();
            this.Hide();
        }

        private void addStoreProductBtn_Click(object sender, EventArgs e)
        {
            addStoreProductForm add = new addStoreProductForm();
            add.Show();
            this.Hide();
        }

        private void chartBtn_Click(object sender, EventArgs e)
        {
            chartForm chart = new chartForm();
            chart.Show();
            this.Hide();
        }

    }
}
