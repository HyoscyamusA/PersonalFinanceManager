using System;

namespace PersonalFinanceManager.Models
{
    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string CategoryType { get; set; } // "收入" 或 "支出"
        public int? ParentCategoryID { get; set; }
        public string Icon { get; set; }
        public string Color { get; set; }
        public decimal BudgetAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public int SortOrder { get; set; }
        public bool IsSystemPreset { get; set; }
        public string Description { get; set; }

        public Category()
        {
            SortOrder = 0;
            IsSystemPreset = false;
            BudgetAmount = 0;
            ActualAmount = 0;
        }
    }
}