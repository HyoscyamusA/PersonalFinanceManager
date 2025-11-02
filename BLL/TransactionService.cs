using MySql.Data.MySqlClient;
using PersonalFinanceManager.DAL;
using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;

namespace PersonalFinanceManager.BLL
{
    public class TransactionService
    {
        private readonly TransactionRepository _transactionRepository;
        private readonly AccountService _accountService;
        private readonly BudgetRepository _budgetRepository;
        private readonly DatabaseHelper _databaseHelper;

        public TransactionService()
        {
            _transactionRepository = new TransactionRepository();
            _accountService = new AccountService();
            _budgetRepository = new BudgetRepository();
            _databaseHelper = new DatabaseHelper(); //  初始化
        }

        private void RecalculateAccountBalance(int accountId)
        {
            var account = _accountService.GetAccountById(accountId);
            if (account == null) return;

            //// 如果账户的初始金额和当前余额已经设置，跳过不重新计算
            //if (account.InitialAmount == account.CurrentBalance)
            //{
            //    Console.WriteLine($"[Recalculate] 账户余额无需更新，账户ID {accountId} 的初始金额与当前余额一致。");
            //    return;
            //}

            // 获取该账户所有交易记录
            var transactions = _transactionRepository.GetTransactionsByAccountId(accountId);

            decimal totalIncome = 0;
            decimal totalExpense = 0;

            // 计算所有交易的收入和支出
            foreach (var t in transactions)
            {
                if (t.TransactionType == "收入") totalIncome += t.Amount;
                else if (t.TransactionType == "支出") totalExpense += t.Amount;
                // 转账逻辑可在此扩展
            }

            // 总余额计算：CurrentBalance = InitialAmount + (收入 - 支出)
            //account.CurrentBalance = account.CurrentBalance + totalIncome - totalExpense;
            account.CurrentBalance =  totalIncome - totalExpense;
            // 更新账户余额
            _accountService.UpdateAccount(account);

            Console.WriteLine($"[Recalculate] 账户ID {accountId} 余额重新计算: 初始金额 {account.InitialAmount}, 收入 {totalIncome}, 支出 {totalExpense}, 新余额 {account.CurrentBalance}");
        }


        public bool AddTransaction(Transaction transaction)
        {
            if (transaction.Amount <= 0)
                throw new ArgumentException("交易金额必须大于0");
            if (transaction.AccountID <= 0)
                throw new ArgumentException("账户ID无效");
            if (transaction.CategoryID <= 0)
                throw new ArgumentException("类别ID无效");

            bool result = _transactionRepository.AddTransaction(transaction);
            if (result)
            {
                // 统一重新计算余额
                RecalculateAccountBalance(transaction.AccountID);

                // 更新预算实际支出
                UpdateBudgetActualAmount(transaction.TransactionTime.Year, transaction.TransactionTime.Month);
            }
            return result;
        }

        public bool UpdateTransaction(Transaction transaction)
        {
            if (transaction.Amount <= 0 || transaction.AccountID <= 0 || transaction.CategoryID <= 0)
                throw new ArgumentException("交易数据无效");

            var originalTransaction = GetTransactionById(transaction.TransactionID);
            if (originalTransaction == null)
                throw new Exception("找不到原始交易记录");

            bool result = _transactionRepository.UpdateTransaction(transaction);
            if (result)
            {
                // 先反转原始交易对余额的影响
                ReverseAccountBalance(originalTransaction);

                // 更新交易后，重新计算账户余额
                RecalculateAccountBalance(transaction.AccountID);

                // 更新预算实际支出（原交易月 + 新交易月）
                UpdateBudgetActualAmount(originalTransaction.TransactionTime.Year, originalTransaction.TransactionTime.Month);
                if (originalTransaction.TransactionTime.Month != transaction.TransactionTime.Month ||
                    originalTransaction.TransactionTime.Year != transaction.TransactionTime.Year)
                {
                    UpdateBudgetActualAmount(transaction.TransactionTime.Year, transaction.TransactionTime.Month);
                }
            }
            return result;
        }


        public bool DeleteTransaction(int transactionId)
        {
            var transaction = GetTransactionById(transactionId);
            if (transaction == null) return false;

            bool result = _transactionRepository.DeleteTransaction(transactionId);
            if (result)
            {
                // 反转交易对账户余额的影响
                ReverseAccountBalance(transaction);

                // 重新计算账户余额
                RecalculateAccountBalance(transaction.AccountID);

                // 更新预算实际支出
                UpdateBudgetActualAmount(transaction.TransactionTime.Year, transaction.TransactionTime.Month);
            }
            return result;
        }

        private void ReverseAccountBalance(Transaction transaction)
        {
            var account = _accountService.GetAccountById(transaction.AccountID);
            if (account != null)
            {
                if (transaction.TransactionType == "收入")
                {
                    account.CurrentBalance -= transaction.Amount;  // 减去收入
                }
                else if (transaction.TransactionType == "支出")
                {
                    account.CurrentBalance += transaction.Amount;  // 加回支出
                }

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

        public bool DeleteTransactionsByAccountId(int accountId)
        {
            string sql = "DELETE FROM transactions WHERE AccountID = @AccountID";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@AccountID", accountId)
            };

            _databaseHelper.ExecuteNonQuery(sql, parameters);
            return true;
        }



    }
}