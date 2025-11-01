
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

    //public int AddAccount(Account account)
    //{
    //    string sql = @"INSERT INTO Accounts (AccountName, AccountType, InitialAmount, CurrentBalance, 
    //                                        Currency, BankName, CardNumber, Description, ColorMarker)
    //                   VALUES (@AccountName, @AccountType, @InitialAmount, @CurrentBalance, 
    //                           @Currency, @BankName, @CardNumber, @Description, @ColorMarker);
    //                   SELECT LAST_INSERT_ID();";

    //    var parameters = new MySqlParameter[]
    //    {
    //        new MySqlParameter("@AccountName", account.AccountName),
    //        new MySqlParameter("@AccountType", account.AccountType),
    //        new MySqlParameter("@InitialAmount", account.InitialAmount),
    //        new MySqlParameter("@CurrentBalance", account.InitialAmount), // 初始余额=初始金额
    //        new MySqlParameter("@Currency", account.Currency),
    //        new MySqlParameter("@BankName", account.BankName ?? (object)DBNull.Value),
    //        new MySqlParameter("@CardNumber", account.CardNumber ?? (object)DBNull.Value),
    //        new MySqlParameter("@Description", account.Description ?? (object)DBNull.Value),
    //        new MySqlParameter("@ColorMarker", account.ColorMarker ?? (object)DBNull.Value)
    //    };

    //    var result = _dbHelper.ExecuteScalar(sql, parameters);
    //    return Convert.ToInt32(result);
    //}
    //public int AddAccount(Account account)
    //{
    //    // 确保初始金额有效
    //    if (account.InitialAmount < 0)
    //    {
    //        throw new ArgumentException("初始金额不能为负数");
    //    }

    //    // 设置账户余额为初始金额
    //    account.CurrentBalance = account.InitialAmount;

    //    string sql = @"INSERT INTO Accounts (AccountName, AccountType, InitialAmount, CurrentBalance, 
    //                                        Currency, BankName, CardNumber, Description, ColorMarker)
    //               VALUES (@AccountName, @AccountType, @InitialAmount, @CurrentBalance, 
    //                       @Currency, @BankName, @CardNumber, @Description, @ColorMarker);
    //               SELECT LAST_INSERT_ID();";

    //    var parameters = new MySqlParameter[]
    //    {
    //    new MySqlParameter("@AccountName", account.AccountName),
    //    new MySqlParameter("@AccountType", account.AccountType),
    //    new MySqlParameter("@InitialAmount", account.InitialAmount),
    //    new MySqlParameter("@CurrentBalance", account.CurrentBalance), // 确保余额初始化为初始金额
    //    new MySqlParameter("@Currency", account.Currency),
    //    new MySqlParameter("@BankName", account.BankName ?? (object)DBNull.Value),
    //    new MySqlParameter("@CardNumber", account.CardNumber ?? (object)DBNull.Value),
    //    new MySqlParameter("@Description", account.Description ?? (object)DBNull.Value),
    //    new MySqlParameter("@ColorMarker", account.ColorMarker ?? (object)DBNull.Value)
    //    };

    //    var result = _dbHelper.ExecuteScalar(sql, parameters);
    //    return Convert.ToInt32(result);
    //}
    //public int AddAccount(Account account)
    //{
    //    // 确保初始金额有效
    //    if (account.InitialAmount < 0)
    //    {
    //        throw new ArgumentException("初始金额不能为负数");
    //    }

    //    // 设置账户余额为初始金额
    //    account.CurrentBalance = account.InitialAmount;

    //    string sql = @"INSERT INTO Accounts (AccountName, AccountType, InitialAmount, CurrentBalance, 
    //                                        Currency, BankName, CardNumber, Description, ColorMarker)
    //               VALUES (@AccountName, @AccountType, @InitialAmount, @CurrentBalance, 
    //                       @Currency, @BankName, @CardNumber, @Description, @ColorMarker);
    //               SELECT LAST_INSERT_ID();";

    //    var parameters = new MySqlParameter[]
    //    {
    //    new MySqlParameter("@AccountName", account.AccountName),
    //    new MySqlParameter("@AccountType", account.AccountType),
    //    new MySqlParameter("@InitialAmount", account.InitialAmount),
    //    new MySqlParameter("@CurrentBalance", account.CurrentBalance), // 确保余额初始化为初始金额
    //    new MySqlParameter("@Currency", account.Currency),
    //    new MySqlParameter("@BankName", account.BankName ?? (object)DBNull.Value),
    //    new MySqlParameter("@CardNumber", account.CardNumber ?? (object)DBNull.Value),
    //    new MySqlParameter("@Description", account.Description ?? (object)DBNull.Value),
    //    new MySqlParameter("@ColorMarker", account.ColorMarker ?? (object)DBNull.Value)
    //    };

    //    var result = _dbHelper.ExecuteScalar(sql, parameters);
    //    return Convert.ToInt32(result);
    //}
    //public int AddAccount(Account account)
    //{
    //    // 确保初始金额有效
    //    if (account.InitialAmount < 0)
    //    {
    //        throw new ArgumentException("初始金额不能为负数");
    //    }

    //    // 设置账户余额为初始金额
    //    account.CurrentBalance = account.InitialAmount;

    //    string sql = @"INSERT INTO Accounts (AccountName, AccountType, InitialAmount, CurrentBalance, 
    //                                    Currency, BankName, CardNumber, Description, ColorMarker)
    //               VALUES (@AccountName, @AccountType, @InitialAmount, @CurrentBalance, 
    //                       @Currency, @BankName, @CardNumber, @Description, @ColorMarker);
    //               SELECT LAST_INSERT_ID();";

    //    var parameters = new MySqlParameter[]
    //    {
    //    new MySqlParameter("@AccountName", account.AccountName),
    //    new MySqlParameter("@AccountType", account.AccountType),
    //    new MySqlParameter("@InitialAmount", account.InitialAmount),
    //    new MySqlParameter("@CurrentBalance", account.CurrentBalance), // 确保余额初始化为初始金额
    //    new MySqlParameter("@Currency", account.Currency),
    //    new MySqlParameter("@BankName", account.BankName ?? (object)DBNull.Value),
    //    new MySqlParameter("@CardNumber", account.CardNumber ?? (object)DBNull.Value),
    //    new MySqlParameter("@Description", account.Description ?? (object)DBNull.Value),
    //    new MySqlParameter("@ColorMarker", account.ColorMarker ?? (object)DBNull.Value)
    //    };

    //    var result = _dbHelper.ExecuteScalar(sql, parameters);
    //    return Convert.ToInt32(result);
    //}
    public int AddAccount(Account account)
    {
        // 确保初始金额有效
        if (account.InitialAmount < 0)
        {
            throw new ArgumentException("初始金额不能为负数");
        }

        // 设置账户余额为初始金额
        account.CurrentBalance = account.InitialAmount;

        // 日志记录
        Console.WriteLine($"[AddAccount] 添加账户：{account.AccountName}，初始金额：{account.InitialAmount}，当前余额：{account.CurrentBalance}");

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
        new MySqlParameter("@CurrentBalance", account.CurrentBalance), // 确保余额初始化为初始金额
        new MySqlParameter("@Currency", account.Currency),
        new MySqlParameter("@BankName", account.BankName ?? (object)DBNull.Value),
        new MySqlParameter("@CardNumber", account.CardNumber ?? (object)DBNull.Value),
        new MySqlParameter("@Description", account.Description ?? (object)DBNull.Value),
        new MySqlParameter("@ColorMarker", account.ColorMarker ?? (object)DBNull.Value)
        };

        var result = _dbHelper.ExecuteScalar(sql, parameters);
        return Convert.ToInt32(result);
    }


    //public bool UpdateAccount(Account account)
    //{
    //    string sql = @"UPDATE Accounts 
    //                   SET AccountName = @AccountName, AccountType = @AccountType, 
    //                       InitialAmount = @InitialAmount, CurrentBalance = @CurrentBalance,
    //                       Currency = @Currency, BankName = @BankName, CardNumber = @CardNumber,
    //                       Description = @Description, ColorMarker = @ColorMarker
    //                   WHERE AccountID = @AccountID";

    //    var parameters = new MySqlParameter[]
    //    {
    //        new MySqlParameter("@AccountName", account.AccountName),
    //        new MySqlParameter("@AccountType", account.AccountType),
    //        new MySqlParameter("@InitialAmount", account.InitialAmount),
    //        new MySqlParameter("@CurrentBalance", account.CurrentBalance),
    //        new MySqlParameter("@Currency", account.Currency),
    //        new MySqlParameter("@BankName", account.BankName ?? (object)DBNull.Value),
    //        new MySqlParameter("@CardNumber", account.CardNumber ?? (object)DBNull.Value),
    //        new MySqlParameter("@Description", account.Description ?? (object)DBNull.Value),
    //        new MySqlParameter("@ColorMarker", account.ColorMarker ?? (object)DBNull.Value),
    //        new MySqlParameter("@AccountID", account.AccountID)
    //    };

    //    return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
    //}
    //public bool UpdateAccount(Account account)
    //{
    //    // 实例化交易仓储类，用于检测是否有支出交易
    //    var transactionRepo = new TransactionRepository();

    //    // 获取数据库中原始账户信息
    //    var existingAccount = GetAccountById(account.AccountID);

    //    // 检查是否存在支出交易
    //    bool hasExpense = transactionRepo.HasExpenseTransaction(account.AccountID);

    //    // 如果当前余额和初始余额相等
    //    if (existingAccount.InitialAmount == existingAccount.CurrentBalance)
    //    {
    //        if (hasExpense)
    //        {
    //            // 若已有支出交易，不允许修改初始余额
    //            throw new InvalidOperationException("该账户已有支出交易，不能修改初始余额！");
    //        }
    //        else
    //        {
    //            // 没支出 → 允许同步修改初始余额和当前余额
    //            existingAccount.InitialAmount = account.InitialAmount;
    //            existingAccount.CurrentBalance = account.InitialAmount;
    //        }
    //    }
    //    else
    //    {
    //        // 若余额不同，说明已有收入/支出导致变动，不动初始余额
    //        existingAccount.AccountName = account.AccountName;
    //        existingAccount.AccountType = account.AccountType;
    //        existingAccount.Currency = account.Currency;
    //        existingAccount.BankName = account.BankName;
    //        existingAccount.CardNumber = account.CardNumber;
    //        existingAccount.Description = account.Description;
    //        existingAccount.ColorMarker = account.ColorMarker;
    //    }

    //    // 最终执行数据库更新
    //    string sql = @"
    //    UPDATE Accounts 
    //    SET AccountName = @AccountName, AccountType = @AccountType, 
    //        InitialAmount = @InitialAmount, CurrentBalance = @CurrentBalance,
    //        Currency = @Currency, BankName = @BankName, CardNumber = @CardNumber,
    //        Description = @Description, ColorMarker = @ColorMarker
    //    WHERE AccountID = @AccountID";

    //    var parameters = new MySqlParameter[]
    //    {
    //    new MySqlParameter("@AccountName", existingAccount.AccountName),
    //    new MySqlParameter("@AccountType", existingAccount.AccountType),
    //    new MySqlParameter("@InitialAmount", existingAccount.InitialAmount),
    //    new MySqlParameter("@CurrentBalance", existingAccount.CurrentBalance),
    //    new MySqlParameter("@Currency", existingAccount.Currency),
    //    new MySqlParameter("@BankName", existingAccount.BankName ?? (object)DBNull.Value),
    //    new MySqlParameter("@CardNumber", existingAccount.CardNumber ?? (object)DBNull.Value),
    //    new MySqlParameter("@Description", existingAccount.Description ?? (object)DBNull.Value),
    //    new MySqlParameter("@ColorMarker", existingAccount.ColorMarker ?? (object)DBNull.Value),
    //    new MySqlParameter("@AccountID", existingAccount.AccountID)
    //    };

    //    return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
    //}
    //public bool UpdateAccount(Account account)
    //{
    //    var existingAccount = GetAccountById(account.AccountID);
    //    // 实例化交易仓储类，用于检测是否有支出交易
    //    var transactionRepo = new TransactionRepository();
    //    if (existingAccount == null)
    //        throw new Exception("账户不存在");

    //    // 检查是否存在支出交易
    //    bool hasTransaction = transactionRepo.HasExpenseTransaction(account.AccountID);

    //    // 🧩 核心逻辑：根据是否有交易来判断是否可修改初始余额
    //    if (hasTransaction)
    //    {
    //        // 有交易记录，不允许修改初始金额
    //        if (account.InitialAmount != existingAccount.InitialAmount)
    //            throw new InvalidOperationException("该账户已有交易记录，禁止修改初始金额。");

    //        // 保留当前余额
    //        account.CurrentBalance = existingAccount.CurrentBalance;
    //    }
    //    else
    //    {
    //        // 无交易记录时，可同步修改余额
    //        if (existingAccount.CurrentBalance == existingAccount.InitialAmount)
    //            account.CurrentBalance = account.InitialAmount;
    //    }

    //    string sql = @"
    //            UPDATE Accounts 
    //            SET AccountName = @AccountName, 
    //                AccountType = @AccountType, 
    //                InitialAmount = @InitialAmount, 
    //                CurrentBalance = @CurrentBalance,
    //                Currency = @Currency, 
    //                BankName = @BankName, 
    //                CardNumber = @CardNumber,
    //                Description = @Description, 
    //                ColorMarker = @ColorMarker
    //            WHERE AccountID = @AccountID";

    //    var parameters = new MySqlParameter[]
    //    {
    //            new MySqlParameter("@AccountName", account.AccountName),
    //            new MySqlParameter("@AccountType", account.AccountType),
    //            new MySqlParameter("@InitialAmount", account.InitialAmount),
    //            new MySqlParameter("@CurrentBalance", account.CurrentBalance),
    //            new MySqlParameter("@Currency", account.Currency),
    //            new MySqlParameter("@BankName", account.BankName ?? (object)DBNull.Value),
    //            new MySqlParameter("@CardNumber", account.CardNumber ?? (object)DBNull.Value),
    //            new MySqlParameter("@Description", account.Description ?? (object)DBNull.Value),
    //            new MySqlParameter("@ColorMarker", account.ColorMarker ?? (object)DBNull.Value),
    //            new MySqlParameter("@AccountID", account.AccountID)
    //    };

    //    return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
    //}
    //public bool UpdateAccount(Account account)
    //{
    //    var existingAccount = GetAccountById(account.AccountID);
    //    if (existingAccount == null)
    //        throw new Exception("账户不存在");

    //    // 只有当 InitialAmount == CurrentBalance 时，才允许修改
    //    if (existingAccount.InitialAmount != existingAccount.CurrentBalance)
    //    {
    //        throw new InvalidOperationException("只有当初始余额与当前余额一致时，才能修改账户信息");
    //    }

    //    // 修改时同步更新 InitialAmount 和 CurrentBalance
    //    account.CurrentBalance = account.InitialAmount;

    //    string sql = @"
    //        UPDATE Accounts 
    //        SET AccountName = @AccountName, 
    //            AccountType = @AccountType, 
    //            InitialAmount = @InitialAmount, 
    //            CurrentBalance = @CurrentBalance,
    //            Currency = @Currency, 
    //            BankName = @BankName, 
    //            CardNumber = @CardNumber,
    //            Description = @Description, 
    //            ColorMarker = @ColorMarker
    //        WHERE AccountID = @AccountID";

    //    var parameters = new MySqlParameter[]
    //    {
    //        new MySqlParameter("@AccountName", account.AccountName),
    //        new MySqlParameter("@AccountType", account.AccountType),
    //        new MySqlParameter("@InitialAmount", account.InitialAmount),
    //        new MySqlParameter("@CurrentBalance", account.CurrentBalance), // 确保同步修改余额
    //        new MySqlParameter("@Currency", account.Currency),
    //        new MySqlParameter("@BankName", account.BankName ?? (object)DBNull.Value),
    //        new MySqlParameter("@CardNumber", account.CardNumber ?? (object)DBNull.Value),
    //        new MySqlParameter("@Description", account.Description ?? (object)DBNull.Value),
    //        new MySqlParameter("@ColorMarker", account.ColorMarker ?? (object)DBNull.Value),
    //        new MySqlParameter("@AccountID", account.AccountID)
    //    };

    //    return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
    //}

    //public bool UpdateAccount(Account account)
    //{
    //    var existingAccount = GetAccountById(account.AccountID);
    //    if (existingAccount == null)
    //        throw new Exception("账户不存在");

    //    // 只有当 InitialAmount == CurrentBalance 时，才允许修改
    //    if (existingAccount.InitialAmount != existingAccount.CurrentBalance)
    //    {
    //        throw new InvalidOperationException("只有当初始余额与当前余额一致时，才能修改账户信息");
    //    }

    //    // 修改时同步更新 InitialAmount 和 CurrentBalance
    //    account.CurrentBalance = account.InitialAmount;

    //    string sql = @"
    //        UPDATE Accounts 
    //        SET AccountName = @AccountName, 
    //            AccountType = @AccountType, 
    //            InitialAmount = @InitialAmount, 
    //            CurrentBalance = @CurrentBalance,
    //            Currency = @Currency, 
    //            BankName = @BankName, 
    //            CardNumber = @CardNumber,
    //            Description = @Description, 
    //            ColorMarker = @ColorMarker
    //        WHERE AccountID = @AccountID";

    //    var parameters = new MySqlParameter[]
    //    {
    //        new MySqlParameter("@AccountName", account.AccountName),
    //        new MySqlParameter("@AccountType", account.AccountType),
    //        new MySqlParameter("@InitialAmount", account.InitialAmount),
    //        new MySqlParameter("@CurrentBalance", account.CurrentBalance), // 确保同步修改余额
    //        new MySqlParameter("@Currency", account.Currency),
    //        new MySqlParameter("@BankName", account.BankName ?? (object)DBNull.Value),
    //        new MySqlParameter("@CardNumber", account.CardNumber ?? (object)DBNull.Value),
    //        new MySqlParameter("@Description", account.Description ?? (object)DBNull.Value),
    //        new MySqlParameter("@ColorMarker", account.ColorMarker ?? (object)DBNull.Value),
    //        new MySqlParameter("@AccountID", account.AccountID)
    //    };

    //    return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
    //}
    //public bool UpdateAccount(Account account)
    //{
    //    var existingAccount = GetAccountById(account.AccountID);

    //    // 日志记录账户余额前后变化
    //    Console.WriteLine($"[UpdateAccount] 更新账户：{account.AccountName}，初始金额：{existingAccount.InitialAmount}，当前余额：{existingAccount.CurrentBalance}");

    //    if (existingAccount == null)
    //        throw new Exception("账户不存在");

    //    string sql = @"
    //        UPDATE Accounts 
    //        SET AccountName = @AccountName, 
    //            AccountType = @AccountType, 
    //            InitialAmount = @InitialAmount, 
    //            CurrentBalance = @CurrentBalance,
    //            Currency = @Currency, 
    //            BankName = @BankName, 
    //            CardNumber = @CardNumber,
    //            Description = @Description, 
    //            ColorMarker = @ColorMarker
    //        WHERE AccountID = @AccountID";

    //    var parameters = new MySqlParameter[]
    //    {
    //    new MySqlParameter("@AccountName", account.AccountName),
    //    new MySqlParameter("@AccountType", account.AccountType),
    //    new MySqlParameter("@InitialAmount", account.InitialAmount),
    //    new MySqlParameter("@CurrentBalance", account.CurrentBalance),
    //    new MySqlParameter("@Currency", account.Currency),
    //    new MySqlParameter("@BankName", account.BankName ?? (object)DBNull.Value),
    //    new MySqlParameter("@CardNumber", account.CardNumber ?? (object)DBNull.Value),
    //    new MySqlParameter("@Description", account.Description ?? (object)DBNull.Value),
    //    new MySqlParameter("@ColorMarker", account.ColorMarker ?? (object)DBNull.Value),
    //    new MySqlParameter("@AccountID", account.AccountID)
    //    };

    //    bool result = _dbHelper.ExecuteNonQuery(sql, parameters) > 0;

    //    // 日志记录更新后的账户余额
    //    Console.WriteLine($"[UpdateAccount] 账户余额更新后：账户ID：{account.AccountID}，新余额：{account.CurrentBalance}");

    //    return result;
    //}
    public bool UpdateAccount(Account account)
    {
        var existingAccount = GetAccountById(account.AccountID);

        // 日志记录账户余额前后变化
        Console.WriteLine($"[UpdateAccount] 更新账户：{account.AccountName}，初始金额：{existingAccount.InitialAmount}，当前余额：{existingAccount.CurrentBalance}");

        if (existingAccount == null)
            throw new Exception("账户不存在");

        string sql = @"
        UPDATE Accounts 
        SET AccountName = @AccountName, 
            AccountType = @AccountType, 
            InitialAmount = @InitialAmount, 
            CurrentBalance = @CurrentBalance,
            Currency = @Currency, 
            BankName = @BankName, 
            CardNumber = @CardNumber,
            Description = @Description, 
            ColorMarker = @ColorMarker
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

        bool result = _dbHelper.ExecuteNonQuery(sql, parameters) > 0;

        // 日志记录更新后的账户余额
        Console.WriteLine($"[UpdateAccount] 账户余额更新后：账户ID：{account.AccountID}，新余额：{account.CurrentBalance}");

        return result;
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