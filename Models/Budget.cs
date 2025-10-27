using System;

namespace PersonalFinanceManager.Models
{
    public class Budget
    {
        public int BudgetID { get; set; }
        public int BudgetYear { get; set; }
        public int BudgetMonth { get; set; }
        public int CategoryID { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public decimal CompletionRate { get; set; }
        public decimal WarningThreshold { get; set; }
        public string ReminderMethod { get; set; }
        public string Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateTime { get; set; }
        public string Remark { get; set; }

        // 导航属性
        public string CategoryName { get; set; }

        public Budget()
        {
            BudgetYear = DateTime.Now.Year;
            BudgetMonth = DateTime.Now.Month;
            WarningThreshold = 80;
            ReminderMethod = "应用内";
            Status = "激活";
            CreateTime = DateTime.Now;
        }

        public void CalculateCompletionRate()
        {
            if (BudgetAmount > 0)
            {
                CompletionRate = (ActualAmount / BudgetAmount) * 100;
            }
            else
            {
                CompletionRate = 0;
            }
        }
    }
}