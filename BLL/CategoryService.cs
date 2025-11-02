using System;
using System.Collections.Generic;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.DAL;

namespace PersonalFinanceManager.BLL
{
    public class CategoryService
    {
        private readonly CategoryRepository _categoryRepository;

        public CategoryService()
        {
            _categoryRepository = new CategoryRepository();
        }

        public List<Category> GetAllCategories()
        {
            try
            {
                return _categoryRepository.GetAllCategories();
            }
            catch (Exception ex)
            {
                throw new Exception($"获取类别列表失败: {ex.Message}");
            }
        }

        public List<Category> GetIncomeCategories()
        {
            try
            {
                return _categoryRepository.GetCategoriesByType("收入");
            }
            catch (Exception ex)
            {
                throw new Exception($"获取收入类别失败: {ex.Message}");
            }
        }

        public List<Category> GetExpenseCategories()
        {
            try
            {
                return _categoryRepository.GetCategoriesByType("支出");
            }
            catch (Exception ex)
            {
                throw new Exception($"获取支出类别失败: {ex.Message}");
            }
        }

        public Category GetCategoryById(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("类别ID无效");

            try
            {
                return _categoryRepository.GetCategoryById(categoryId);
            }
            catch (Exception ex)
            {
                throw new Exception($"获取类别信息失败: {ex.Message}");
            }
        }
    }
}