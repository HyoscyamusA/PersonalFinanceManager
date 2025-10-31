using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models
{

        public class IncomeExpenseStat
        {
            public DateTime Date { get; set; }
            public decimal Income { get; set; }
            public decimal Expense { get; set; }
            public decimal NetAmount => Income - Expense;
        }

        /// <summary>
        /// 分类统计模型
        /// </summary>
        public class CategoryStat
        {
            public string CategoryName { get; set; }
            public decimal Amount { get; set; }
            public decimal Percentage { get; set; }
            public string TransactionType { get; set; }
        }

        /// <summary>
        /// 预算执行统计
        /// </summary>
        public class BudgetStat
        {
            public string CategoryName { get; set; }
            public decimal BudgetAmount { get; set; }
            public decimal ActualAmount { get; set; }
            public decimal CompletionRate { get; set; }
            public string Status { get; set; }
        }

        /// <summary>
        /// 月度统计汇总
        /// </summary>
        public class MonthlySummary
        {
            public int Year { get; set; }
            public int Month { get; set; }
            public decimal TotalIncome { get; set; }
            public decimal TotalExpense { get; set; }
            public decimal NetAmount => TotalIncome - TotalExpense;
            public int TransactionCount { get; set; }
        }

        /// <summary>
        /// 趋势分析数据
        /// </summary>
        public class TrendAnalysis
        {
            public string Period { get; set; } // 如 "2025-10", "2025-Q4", "2025"
            public decimal Income { get; set; }
            public decimal Expense { get; set; }
            public decimal NetAmount => Income - Expense;
            public decimal GrowthRate { get; set; } // 增长率
        }
    
}
