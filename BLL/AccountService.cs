using MySql.Data.MySqlClient;
using PersonalFinanceManager.DAL;
using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;

namespace PersonalFinanceManager.BLL
{
    public class AccountService
    {
        private readonly AccountRepository _accountRepository;
        private readonly DatabaseHelper _databaseHelper;

        public AccountService()
        {
            _accountRepository = new AccountRepository();
            _databaseHelper = new DatabaseHelper(); // 初始化
        }

        public List<Account> GetAllAccounts()
        {
            try
            {
                return _accountRepository.GetAllAccounts();
            }
            catch (Exception ex)
            {
                throw new Exception($"获取账户列表失败: {ex.Message}");
            }
        }

        public bool AddAccount(Account account)
        {
            if (string.IsNullOrWhiteSpace(account.AccountName))
                throw new ArgumentException("账户名称不能为空");

            if (account.InitialAmount < 0)
                throw new ArgumentException("初始金额不能为负数");

            try
            {
                _accountRepository.AddAccount(account);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"添加账户失败: {ex.Message}");
            }
        }

        public bool UpdateAccount(Account account)
        {
            if (string.IsNullOrWhiteSpace(account.AccountName))
                throw new ArgumentException("账户名称不能为空");

            if (account.AccountID <= 0)
                throw new ArgumentException("账户ID无效");

            try
            {
                return _accountRepository.UpdateAccount(account);
            }
            catch (Exception ex)
            {
                throw new Exception($"更新账户失败: {ex.Message}");
            }
        }

        public bool DeleteAccount(int accountId)
        {
            if (accountId <= 0)
                throw new ArgumentException("账户ID无效");

            try
            {
                return _accountRepository.DeleteAccount(accountId);
            }
            catch (Exception ex)
            {
                throw new Exception($"删除账户失败: {ex.Message}");
            }
        }

        public decimal GetTotalBalance()
        {
            try
            {
                return _accountRepository.GetTotalBalance();
            }
            catch (Exception ex)
            {
                throw new Exception($"获取总余额失败: {ex.Message}");
            }
        }

        public Account GetAccountById(int accountId)
        {
            if (accountId <= 0)
                throw new ArgumentException("账户ID无效");

            try
            {
                return _accountRepository.GetAccountById(accountId);
            }
            catch (Exception ex)
            {
                throw new Exception($"获取账户信息失败: {ex.Message}");
            }
        }
        public Account GetLatestAccount()
        {
            try
            {
                string sql = "SELECT * FROM accounts ORDER BY CreateTime DESC LIMIT 1";
                var table = _databaseHelper.ExecuteQuery(sql);
                if (table.Rows.Count == 0)
                    return null;

                var row = table.Rows[0];
                return new Account
                {
                    AccountID = Convert.ToInt32(row["AccountID"]),
                    AccountName = row["AccountName"].ToString(),
                    AccountType = row["AccountType"].ToString(),
                    InitialAmount = Convert.ToDecimal(row["InitialAmount"]),
                    CurrentBalance = Convert.ToDecimal(row["CurrentBalance"]),
                    Currency = row["Currency"].ToString(),
                    BankName = row["BankName"].ToString(),
                    CardNumber = row["CardNumber"].ToString(),
                    Description = row["Description"].ToString(),
                    CreateTime = Convert.ToDateTime(row["CreateTime"])
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"获取最新账户失败: {ex.Message}");
            }
        }



    }
}