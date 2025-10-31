using System;
using System.Collections.Generic;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.DAL;

namespace PersonalFinanceManager.BLL
{
    public class TransactionService
    {
        private readonly TransactionRepository _transactionRepository;
        private readonly AccountService _accountService;
        private readonly BudgetRepository _budgetRepository;

        public TransactionService()
        {
            _transactionRepository = new TransactionRepository();
            _accountService = new AccountService();
            _budgetRepository = new BudgetRepository();
        }

        public bool AddTransaction(Transaction transaction)
        {
            if (transaction.Amount <= 0)
                throw new ArgumentException("交易金额必须大于0");

            if (transaction.AccountID <= 0)
                throw new ArgumentException("账户ID无效");

            if (transaction.CategoryID <= 0)
                throw new ArgumentException("类别ID无效");

            try
            {
                bool result = _transactionRepository.AddTransaction(transaction);
                if (result)
                {
                    // 更新账户余额
                    UpdateAccountBalance(transaction);

                    // 新增：更新预算实际支出
                    UpdateBudgetActualAmount(transaction.TransactionTime.Year, transaction.TransactionTime.Month);
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"添加交易记录失败: {ex.Message}");
            }
        }

        // 添加 UpdateTransaction 方法
        public bool UpdateTransaction(Transaction transaction)
        {
            if (transaction.Amount <= 0)
                throw new ArgumentException("交易金额必须大于0");

            if (transaction.AccountID <= 0)
                throw new ArgumentException("账户ID无效");

            if (transaction.CategoryID <= 0)
                throw new ArgumentException("类别ID无效");

            try
            {
                // 先获取原始交易记录
                var originalTransaction = GetTransactionById(transaction.TransactionID);
                if (originalTransaction == null)
                    throw new Exception("找不到要更新的交易记录");

                // 恢复原始交易对账户余额的影响
                ReverseAccountBalance(originalTransaction);

                // 更新交易记录
                bool result = _transactionRepository.UpdateTransaction(transaction);
                if (result)
                {
                    // 应用新交易对账户余额的影响
                    UpdateAccountBalance(transaction);

                    // 新增：更新预算实际支出（两个月份都需要更新）
                    UpdateBudgetActualAmount(originalTransaction.TransactionTime.Year, originalTransaction.TransactionTime.Month);
                    if (originalTransaction.TransactionTime.Year != transaction.TransactionTime.Year ||
                        originalTransaction.TransactionTime.Month != transaction.TransactionTime.Month)
                    {
                        // 如果月份发生变化，更新新月份的预算
                        UpdateBudgetActualAmount(transaction.TransactionTime.Year, transaction.TransactionTime.Month);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception($"更新交易记录失败: {ex.Message}");
            }
        }

        private void UpdateAccountBalance(Transaction transaction)
        {
            var account = _accountService.GetAccountById(transaction.AccountID);
            if (account != null)
            {
                if (transaction.TransactionType == "收入")
                {
                    account.CurrentBalance += transaction.Amount;
                }
                else if (transaction.TransactionType == "支出")
                {
                    account.CurrentBalance -= transaction.Amount;
                }
                // 转账逻辑稍后实现

                _accountService.UpdateAccount(account);
            }
        }

        public List<Transaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            try
            {
                return _transactionRepository.GetTransactionsByDateRange(startDate, endDate);
            }
            catch (Exception ex)
            {
                throw new Exception($"获取交易记录失败: {ex.Message}");
            }
        }

        public List<Transaction> GetRecentTransactions(int count = 50)
        {
            try
            {
                return _transactionRepository.GetRecentTransactions(count);
            }
            catch (Exception ex)
            {
                throw new Exception($"获取最近交易记录失败: {ex.Message}");
            }
        }

        public bool DeleteTransaction(int transactionId)
        {
            if (transactionId <= 0)
                throw new ArgumentException("交易记录ID无效");

            try
            {
                // 先获取交易记录以更新账户余额
                var transaction = GetTransactionById(transactionId);
                if (transaction != null)
                {
                    // 反向更新账户余额
                    ReverseAccountBalance(transaction);

                    bool result = _transactionRepository.DeleteTransaction(transactionId);
                    if (result)
                    {
                        // 新增：更新预算实际支出
                        UpdateBudgetActualAmount(transaction.TransactionTime.Year, transaction.TransactionTime.Month);
                    }
                    return result;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"删除交易记录失败: {ex.Message}");
            }
        }

        private void ReverseAccountBalance(Transaction transaction)
        {
            var account = _accountService.GetAccountById(transaction.AccountID);
            if (account != null)
            {
                if (transaction.TransactionType == "收入")
                {
                    account.CurrentBalance -= transaction.Amount;
                }
                else if (transaction.TransactionType == "支出")
                {
                    account.CurrentBalance += transaction.Amount;
                }

                _accountService.UpdateAccount(account);
            }
        }

        public Transaction GetTransactionById(int transactionId)
        {
            try
            {
                return _transactionRepository.GetTransactionById(transactionId);
            }
            catch (Exception ex)
            {
                throw new Exception($"获取交易记录失败: {ex.Message}");
            }
        }

        public decimal GetTotalIncome(DateTime startDate, DateTime endDate)
        {
            var transactions = GetTransactionsByDateRange(startDate, endDate);
            decimal total = 0;
            foreach (var transaction in transactions)
            {
                if (transaction.TransactionType == "收入")
                {
                    total += transaction.Amount;
                }
            }
            return total;
        }

        public decimal GetTotalExpense(DateTime startDate, DateTime endDate)
        {
            var transactions = GetTransactionsByDateRange(startDate, endDate);
            decimal total = 0;
            foreach (var transaction in transactions)
            {
                if (transaction.TransactionType == "支出")
                {
                    total += transaction.Amount;
                }
            }
            return total;
        }

        // 添加获取所有交易的方法（如果需要）
        public List<Transaction> GetAllTransactions()
        {
            try
            {
                // 这里可以设置一个较大的日期范围来获取所有交易
                return GetTransactionsByDateRange(DateTime.MinValue, DateTime.MaxValue);
            }
            catch (Exception ex)
            {
                throw new Exception($"获取所有交易记录失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 新增：更新指定月份预算的实际支出金额
        /// </summary>
        private void UpdateBudgetActualAmount(int year, int month)
        {
            try
            {
                // 调用 BudgetRepository 的方法更新实际支出
                _budgetRepository.UpdateActualAmounts(year, month);
            }
            catch (Exception ex)
            {
                // 记录日志，但不抛出异常，避免影响主要交易操作
                System.Diagnostics.Debug.WriteLine($"更新预算实际支出失败: {ex.Message}");
            }
        }
    }
}