//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PersonalFinanceManager.BLL
//{
//    internal class BudgetService
//    {
//    }
//}
using System;
using System.Collections.Generic;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.DAL;

namespace PersonalFinanceManager.BLL
{
    public class BudgetService
    {
        private readonly BudgetRepository _budgetRepository;

        public BudgetService()
        {
            _budgetRepository = new BudgetRepository();
        }

        public bool SetBudget(Budget budget)
        {
            if (budget.BudgetAmount <= 0)
                throw new ArgumentException("预算金额必须大于0");

            if (budget.CategoryID <= 0)
                throw new ArgumentException("类别ID无效");

            if (budget.BudgetYear < 2000 || budget.BudgetYear > 2100)
                throw new ArgumentException("预算年份无效");

            if (budget.BudgetMonth < 1 || budget.BudgetMonth > 12)
                throw new ArgumentException("预算月份无效");

            try
            {
                return _budgetRepository.SetBudget(budget);
            }
            catch (Exception ex)
            {
                throw new Exception($"设置预算失败: {ex.Message}");
            }
        }

        public List<Budget> GetBudgetsByMonth(int year, int month)
        {
            if (year < 2000 || year > 2100)
                throw new ArgumentException("年份无效");

            if (month < 1 || month > 12)
                throw new ArgumentException("月份无效");

            try
            {
                return _budgetRepository.GetBudgetsByMonth(year, month);
            }
            catch (Exception ex)
            {
                throw new Exception($"获取预算失败: {ex.Message}");
            }
        }

        public List<Budget> GetBudgetWarnings()
        {
            try
            {
                return _budgetRepository.GetBudgetWarnings();
            }
            catch (Exception ex)
            {
                throw new Exception($"获取预算预警失败: {ex.Message}");
            }
        }
        // 在 BudgetService 类中添加以下方法
        public bool DeleteBudget(int budgetId)
        {
            if (budgetId <= 0)
                throw new ArgumentException("预算ID无效");

            try
            {
                // 需要在 BudgetRepository 中实现 DeleteBudget 方法
                return _budgetRepository.DeleteBudget(budgetId);
            }
            catch (Exception ex)
            {
                throw new Exception($"删除预算失败: {ex.Message}");
            }
        }

    }
}