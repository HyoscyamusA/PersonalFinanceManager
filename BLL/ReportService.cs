using System;
using System.Collections.Generic;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.DAL;

namespace PersonalFinanceManager.BLL
{
    public class ReportService
    {
        private readonly ReportRepository _reportRepository;
        private readonly TransactionService _transactionService;

        public ReportService()
        {
            _reportRepository = new ReportRepository();
            _transactionService = new TransactionService();
        }

        /// <summary>
        /// 获取收支统计
        /// </summary>
        public List<IncomeExpenseStat> GetIncomeExpenseStats(DateTime startDate, DateTime endDate, string groupBy = "day")
        {
            try
            {
                return _reportRepository.GetIncomeExpenseStats(startDate, endDate, groupBy);
            }
            catch (Exception ex)
            {
                throw new Exception($"获取收支统计失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取分类统计
        /// </summary>
        public List<CategoryStat> GetCategoryStats(DateTime startDate, DateTime endDate, string transactionType = "支出")
        {
            try
            {
                return _reportRepository.GetCategoryStats(startDate, endDate, transactionType);
            }
            catch (Exception ex)
            {
                throw new Exception($"获取分类统计失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取预算执行统计
        /// </summary>
        public List<BudgetStat> GetBudgetStats(int year, int month)
        {
            try
            {
                return _reportRepository.GetBudgetStats(year, month);
            }
            catch (Exception ex)
            {
                throw new Exception($"获取预算执行统计失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取月度汇总
        /// </summary>
        public List<MonthlySummary> GetMonthlySummaries(int year)
        {
            try
            {
                return _reportRepository.GetMonthlySummaries(year);
            }
            catch (Exception ex)
            {
                throw new Exception($"获取月度汇总失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取趋势分析
        /// </summary>
        public List<TrendAnalysis> GetTrendAnalysis(DateTime startDate, DateTime endDate, string periodType = "month")
        {
            try
            {
                return _reportRepository.GetTrendAnalysis(startDate, endDate, periodType);
            }
            catch (Exception ex)
            {
                throw new Exception($"获取趋势分析失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取账户余额变动历史
        /// </summary>
        public System.Data.DataTable GetAccountBalanceHistory(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _reportRepository.GetAccountBalanceHistory(startDate, endDate);
            }
            catch (Exception ex)
            {
                throw new Exception($"获取账户余额变动历史失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取统计概览
        /// </summary>
        public Dictionary<string, object> GetOverview(DateTime startDate, DateTime endDate)
        {
            var overview = new Dictionary<string, object>();

            try
            {
                // 总收入
                overview["TotalIncome"] = _transactionService.GetTotalIncome(startDate, endDate);

                // 总支出
                overview["TotalExpense"] = _transactionService.GetTotalExpense(startDate, endDate);

                // 净收入
                overview["NetAmount"] = (decimal)overview["TotalIncome"] - (decimal)overview["TotalExpense"];

                // 交易笔数
                var transactions = _transactionService.GetTransactionsByDateRange(startDate, endDate);
                overview["TransactionCount"] = transactions.Count;

                // 平均每日支出
                var days = (endDate - startDate).Days + 1;
                overview["AverageDailyExpense"] = days > 0 ? (decimal)overview["TotalExpense"] / days : 0;

                return overview;
            }
            catch (Exception ex)
            {
                throw new Exception($"获取统计概览失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 导出报表数据到CSV
        /// </summary>
        public string ExportToCsv(List<IncomeExpenseStat> data, string filename)
        {
            try
            {
                var csv = new System.Text.StringBuilder();

                // 添加标题行
                csv.AppendLine("日期,收入,支出,净收入");

                // 添加数据行
                foreach (var item in data)
                {
                    csv.AppendLine($"{item.Date:yyyy-MM-dd},{item.Income},{item.Expense},{item.NetAmount}");
                }

                // 保存文件
                string filePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", filename);
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(filePath));
                System.IO.File.WriteAllText(filePath, csv.ToString(), System.Text.Encoding.UTF8);

                return filePath;
            }
            catch (Exception ex)
            {
                throw new Exception($"导出CSV失败: {ex.Message}");
            }
        }
    }
}