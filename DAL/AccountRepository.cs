
//using System;
//using System.Data;
//using System.Collections.Generic;
//using MySql.Data.MySqlClient;
//using PersonalFinanceManager.Models;

//namespace PersonalFinanceManager.DAL
//{
//    public class AccountRepository
//    {
//        private readonly DatabaseHelper _dbHelper;

//        public AccountRepository()
//        {
//            _dbHelper = new DatabaseHelper();
//        }

//        public List<Account> GetAllAccounts()
//        {
//            var accounts = new List<Account>();
//            string sql = @"
//                SELECT AccountID, AccountName, AccountType, InitialAmount, CurrentBalance, 
//                       Currency, BankName, CardNumber, Description, ColorMarker, IsActive, CreateTime
//                FROM Accounts 
//                WHERE IsActive = TRUE 
//                ORDER BY CreateTime DESC";

//            var dataTable = _dbHelper.ExecuteQuery(sql);
//            foreach (DataRow row in dataTable.Rows)
//            {
//                accounts.Add(new Account
//                {
//                    AccountID = Convert.ToInt32(row["AccountID"]),
//                    AccountName = row["AccountName"].ToString(),
//                    AccountType = row["AccountType"].ToString(),
//                    InitialAmount = Convert.ToDecimal(row["InitialAmount"]),
//                    CurrentBalance = Convert.ToDecimal(row["CurrentBalance"]),
//                    Currency = row["Currency"].ToString(),
//                    BankName = row["BankName"] == DBNull.Value ? null : row["BankName"].ToString(),
//                    CardNumber = row["CardNumber"] == DBNull.Value ? null : row["CardNumber"].ToString(),
//                    Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(),
//                    ColorMarker = row["ColorMarker"] == DBNull.Value ? null : row["ColorMarker"].ToString(),
//                    IsActive = Convert.ToBoolean(row["IsActive"]),
//                    CreateTime = Convert.ToDateTime(row["CreateTime"])
//                });
//            }
//            return accounts;
//        }

//        public int AddAccount(Account account)
//        {
//            string sql = @"
//                INSERT INTO Accounts (AccountName, AccountType, InitialAmount, CurrentBalance, 
//                                    Currency, BankName, CardNumber, Description, ColorMarker, IsActive)
//                VALUES (@AccountName, @AccountType, @InitialAmount, @CurrentBalance, 
//                       @Currency, @BankName, @CardNumber, @Description, @ColorMarker, @IsActive);
//                SELECT LAST_INSERT_ID();";

//            var parameters = new MySqlParameter[]
//            {
//                new MySqlParameter("@AccountName", account.AccountName),
//                new MySqlParameter("@AccountType", account.AccountType),
//                new MySqlParameter("@InitialAmount", account.InitialAmount),
//                new MySqlParameter("@CurrentBalance", account.InitialAmount),
//                new MySqlParameter("@Currency", account.Currency),
//                new MySqlParameter("@BankName", account.BankName ?? (object)DBNull.Value),
//                new MySqlParameter("@CardNumber", account.CardNumber ?? (object)DBNull.Value),
//                new MySqlParameter("@Description", account.Description ?? (object)DBNull.Value),
//                new MySqlParameter("@ColorMarker", account.ColorMarker ?? (object)DBNull.Value),
//                new MySqlParameter("@IsActive", account.IsActive)
//            };

//            var result = _dbHelper.ExecuteScalar(sql, parameters);
//            return Convert.ToInt32(result);
//        }

//        public bool UpdateAccount(Account account)
//        {
//            string sql = @"
//                UPDATE Accounts 
//                SET AccountName = @AccountName, AccountType = @AccountType, 
//                    InitialAmount = @InitialAmount, CurrentBalance = @CurrentBalance,
//                    Currency = @Currency, BankName = @BankName, CardNumber = @CardNumber,
//                    Description = @Description, ColorMarker = @ColorMarker, IsActive = @IsActive
//                WHERE AccountID = @AccountID";

//            var parameters = new MySqlParameter[]
//            {
//                new MySqlParameter("@AccountName", account.AccountName),
//                new MySqlParameter("@AccountType", account.AccountType),
//                new MySqlParameter("@InitialAmount", account.InitialAmount),
//                new MySqlParameter("@CurrentBalance", account.CurrentBalance),
//                new MySqlParameter("@Currency", account.Currency),
//                new MySqlParameter("@BankName", account.BankName ?? (object)DBNull.Value),
//                new MySqlParameter("@CardNumber", account.CardNumber ?? (object)DBNull.Value),
//                new MySqlParameter("@Description", account.Description ?? (object)DBNull.Value),
//                new MySqlParameter("@ColorMarker", account.ColorMarker ?? (object)DBNull.Value),
//                new MySqlParameter("@IsActive", account.IsActive),
//                new MySqlParameter("@AccountID", account.AccountID)
//            };

//            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
//        }

//        public bool DeleteAccount(int accountId)
//        {
//            string sql = "UPDATE Accounts SET IsActive = FALSE WHERE AccountID = @AccountID";
//            var parameter = new MySqlParameter("@AccountID", accountId);
//            return _dbHelper.ExecuteNonQuery(sql, new[] { parameter }) > 0;
//        }

//        public decimal GetTotalBalance()
//        {
//            string sql = "SELECT SUM(CurrentBalance) FROM Accounts WHERE IsActive = TRUE";
//            var result = _dbHelper.ExecuteScalar(sql);
//            return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
//        }

//        public Account GetAccountById(int accountId)
//        {
//            string sql = "SELECT * FROM Accounts WHERE AccountID = @AccountID";
//            var parameter = new MySqlParameter("@AccountID", accountId);

//            var dataTable = _dbHelper.ExecuteQuery(sql, new[] { parameter });
//            if (dataTable.Rows.Count == 0)
//                return null;

//            var row = dataTable.Rows[0];
//            return new Account
//            {
//                AccountID = Convert.ToInt32(row["AccountID"]),
//                AccountName = row["AccountName"].ToString(),
//                AccountType = row["AccountType"].ToString(),
//                InitialAmount = Convert.ToDecimal(row["InitialAmount"]),
//                CurrentBalance = Convert.ToDecimal(row["CurrentBalance"]),
//                Currency = row["Currency"].ToString(),
//                BankName = row["BankName"] == DBNull.Value ? null : row["BankName"].ToString(),
//                CardNumber = row["CardNumber"] == DBNull.Value ? null : row["CardNumber"].ToString(),
//                Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(),
//                ColorMarker = row["ColorMarker"] == DBNull.Value ? null : row["ColorMarker"].ToString(),
//                IsActive = Convert.ToBoolean(row["IsActive"]),
//                CreateTime = Convert.ToDateTime(row["CreateTime"])
//            };
//        }
//    }
//}
using MySql.Data.MySqlClient;
using PersonalFinanceManager.DAL;
using PersonalFinanceManager.Models;
using System;
using System.Collections.Generic;
using System.Data;

public class AccountRepository
{
    private readonly DatabaseHelper _dbHelper;

    public AccountRepository()
    {
        _dbHelper = new DatabaseHelper();
    }

    public List<Account> GetAllAccounts()
    {
        var accounts = new List<Account>();
        string sql = @"SELECT AccountID, AccountName, AccountType, InitialAmount, CurrentBalance, 
                              Currency, BankName, CardNumber, Description, ColorMarker, CreateTime
                       FROM Accounts 
                       ORDER BY CreateTime DESC";

        var dataTable = _dbHelper.ExecuteQuery(sql);
        foreach (DataRow row in dataTable.Rows)
        {
            accounts.Add(new Account
            {
                AccountID = Convert.ToInt32(row["AccountID"]),
                AccountName = row["AccountName"].ToString(),
                AccountType = row["AccountType"].ToString(),
                InitialAmount = Convert.ToDecimal(row["InitialAmount"]),
                CurrentBalance = Convert.ToDecimal(row["CurrentBalance"]),
                Currency = row["Currency"].ToString(),
                BankName = row["BankName"] == DBNull.Value ? null : row["BankName"].ToString(),
                CardNumber = row["CardNumber"] == DBNull.Value ? null : row["CardNumber"].ToString(),
                Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(),
                ColorMarker = row["ColorMarker"] == DBNull.Value ? null : row["ColorMarker"].ToString(),
                CreateTime = Convert.ToDateTime(row["CreateTime"])
            });
        }
        return accounts;
    }

    public int AddAccount(Account account)
    {
        string sql = @"INSERT INTO Accounts (AccountName, AccountType, InitialAmount, CurrentBalance, 
                                            Currency, BankName, CardNumber, Description, ColorMarker)
                       VALUES (@AccountName, @AccountType, @InitialAmount, @CurrentBalance, 
                               @Currency, @BankName, @CardNumber, @Description, @ColorMarker);
                       SELECT LAST_INSERT_ID();";

        var parameters = new MySqlParameter[]
        {
            new MySqlParameter("@AccountName", account.AccountName),
            new MySqlParameter("@AccountType", account.AccountType),
            new MySqlParameter("@InitialAmount", account.InitialAmount),
            new MySqlParameter("@CurrentBalance", account.InitialAmount), // 初始余额=初始金额
            new MySqlParameter("@Currency", account.Currency),
            new MySqlParameter("@BankName", account.BankName ?? (object)DBNull.Value),
            new MySqlParameter("@CardNumber", account.CardNumber ?? (object)DBNull.Value),
            new MySqlParameter("@Description", account.Description ?? (object)DBNull.Value),
            new MySqlParameter("@ColorMarker", account.ColorMarker ?? (object)DBNull.Value)
        };

        var result = _dbHelper.ExecuteScalar(sql, parameters);
        return Convert.ToInt32(result);
    }

    public bool UpdateAccount(Account account)
    {
        string sql = @"UPDATE Accounts 
                       SET AccountName = @AccountName, AccountType = @AccountType, 
                           InitialAmount = @InitialAmount, CurrentBalance = @CurrentBalance,
                           Currency = @Currency, BankName = @BankName, CardNumber = @CardNumber,
                           Description = @Description, ColorMarker = @ColorMarker
                       WHERE AccountID = @AccountID";

        var parameters = new MySqlParameter[]
        {
            new MySqlParameter("@AccountName", account.AccountName),
            new MySqlParameter("@AccountType", account.AccountType),
            new MySqlParameter("@InitialAmount", account.InitialAmount),
            new MySqlParameter("@CurrentBalance", account.CurrentBalance),
            new MySqlParameter("@Currency", account.Currency),
            new MySqlParameter("@BankName", account.BankName ?? (object)DBNull.Value),
            new MySqlParameter("@CardNumber", account.CardNumber ?? (object)DBNull.Value),
            new MySqlParameter("@Description", account.Description ?? (object)DBNull.Value),
            new MySqlParameter("@ColorMarker", account.ColorMarker ?? (object)DBNull.Value),
            new MySqlParameter("@AccountID", account.AccountID)
        };

        return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
    }

    public bool DeleteAccount(int accountId)
    {
        string sql = "DELETE FROM Accounts WHERE AccountID = @AccountID";
        var parameter = new MySqlParameter("@AccountID", accountId);
        return _dbHelper.ExecuteNonQuery(sql, new[] { parameter }) > 0;
    }

    public decimal GetTotalBalance()
    {
        string sql = "SELECT SUM(CurrentBalance) FROM Accounts";
        var result = _dbHelper.ExecuteScalar(sql);
        return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
    }

    public Account GetAccountById(int accountId)
    {
        string sql = "SELECT * FROM Accounts WHERE AccountID = @AccountID";
        var parameter = new MySqlParameter("@AccountID", accountId);

        var dataTable = _dbHelper.ExecuteQuery(sql, new[] { parameter });
        if (dataTable.Rows.Count == 0)
            return null;

        var row = dataTable.Rows[0];
        return new Account
        {
            AccountID = Convert.ToInt32(row["AccountID"]),
            AccountName = row["AccountName"].ToString(),
            AccountType = row["AccountType"].ToString(),
            InitialAmount = Convert.ToDecimal(row["InitialAmount"]),
            CurrentBalance = Convert.ToDecimal(row["CurrentBalance"]),
            Currency = row["Currency"].ToString(),
            BankName = row["BankName"] == DBNull.Value ? null : row["BankName"].ToString(),
            CardNumber = row["CardNumber"] == DBNull.Value ? null : row["CardNumber"].ToString(),
            Description = row["Description"] == DBNull.Value ? null : row["Description"].ToString(),
            ColorMarker = row["ColorMarker"] == DBNull.Value ? null : row["ColorMarker"].ToString(),
            CreateTime = Convert.ToDateTime(row["CreateTime"])
        };
    }
}