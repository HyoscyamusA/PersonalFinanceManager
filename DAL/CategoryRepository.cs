using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using PersonalFinanceManager.Models;

namespace PersonalFinanceManager.DAL
{
    public class CategoryRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public CategoryRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        public List<Category> GetAllCategories()
        {
            var categories = new List<Category>();
            string sql = @"
                SELECT CategoryID, CategoryName, CategoryType, ParentCategoryID, 
                       Icon, Color, BudgetAmount, ActualAmount, SortOrder, IsSystemPreset, Description
                FROM Categories 
                ORDER BY CategoryType, SortOrder";

            var dataTable = _dbHelper.ExecuteQuery(sql);
            foreach (DataRow row in dataTable.Rows)
            {
                categories.Add(new Category
                {
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    CategoryName = row["CategoryName"].ToString(),
                    CategoryType = row["CategoryType"].ToString(),
                    ParentCategoryID = row["ParentCategoryID"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["ParentCategoryID"]),
                    Icon = row["Icon"] == DBNull.Value ? null : row["Icon"].ToString(),
                    Color = row["Color"] == DBNull.Value ? null : row["Color"].ToString(),
                    BudgetAmount = Convert.ToDecimal(row["BudgetAmount"]),
                    ActualAmount = Convert.ToDecimal(row["ActualAmount"]),
                    SortOrder = Convert.ToInt32(row["SortOrder"]),
                    IsSystemPreset = Convert.ToBoolean(row["IsSystemPreset"]),
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString()
                });
            }
            return categories;
        }

        public List<Category> GetCategoriesByType(string categoryType)
        {
            var categories = new List<Category>();
            string sql = "SELECT * FROM Categories WHERE CategoryType = @CategoryType ORDER BY SortOrder";
            var parameter = new MySqlParameter("@CategoryType", categoryType);

            var dataTable = _dbHelper.ExecuteQuery(sql, new[] { parameter });
            foreach (DataRow row in dataTable.Rows)
            {
                categories.Add(new Category
                {
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    CategoryName = row["CategoryName"].ToString(),
                    CategoryType = row["CategoryType"].ToString(),
                    ParentCategoryID = row["ParentCategoryID"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["ParentCategoryID"]),
                    Icon = row["Icon"] == DBNull.Value ? null : row["Icon"].ToString(),
                    Color = row["Color"] == DBNull.Value ? null : row["Color"].ToString(),
                    BudgetAmount = Convert.ToDecimal(row["BudgetAmount"]),
                    ActualAmount = Convert.ToDecimal(row["ActualAmount"]),
                    SortOrder = Convert.ToInt32(row["SortOrder"]),
                    IsSystemPreset = Convert.ToBoolean(row["IsSystemPreset"]),
                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString()
                });
            }
            return categories;
        }

        public Category GetCategoryById(int categoryId)
        {
            string sql = "SELECT * FROM Categories WHERE CategoryID = @CategoryID";
            var parameter = new MySqlParameter("@CategoryID", categoryId);

            var dataTable = _dbHelper.ExecuteQuery(sql, new[] { parameter });
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];
            return new Category
            {
                CategoryID = Convert.ToInt32(row["CategoryID"]),
                CategoryName = row["CategoryName"].ToString(),
                CategoryType = row["CategoryType"].ToString(),
                ParentCategoryID = row["ParentCategoryID"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["ParentCategoryID"]),
                Icon = row["Icon"] == DBNull.Value ? null : row["Icon"].ToString(),
                Color = row["Color"] == DBNull.Value ? null : row["Color"].ToString(),
                BudgetAmount = Convert.ToDecimal(row["BudgetAmount"]),
                ActualAmount = Convert.ToDecimal(row["ActualAmount"]),
                SortOrder = Convert.ToInt32(row["SortOrder"]),
                IsSystemPreset = Convert.ToBoolean(row["IsSystemPreset"]),
                Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString()
            };
        }
    }
}