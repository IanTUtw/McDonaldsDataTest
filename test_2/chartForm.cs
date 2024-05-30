using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraCharts;

namespace test_2
{
    public partial class chartForm : Form
    {
        string connectionString = @"Data Source=LAPTOP-4372HT1C\SQLEXPRESS;Initial Catalog=McDonaldData;User ID=Ian89;Password=ian121989;Integrated Security=False;";

        DataTable finalTable = new DataTable(); // 將最終表格放在類別範圍內

        public chartForm()
        {
            InitializeComponent();
        }

        private void chartForm_Load(object sender, EventArgs e)
        {
            // 隱藏圖表
            chartControl1.Visible = false;
        }

        private void ConfigureChart(DataTable finalTable)
        {
            // 清除現有的系列
            chartControl1.Series.Clear();

            // 創建一個堆疊柱形系列
            Series series1 = new Series("總銷售成本", ViewType.StackedBar);
            Series series2 = new Series("總銷售金額", ViewType.StackedBar);

            // 將 finalTable 中的數據添加到系列中
            foreach (DataRow row in finalTable.Rows)
            {
                string storeName = row["門市"].ToString();
                decimal totalCost = (decimal)row["總銷售成本"];
                decimal totalSale = (decimal)row["總銷售金額"];

                series1.Points.Add(new SeriesPoint(storeName, totalCost));
                series2.Points.Add(new SeriesPoint(storeName, totalSale));
            }


            // 將系列添加到圖表中
            chartControl1.Series.AddRange(new Series[] { series1, series2 });

            // 設置圖表標題
            chartControl1.Titles.Clear();
            chartControl1.Titles.Add(new ChartTitle { Text = "銷售營利比例" });

            // 設置 X 軸標題
            ((XYDiagram)chartControl1.Diagram).AxisX.Title.Text = "門市";

            // 設置 Y 軸標題
            ((XYDiagram)chartControl1.Diagram).AxisY.Title.Text = "總金額(元)";
        }

        private DataTable GetCombinedDataForChart1()//銷售營利比例圖 資料
        {
            // 清空所有資料行
            finalTable.Columns.Clear();
            finalTable.Columns.Add("日期", typeof(DateTime)); // 新增月份欄位
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
             ORDER BY n.numDate";

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

                    DataRow newRow;
                    DataRow[] existingRows = finalTable.Select(string.Format("日期 = '{0}' AND 門市 = '{1}'", numDate.ToString("yyyy-MM-dd"), storeName));

                    if (existingRows.Length > 0)
                    {
                        newRow = existingRows[0];
                    }
                    else
                    {
                        newRow = finalTable.NewRow();
                        newRow["日期"] = numDate;
                        newRow["門市"] = storeName;
                        newRow["創建時間"] = createDate;
                        newRow["更新時間"] = updateDate;
                        finalTable.Rows.Add(newRow);
                    }

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


        private ChartDataSource GetChartDataSource(DataTable finalTable)
        {
            ChartDataSource chartDataSource = new ChartDataSource();

            // 根據門市名稱
            foreach (DataRow row in finalTable.Rows)
            {
                string storeName = row["門市"].ToString();
                decimal totalSale = (decimal)row["總銷售金額"];
                chartDataSource.AppendData(storeName, totalSale);
            }

            return chartDataSource;
        }

        public class ChartDataSource
        {
            private Dictionary<string, decimal> data = new Dictionary<string, decimal>();

            public void AppendData(string argumentValue, decimal valueY)
            {
                data[argumentValue] = valueY;
            }

            public List<string> GetArgumentValues()
            {
                return data.Keys.ToList();
            }

            public List<decimal> GetValueYValues()
            {
                return data.Values.ToList();
            }
        }

        private void returnBtn_Click(object sender, EventArgs e)
        {
            managerDataCheckFormN manager = new managerDataCheckFormN();
            manager.Show();
            this.Hide();
        }

        private void storeFigureBtn_Click(object sender, EventArgs e)//銷售營利占比Btn
        {
            finalTable.Clear(); // 清空先前的數據
            // 獲取第一個圖表的資料
            DataTable chart1Data = GetCombinedDataForChart1();

            // 設置並顯示第一個圖表
            ConfigureChart(chart1Data);
            chartControl1.Visible = true;
        }

        private void productPercentBtn_Click(object sender, EventArgs e)//產品銷售占比Btn
        {
            // 清空先前的數據
            finalTable.Clear();

            // 獲取產品銷售數據
            Dictionary<string, decimal> productSalesData = GetProductSales();

            // 設置並顯示產品銷售佔比圖
            ShowProductSalesPieChart(productSalesData);
        }
        private Dictionary<string, decimal> GetProductSales()
        {
            Dictionary<string, decimal> productSales = new Dictionary<string, decimal>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                List<string> productNames = GetProductNames();

                foreach (string productName in productNames)
                {
                    productSales[productName] = 0; // 初始化每個產品的銷售數量為0
                }

                string query = @"SELECT p.productName, SUM(n.num) AS TotalNums
                         FROM numData n 
                         JOIN product p ON n.productID = p.productID
                         GROUP BY p.productName";

                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string productName = reader.GetString(0);
                    int num = reader.GetInt32(1);

                    // 將產品名稱和數量保存到 Dictionary 中
                    productSales[productName] = num;
                }

                reader.Close();
            }

            return productSales;
        }

        private DataTable GetProductSalesData()//產品銷售占比 資料
        {
            DataTable finalTable = new DataTable();
            finalTable.Columns.Add("產品名稱", typeof(string));
            finalTable.Columns.Add("數量", typeof(decimal));

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                List<string> productNames = GetProductNames();

                foreach (string productName in productNames)
                {
                    DataRow newRow = finalTable.NewRow();
                    newRow["產品名稱"] = productName;
                    newRow["數量"] = 0; // 初始化數量為0
                    finalTable.Rows.Add(newRow);
                }

                string query = @"SELECT p.productName, n.num
                         FROM numData n 
                         JOIN product p ON n.productID = p.productID";

                SqlCommand command = new SqlCommand(query, conn);
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string productName = reader.GetString(0);
                    int num = reader.GetInt32(1);

                    // 在 finalTable 中找到對應產品名稱的行
                    DataRow[] rows = finalTable.Select("產品名稱 = '" + productName + "'");

                    if (rows.Length > 0)
                    {
                        // 將數量加到該產品的數量中
                        rows[0]["數量"] = Convert.ToDecimal(rows[0]["數量"]) + num;
                    }
                }

                reader.Close();
            }

            return finalTable;
        }

        private void ShowProductSalesPieChart(Dictionary<string, decimal> productSalesData)
        {
            // 清除現有的系列
            chartControl1.Series.Clear();

            // 創建一個新的系列
            Series series = new Series("產品銷售佔比", ViewType.Pie);

            // 將產品銷售數據添加到系列中
            foreach (var pair in productSalesData)
            {
                string productName = pair.Key;
                decimal totalSales = pair.Value;

                // 將產品名稱和銷售數量添加到系列中
                series.Points.Add(new SeriesPoint(productName, totalSales));
            }

            // 將系列添加到圖表中
            chartControl1.Series.Add(series);

            // 設置圖表標題
            chartControl1.Titles.Clear();
            chartControl1.Titles.Add(new ChartTitle { Text = "各項產品銷售佔比" });

            // 設置圖例的可見性為真（顯示圖例）
            chartControl1.Legend.Visible = true;

            // 顯示圖表
            chartControl1.Visible = true;
        }

        private List<string> GetProductNames()
        {
            List<string> productNames = new List<string>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT productName FROM product";
                SqlCommand command = new SqlCommand(query, conn);
                conn.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    productNames.Add(reader.GetString(0));
                }
                reader.Close();
            }

            return productNames;
        }
    }
}
