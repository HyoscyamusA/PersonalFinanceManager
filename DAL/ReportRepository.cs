using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using PersonalFinanceManager.Models;

namespace PersonalFinanceManager.DAL
{
    public class ReportRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public ReportRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        /// <summary>
        /// 获取收支统计（按日、月、年）
        /// </summary>
        public List<IncomeExpenseStat> GetIncomeExpenseStats(DateTime startDate, DateTime endDate, string groupBy = "day")
        {
            var stats = new List<IncomeExpenseStat>();

            // 修复：使用传统的 switch 语句替代 C# 8.0 的递归模式
            string dateFormat;
            switch (groupBy.ToLower())
            {
                case "month":
                    dateFormat = "%Y-%m";
                    break;
                case "year":
                    dateFormat = "%Y";
                    break;
                default:
                    dateFormat = "%Y-%m-%d"; // 默认按天
                    break;
            }

            string sql = $@"
                SELECT 
                    DATE_FORMAT(TransactionTime, '{dateFormat}') as PeriodDate,
                    SUM(CASE WHEN TransactionType = '收入' THEN Amount ELSE 0 END) as Income,
                    SUM(CASE WHEN TransactionType = '支出' THEN Amount ELSE 0 END) as Expense
                FROM Transactions 
                WHERE TransactionTime BETWEEN @StartDate AND @EndDate
                GROUP BY DATE_FORMAT(TransactionTime, '{dateFormat}')
                ORDER BY PeriodDate";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@StartDate", startDate),
                new MySqlParameter("@EndDate", endDate)
            };

            var dataTable = _dbHelper.ExecuteQuery(sql, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                string periodDate = row["PeriodDate"].ToString();
                DateTime date;

                if (groupBy == "month")
                {
                    date = DateTime.Parse(periodDate + "-01");
                }
                else if (groupBy == "year")
                {
                    date = DateTime.Parse(periodDate + "-01-01");
                }
                else
                {
                    date = DateTime.Parse(periodDate);
                }

                var stat = new IncomeExpenseStat
                {
                    Date = date,
                    Income = Convert.ToDecimal(row["Income"]),
                    Expense = Convert.ToDecimal(row["Expense"])
                };
                stats.Add(stat);
            }

            return stats;
        }

        /// <summary>
        /// 获取分类统计
        /// </summary>
        public List<CategoryStat> GetCategoryStats(DateTime startDate, DateTime endDate, string transactionType = "支出")
        {
            var stats = new List<CategoryStat>();

            string sql = @"
                SELECT 
                    c.CategoryName,
                    SUM(t.Amount) as TotalAmount,
                    (SUM(t.Amount) / (SELECT SUM(Amount) FROM Transactions 
                                     WHERE TransactionTime BETWEEN @StartDate AND @EndDate 
                                     AND TransactionType = @TransactionType)) * 100 as Percentage
                FROM Transactions t
                INNER JOIN Categories c ON t.CategoryID = c.CategoryID
                WHERE t.TransactionTime BETWEEN @StartDate AND @EndDate 
                AND t.TransactionType = @TransactionType
                GROUP BY c.CategoryID, c.CategoryName
                ORDER BY TotalAmount DESC";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@StartDate", startDate),
                new MySqlParameter("@EndDate", endDate),
                new MySqlParameter("@TransactionType", transactionType)
            };

            var dataTable = _dbHelper.ExecuteQuery(sql, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                var stat = new CategoryStat
                {
                    CategoryName = row["CategoryName"].ToString(),
                    Amount = Convert.ToDecimal(row["TotalAmount"]),
                    Percentage = Convert.ToDecimal(row["Percentage"]),
                    TransactionType = transactionType
                };
                stats.Add(stat);
            }

            return stats;
        }

        /// <summary>
        /// 获取预算执行统计
        /// </summary>
        public List<BudgetStat> GetBudgetStats(int year, int month)
        {
            var stats = new List<BudgetStat>();

            string sql = @"
                SELECT 
                    c.CategoryName,
                    b.BudgetAmount,
                    b.ActualAmount,
                    b.CompletionRate,
                    b.Status
                FROM Budgets b
                INNER JOIN Categories c ON b.CategoryID = c.CategoryID
                WHERE b.BudgetYear = @Year AND b.BudgetMonth = @Month
                ORDER BY b.CompletionRate DESC";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@Year", year),
                new MySqlParameter("@Month", month)
            };

            var dataTable = _dbHelper.ExecuteQuery(sql, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                var stat = new BudgetStat
                {
                    CategoryName = row["CategoryName"].ToString(),
                    BudgetAmount = Convert.ToDecimal(row["BudgetAmount"]),
                    ActualAmount = Convert.ToDecimal(row["ActualAmount"]),
                    CompletionRate = Convert.ToDecimal(row["CompletionRate"]),
                    Status = row["Status"].ToString()
                };
                stats.Add(stat);
            }

            return stats;
        }

        /// <summary>
        /// 获取月度汇总统计
        /// </summary>
        public List<MonthlySummary> GetMonthlySummaries(int year)
        {
            var summaries = new List<MonthlySummary>();

            string sql = @"
                SELECT 
                    YEAR(TransactionTime) as Year,
                    MONTH(TransactionTime) as Month,
                    SUM(CASE WHEN TransactionType = '收入' THEN Amount ELSE 0 END) as TotalIncome,
                    SUM(CASE WHEN TransactionType = '支出' THEN Amount ELSE 0 END) as TotalExpense,
                    COUNT(*) as TransactionCount
                FROM Transactions 
                WHERE YEAR(TransactionTime) = @Year
                GROUP BY YEAR(TransactionTime), MONTH(TransactionTime)
                ORDER BY Year, Month";

            var parameter = new MySqlParameter("@Year", year);

            var dataTable = _dbHelper.ExecuteQuery(sql, new[] { parameter });
            foreach (DataRow row in dataTable.Rows)
            {
                var summary = new MonthlySummary
                {
                    Year = Convert.ToInt32(row["Year"]),
                    Month = Convert.ToInt32(row["Month"]),
                    TotalIncome = Convert.ToDecimal(row["TotalIncome"]),
                    TotalExpense = Convert.ToDecimal(row["TotalExpense"]),
                    TransactionCount = Convert.ToInt32(row["TransactionCount"])
                };
                summaries.Add(summary);
            }

            return summaries;
        }

        /// <summary>
        /// 获取趋势分析数据
        /// </summary>
        public List<TrendAnalysis> GetTrendAnalysis(DateTime startDate, DateTime endDate, string periodType = "month")
        {
            var trends = new List<TrendAnalysis>();

            // 修复：使用传统的 switch 语句替代 C# 8.0 的递归模式
            string periodFormat;
            switch (periodType.ToLower())
            {
                case "quarter":
                    periodFormat = "%Y-Q%q";
                    break;
                case "year":
                    periodFormat = "%Y";
                    break;
                default:
                    periodFormat = "%Y-%m"; // 默认按月
                    break;
            }

            string sql = $@"
                SELECT 
                    DATE_FORMAT(TransactionTime, '{periodFormat}') as Period,
                    SUM(CASE WHEN TransactionType = '收入' THEN Amount ELSE 0 END) as Income,
                    SUM(CASE WHEN TransactionType = '支出' THEN Amount ELSE 0 END) as Expense
                FROM Transactions 
                WHERE TransactionTime BETWEEN @StartDate AND @EndDate
                GROUP BY DATE_FORMAT(TransactionTime, '{periodFormat}')
                ORDER BY Period";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@StartDate", startDate),
                new MySqlParameter("@EndDate", endDate)
            };

            var dataTable = _dbHelper.ExecuteQuery(sql, parameters);

            decimal previousNetAmount = 0;
            bool isFirst = true;

            foreach (DataRow row in dataTable.Rows)
            {
                var income = Convert.ToDecimal(row["Income"]);
                var expense = Convert.ToDecimal(row["Expense"]);
                var netAmount = income - expense;

                var trend = new TrendAnalysis
                {
                    Period = row["Period"].ToString(),
                    Income = income,
                    Expense = expense
                };

                // 计算增长率
                if (!isFirst && previousNetAmount != 0)
                {
                    trend.GrowthRate = ((netAmount - previousNetAmount) / Math.Abs(previousNetAmount)) * 100;
                }
                else
                {
                    trend.GrowthRate = 0;
                }

                trends.Add(trend);
                previousNetAmount = netAmount;
                isFirst = false;
            }

            return trends;
        }

        /// <summary>
        /// 获取账户余额变动统计
        /// </summary>
        public DataTable GetAccountBalanceHistory(DateTime startDate, DateTime endDate)
        {
            string sql = @"
                SELECT 
                    a.AccountName,
                    DATE(t.TransactionTime) as TransactionDate,
                    t.TransactionType,
                    c.CategoryName,
                    t.Amount,
                    t.Remark
                FROM Transactions t
                INNER JOIN Accounts a ON t.AccountID = a.AccountID
                INNER JOIN Categories c ON t.CategoryID = c.CategoryID
                WHERE t.TransactionTime BETWEEN @StartDate AND @EndDate
                ORDER BY t.TransactionTime DESC";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@StartDate", startDate),
                new MySqlParameter("@EndDate", endDate)
            };

            return _dbHelper.ExecuteQuery(sql, parameters);
        }
    }
}