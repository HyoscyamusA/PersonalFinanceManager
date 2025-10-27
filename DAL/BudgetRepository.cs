//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PersonalFinanceManager.DAL
//{
//    internal class BudgetRepository
//    {
//    }
//}
using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using PersonalFinanceManager.Models;

namespace PersonalFinanceManager.DAL
{
    public class BudgetRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public BudgetRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        public bool SetBudget(Budget budget)
        {
            // 先检查是否已存在该月份该类别的预算
            string checkSql = @"
                SELECT COUNT(*) FROM Budgets 
                WHERE BudgetYear = @BudgetYear AND BudgetMonth = @BudgetMonth AND CategoryID = @CategoryID";

            var checkParameters = new MySqlParameter[]
            {
                new MySqlParameter("@BudgetYear", budget.BudgetYear),
                new MySqlParameter("@BudgetMonth", budget.BudgetMonth),
                new MySqlParameter("@CategoryID", budget.CategoryID)
            };

            var exists = Convert.ToInt32(_dbHelper.ExecuteScalar(checkSql, checkParameters)) > 0;

            string sql;
            MySqlParameter[] parameters;

            if (exists)
            {
                // 更新现有预算
                sql = @"
                    UPDATE Budgets 
                    SET BudgetAmount = @BudgetAmount, WarningThreshold = @WarningThreshold,
                        Status = @Status, Remark = @Remark, UpdateTime = CURRENT_TIMESTAMP
                    WHERE BudgetYear = @BudgetYear AND BudgetMonth = @BudgetMonth AND CategoryID = @CategoryID";
            }
            else
            {
                // 插入新预算
                sql = @"
                    INSERT INTO Budgets (BudgetYear, BudgetMonth, CategoryID, BudgetAmount, 
                                       WarningThreshold, Status, CreatedBy, Remark)
                    VALUES (@BudgetYear, @BudgetMonth, @CategoryID, @BudgetAmount, 
                           @WarningThreshold, @Status, @CreatedBy, @Remark)";
            }

            parameters = new MySqlParameter[]
            {
                new MySqlParameter("@BudgetYear", budget.BudgetYear),
                new MySqlParameter("@BudgetMonth", budget.BudgetMonth),
                new MySqlParameter("@CategoryID", budget.CategoryID),
                new MySqlParameter("@BudgetAmount", budget.BudgetAmount),
                new MySqlParameter("@WarningThreshold", budget.WarningThreshold),
                new MySqlParameter("@Status", budget.Status),
                new MySqlParameter("@CreatedBy", budget.CreatedBy ?? (object)DBNull.Value),
                new MySqlParameter("@Remark", budget.Remark ?? (object)DBNull.Value)
            };

            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public List<Budget> GetBudgetsByMonth(int year, int month)
        {
            var budgets = new List<Budget>();
            string sql = @"
                SELECT b.*, c.CategoryName 
                FROM Budgets b
                LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
                WHERE b.BudgetYear = @BudgetYear AND b.BudgetMonth = @BudgetMonth
                ORDER BY c.CategoryType, c.SortOrder";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@BudgetYear", year),
                new MySqlParameter("@BudgetMonth", month)
            };

            var dataTable = _dbHelper.ExecuteQuery(sql, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                var budget = new Budget
                {
                    BudgetID = Convert.ToInt32(row["BudgetID"]),
                    BudgetYear = Convert.ToInt32(row["BudgetYear"]),
                    BudgetMonth = Convert.ToInt32(row["BudgetMonth"]),
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    BudgetAmount = Convert.ToDecimal(row["BudgetAmount"]),
                    ActualAmount = Convert.ToDecimal(row["ActualAmount"]),
                    CompletionRate = Convert.ToDecimal(row["CompletionRate"]),
                    WarningThreshold = Convert.ToDecimal(row["WarningThreshold"]),
                    ReminderMethod = row["ReminderMethod"].ToString(),
                    Status = row["Status"].ToString(),
                    CreatedBy = row["CreatedBy"] == DBNull.Value ? null : row["CreatedBy"].ToString(),
                    CreateTime = Convert.ToDateTime(row["CreateTime"]),
                    Remark = row["Remark"] == DBNull.Value ? null : row["Remark"].ToString(),
                    CategoryName = row["CategoryName"].ToString()
                };

                // 重新计算完成率
                budget.CalculateCompletionRate();
                budgets.Add(budget);
            }
            return budgets;
        }

        public List<Budget> GetBudgetWarnings()
        {
            var warnings = new List<Budget>();
            string sql = @"
                SELECT b.*, c.CategoryName 
                FROM Budgets b
                LEFT JOIN Categories c ON b.CategoryID = c.CategoryID
                WHERE b.Status = '激活' AND b.CompletionRate >= b.WarningThreshold
                ORDER BY b.CompletionRate DESC";

            var dataTable = _dbHelper.ExecuteQuery(sql);
            foreach (DataRow row in dataTable.Rows)
            {
                var budget = new Budget
                {
                    BudgetID = Convert.ToInt32(row["BudgetID"]),
                    BudgetYear = Convert.ToInt32(row["BudgetYear"]),
                    BudgetMonth = Convert.ToInt32(row["BudgetMonth"]),
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    BudgetAmount = Convert.ToDecimal(row["BudgetAmount"]),
                    ActualAmount = Convert.ToDecimal(row["ActualAmount"]),
                    CompletionRate = Convert.ToDecimal(row["CompletionRate"]),
                    WarningThreshold = Convert.ToDecimal(row["WarningThreshold"]),
                    ReminderMethod = row["ReminderMethod"].ToString(),
                    Status = row["Status"].ToString(),
                    CreatedBy = row["CreatedBy"] == DBNull.Value ? null : row["CreatedBy"].ToString(),
                    CreateTime = Convert.ToDateTime(row["CreateTime"]),
                    Remark = row["Remark"] == DBNull.Value ? null : row["Remark"].ToString(),
                    CategoryName = row["CategoryName"].ToString()
                };
                warnings.Add(budget);
            }
            return warnings;
        }
    }
}