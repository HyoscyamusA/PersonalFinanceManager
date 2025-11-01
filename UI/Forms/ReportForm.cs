using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.BLL;

namespace PersonalFinanceManager.UI.Forms
{
    public partial class ReportForm : Form
    {
        private ReportService _reportService;
        private DateTime _startDate;
        private DateTime _endDate;

        public ReportForm()
        {
            InitializeComponent();
            _reportService = new ReportService();
            _startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            _endDate = DateTime.Now;
            SetupControls();
        }

        private void SetupControls()
        {
            // 设置日期范围
            dtpStartDate.Value = _startDate;
            dtpEndDate.Value = _endDate;

            // 设置报表类型
            cmbReportType.Items.AddRange(new string[] {
                "收支统计",
                "分类统计",
                "预算执行",
                "月度汇总",
                "趋势分析"
            });
            cmbReportType.SelectedIndex = 0;

            // 设置图表类型
            cmbChartType.Items.AddRange(new string[] {
                "柱状图",
                "折线图",
                "饼图"
            });
            cmbChartType.SelectedIndex = 0;

            // 初始化图表
            InitializeChart();
        }

        private void InitializeChart()
        {
            chartReport.Series.Clear();
            chartReport.ChartAreas.Clear();

            var chartArea = new ChartArea();
            chartReport.ChartAreas.Add(chartArea);

            // 设置图表区域样式
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisY.MajorGrid.Enabled = true;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                _startDate = dtpStartDate.Value;
                _endDate = dtpEndDate.Value;

                if (_startDate > _endDate)
                {
                    MessageBox.Show("开始日期不能大于结束日期", "错误",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string reportType = cmbReportType.SelectedItem.ToString();
                string chartType = cmbChartType.SelectedItem.ToString();

                GenerateReport(reportType, chartType);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成报表失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GenerateReport(string reportType, string chartType)
        {
            chartReport.Series.Clear();

            switch (reportType)
            {
                case "收支统计":
                    GenerateIncomeExpenseReport(chartType);
                    break;
                case "分类统计":
                    GenerateCategoryReport(chartType);
                    break;
                case "预算执行":
                    GenerateBudgetReport(chartType);
                    break;
                case "月度汇总":
                    GenerateMonthlySummaryReport(chartType);
                    break;
                case "趋势分析":
                    GenerateTrendAnalysisReport(chartType);
                    break;
            }
        }

        private void GenerateIncomeExpenseReport(string chartType)
        {
            var stats = _reportService.GetIncomeExpenseStats(_startDate, _endDate, "day");

            // 显示数据表格
            dataGridViewReport.DataSource = stats;

            // 显示图表
            var seriesIncome = new Series("收入");
            var seriesExpense = new Series("支出");

            foreach (var stat in stats)
            {
                seriesIncome.Points.AddXY(stat.Date.ToString("MM-dd"), stat.Income);
                seriesExpense.Points.AddXY(stat.Date.ToString("MM-dd"), stat.Expense);
            }

            SetChartType(seriesIncome, chartType);
            SetChartType(seriesExpense, chartType);

            chartReport.Series.Add(seriesIncome);
            chartReport.Series.Add(seriesExpense);

            chartReport.Titles.Clear();
            chartReport.Titles.Add("收支统计图表");
        }

        private void GenerateCategoryReport(string chartType)
        {
            var stats = _reportService.GetCategoryStats(_startDate, _endDate, "支出");

            // 显示数据表格
            dataGridViewReport.DataSource = stats;

            // 显示饼图
            var series = new Series("分类支出");
            series.ChartType = SeriesChartType.Pie;

            foreach (var stat in stats)
            {
                series.Points.AddXY(stat.CategoryName, stat.Amount);
            }

            chartReport.Series.Clear();
            chartReport.Series.Add(series);

            chartReport.Titles.Clear();
            chartReport.Titles.Add("分类支出占比");
        }

        private void GenerateBudgetReport(string chartType)
        {
            var stats = _reportService.GetBudgetStats(DateTime.Now.Year, DateTime.Now.Month);

            // 显示数据表格
            dataGridViewReport.DataSource = stats;

            // 显示图表
            var seriesBudget = new Series("预算金额");
            var seriesActual = new Series("实际支出");

            foreach (var stat in stats)
            {
                seriesBudget.Points.AddXY(stat.CategoryName, stat.BudgetAmount);
                seriesActual.Points.AddXY(stat.CategoryName, stat.ActualAmount);
            }

            SetChartType(seriesBudget, "柱状图");
            SetChartType(seriesActual, "柱状图");

            chartReport.Series.Clear();
            chartReport.Series.Add(seriesBudget);
            chartReport.Series.Add(seriesActual);

            chartReport.Titles.Clear();
            chartReport.Titles.Add("预算执行情况");
        }

        private void GenerateMonthlySummaryReport(string chartType)
        {
            var summaries = _reportService.GetMonthlySummaries(DateTime.Now.Year);

            // 显示数据表格
            dataGridViewReport.DataSource = summaries;

            // 显示图表
            var seriesIncome = new Series("收入");
            var seriesExpense = new Series("支出");

            foreach (var summary in summaries)
            {
                string month = $"{summary.Year}-{summary.Month:D2}";
                seriesIncome.Points.AddXY(month, summary.TotalIncome);
                seriesExpense.Points.AddXY(month, summary.TotalExpense);
            }

            SetChartType(seriesIncome, chartType);
            SetChartType(seriesExpense, chartType);

            chartReport.Series.Clear();
            chartReport.Series.Add(seriesIncome);
            chartReport.Series.Add(seriesExpense);

            chartReport.Titles.Clear();
            chartReport.Titles.Add("月度汇总统计");
        }

        private void GenerateTrendAnalysisReport(string chartType)
        {
            var trends = _reportService.GetTrendAnalysis(_startDate, _endDate, "month");

            // 显示数据表格
            dataGridViewReport.DataSource = trends;

            // 显示图表
            var seriesIncome = new Series("收入");
            var seriesExpense = new Series("支出");
            var seriesNet = new Series("净收入");

            foreach (var trend in trends)
            {
                seriesIncome.Points.AddXY(trend.Period, trend.Income);
                seriesExpense.Points.AddXY(trend.Period, trend.Expense);
                seriesNet.Points.AddXY(trend.Period, trend.NetAmount);
            }

            SetChartType(seriesIncome, chartType);
            SetChartType(seriesExpense, chartType);
            SetChartType(seriesNet, chartType);

            chartReport.Series.Clear();
            chartReport.Series.Add(seriesIncome);
            chartReport.Series.Add(seriesExpense);
            chartReport.Series.Add(seriesNet);

            chartReport.Titles.Clear();
            chartReport.Titles.Add("收支趋势分析");
        }

        private void SetChartType(Series series, string chartType)
        {
            switch (chartType)
            {
                case "柱状图":
                    series.ChartType = SeriesChartType.Column;
                    break;
                case "折线图":
                    series.ChartType = SeriesChartType.Line;
                    break;
                case "饼图":
                    series.ChartType = SeriesChartType.Pie;
                    break;
            }
        }

        //private void btnExport_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (dataGridViewReport.DataSource == null)
        //        {
        //            MessageBox.Show("没有可导出的数据", "提示",
        //                MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            return;
        //        }

        //        using (SaveFileDialog saveDialog = new SaveFileDialog())
        //        {
        //            saveDialog.Filter = "CSV文件 (*.csv)|*.csv";
        //            saveDialog.FileName = $"财务报表_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

        //            if (saveDialog.ShowDialog() == DialogResult.OK)
        //            {
        //                // 这里可以添加导出CSV的逻辑
        //                MessageBox.Show("导出功能待实现", "提示",
        //                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"导出失败: {ex.Message}", "错误",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewReport.DataSource == null || dataGridViewReport.Rows.Count == 0)
                {
                    MessageBox.Show("没有可导出的数据", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                using (SaveFileDialog saveDialog = new SaveFileDialog())
                {
                    saveDialog.Filter = "CSV文件 (*.csv)|*.csv";
                    saveDialog.FileName = $"财务报表_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

                    if (saveDialog.ShowDialog() == DialogResult.OK)
                    {
                        using (var sw = new System.IO.StreamWriter(saveDialog.FileName, false, System.Text.Encoding.UTF8))
                        {
                            // 写入列标题
                            for (int i = 0; i < dataGridViewReport.Columns.Count; i++)
                            {
                                sw.Write(dataGridViewReport.Columns[i].HeaderText);
                                if (i < dataGridViewReport.Columns.Count - 1)
                                    sw.Write(",");
                            }
                            sw.WriteLine();

                            // 写入数据行
                            foreach (DataGridViewRow row in dataGridViewReport.Rows)
                            {
                                if (row.IsNewRow) continue; // 跳过新行
                                for (int i = 0; i < dataGridViewReport.Columns.Count; i++)
                                {
                                    var cellValue = row.Cells[i].Value?.ToString() ?? "";
                                    // 如果值中含逗号或引号，用双引号包起来，双引号内部再用双引号转义
                                    if (cellValue.Contains(",") || cellValue.Contains("\""))
                                        cellValue = $"\"{cellValue.Replace("\"", "\"\"")}\"";
                                    sw.Write(cellValue);
                                    if (i < dataGridViewReport.Columns.Count - 1)
                                        sw.Write(",");
                                }
                                sw.WriteLine();
                            }
                        }

                        MessageBox.Show("导出成功", "提示",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnOverview_Click(object sender, EventArgs e)
        {
            try
            {
                var overview = _reportService.GetOverview(_startDate, _endDate);

                string overviewText = $@"统计概览 ({_startDate:yyyy-MM-dd} 至 {_endDate:yyyy-MM-dd})

总收入: {overview["TotalIncome"]:C2}
总支出: {overview["TotalExpense"]:C2}
净收入: {overview["NetAmount"]:C2}
交易笔数: {overview["TransactionCount"]} 笔
平均每日支出: {overview["AverageDailyExpense"]:C2}";

                MessageBox.Show(overviewText, "统计概览",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"获取统计概览失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}