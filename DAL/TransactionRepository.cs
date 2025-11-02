using System;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using PersonalFinanceManager.Models;

namespace PersonalFinanceManager.DAL
{
    public class TransactionRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public TransactionRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

  


        public bool AddTransaction(Transaction t)
{
    string sql = @"INSERT INTO Transactions (TransactionTime, AccountID, CategoryID, Amount, TransactionType, Remark, Status)
                   VALUES (@TransactionTime,@AccountID,@CategoryID,@Amount,@TransactionType,@Remark,@Status)";
    var param = new MySqlParameter[]
    {
        new MySqlParameter("@TransactionTime", t.TransactionTime),
        new MySqlParameter("@AccountID", t.AccountID),
        new MySqlParameter("@CategoryID", t.CategoryID),
        new MySqlParameter("@Amount", t.Amount),
        new MySqlParameter("@TransactionType", t.TransactionType),
        new MySqlParameter("@Remark", t.Remark ?? ""),
        new MySqlParameter("@Status", t.Status)
    };
    return _dbHelper.ExecuteNonQuery(sql, param) > 0;
}


        public decimal GetAccountBalance(int accountId)
        {
            string sql = "SELECT CurrentBalance FROM Accounts WHERE AccountID = @AccountID";
            var parameter = new MySqlParameter("@AccountID", accountId);
            var result = _dbHelper.ExecuteScalar(sql, new[] { parameter });

            decimal balance = result == DBNull.Value ? 0 : Convert.ToDecimal(result);
            Console.WriteLine($"[GetAccountBalance] 账户ID={accountId} 当前余额={balance}");
            return balance;
        }




        public List<Transaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
        {
            var transactions = new List<Transaction>();
            string sql = @"
                SELECT t.*, a.AccountName, c.CategoryName 
                FROM Transactions t
                LEFT JOIN Accounts a ON t.AccountID = a.AccountID
                LEFT JOIN Categories c ON t.CategoryID = c.CategoryID
                WHERE t.TransactionTime BETWEEN @StartDate AND @EndDate
                ORDER BY t.TransactionTime DESC";

            var parameters = new MySqlParameter[]
            {
                new MySqlParameter("@StartDate", startDate),
                new MySqlParameter("@EndDate", endDate.AddDays(1).AddSeconds(-1))
            };

            var dataTable = _dbHelper.ExecuteQuery(sql, parameters);
            foreach (DataRow row in dataTable.Rows)
            {
                transactions.Add(new Transaction
                {
                    TransactionID = Convert.ToInt32(row["TransactionID"]),
                    TransactionTime = Convert.ToDateTime(row["TransactionTime"]),
                    AccountID = Convert.ToInt32(row["AccountID"]),
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    Amount = Convert.ToDecimal(row["Amount"]),
                    TransactionType = row["TransactionType"].ToString(),
                    TargetAccount = row["TargetAccount"] == DBNull.Value ? null : row["TargetAccount"].ToString(),
                    Remark = row["Remark"] == DBNull.Value ? null : row["Remark"].ToString(),
                    ImagePath = row["ImagePath"] == DBNull.Value ? null : row["ImagePath"].ToString(),
                    Location = row["Location"] == DBNull.Value ? null : row["Location"].ToString(),
                    Tags = row["Tags"] == DBNull.Value ? null : row["Tags"].ToString(),
                    Status = row["Status"].ToString(),
                    CreateTime = Convert.ToDateTime(row["CreateTime"]),
                    UpdateTime = Convert.ToDateTime(row["UpdateTime"]),
                    SyncStatus = row["SyncStatus"].ToString(),
                    SerialNumber = row["SerialNumber"].ToString(),
                    AccountName = row["AccountName"].ToString(),
                    CategoryName = row["CategoryName"].ToString()
                });
            }
            return transactions;
        }

        public List<Transaction> GetRecentTransactions(int count = 50)
        {
            var transactions = new List<Transaction>();
            string sql = @"
                SELECT t.*, a.AccountName, c.CategoryName 
                FROM Transactions t
                LEFT JOIN Accounts a ON t.AccountID = a.AccountID
                LEFT JOIN Categories c ON t.CategoryID = c.CategoryID
                ORDER BY t.TransactionTime DESC 
                LIMIT @Count";

            var parameter = new MySqlParameter("@Count", count);

            var dataTable = _dbHelper.ExecuteQuery(sql, new[] { parameter });
            foreach (DataRow row in dataTable.Rows)
            {
                transactions.Add(new Transaction
                {
                    TransactionID = Convert.ToInt32(row["TransactionID"]),
                    TransactionTime = Convert.ToDateTime(row["TransactionTime"]),
                    AccountID = Convert.ToInt32(row["AccountID"]),
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    Amount = Convert.ToDecimal(row["Amount"]),
                    TransactionType = row["TransactionType"].ToString(),
                    TargetAccount = row["TargetAccount"] == DBNull.Value ? null : row["TargetAccount"].ToString(),
                    Remark = row["Remark"] == DBNull.Value ? null : row["Remark"].ToString(),
                    ImagePath = row["ImagePath"] == DBNull.Value ? null : row["ImagePath"].ToString(),
                    Location = row["Location"] == DBNull.Value ? null : row["Location"].ToString(),
                    Tags = row["Tags"] == DBNull.Value ? null : row["Tags"].ToString(),
                    Status = row["Status"].ToString(),
                    CreateTime = Convert.ToDateTime(row["CreateTime"]),
                    UpdateTime = Convert.ToDateTime(row["UpdateTime"]),
                    SyncStatus = row["SyncStatus"].ToString(),
                    SerialNumber = row["SerialNumber"].ToString(),
                    AccountName = row["AccountName"].ToString(),
                    CategoryName = row["CategoryName"].ToString()
                });
            }
            return transactions;
        }

        public bool DeleteTransaction(int transactionId)
        {
            string sql = "DELETE FROM Transactions WHERE TransactionID = @TransactionID";
            var parameter = new MySqlParameter("@TransactionID", transactionId);
            return _dbHelper.ExecuteNonQuery(sql, new[] { parameter }) > 0;
        }
    
    // 在 TransactionRepository 类中添加以下方法
public Transaction GetTransactionById(int transactionId)
        {
            string sql = @"
        SELECT t.*, a.AccountName, c.CategoryName 
        FROM Transactions t
        LEFT JOIN Accounts a ON t.AccountID = a.AccountID
        LEFT JOIN Categories c ON t.CategoryID = c.CategoryID
        WHERE t.TransactionID = @TransactionID";

            var parameter = new MySqlParameter("@TransactionID", transactionId);

            var dataTable = _dbHelper.ExecuteQuery(sql, new[] { parameter });
            if (dataTable.Rows.Count == 0)
                return null;

            var row = dataTable.Rows[0];
            return new Transaction
            {
                TransactionID = Convert.ToInt32(row["TransactionID"]),
                TransactionTime = Convert.ToDateTime(row["TransactionTime"]),
                AccountID = Convert.ToInt32(row["AccountID"]),
                CategoryID = Convert.ToInt32(row["CategoryID"]),
                Amount = Convert.ToDecimal(row["Amount"]),
                TransactionType = row["TransactionType"].ToString(),
                TargetAccount = row["TargetAccount"] == DBNull.Value ? null : row["TargetAccount"].ToString(),
                Remark = row["Remark"] == DBNull.Value ? null : row["Remark"].ToString(),
                ImagePath = row["ImagePath"] == DBNull.Value ? null : row["ImagePath"].ToString(),
                Location = row["Location"] == DBNull.Value ? null : row["Location"].ToString(),
                Tags = row["Tags"] == DBNull.Value ? null : row["Tags"].ToString(),
                Status = row["Status"].ToString(),
                CreateTime = Convert.ToDateTime(row["CreateTime"]),
                UpdateTime = Convert.ToDateTime(row["UpdateTime"]),
                SyncStatus = row["SyncStatus"].ToString(),
                SerialNumber = row["SerialNumber"].ToString(),
                AccountName = row["AccountName"].ToString(),
                CategoryName = row["CategoryName"].ToString()
            };
        }

        public bool UpdateTransaction(Transaction transaction)
        {
            string sql = @"
        UPDATE Transactions 
        SET TransactionTime = @TransactionTime, AccountID = @AccountID, CategoryID = @CategoryID,
            Amount = @Amount, TransactionType = @TransactionType, TargetAccount = @TargetAccount,
            Remark = @Remark, ImagePath = @ImagePath, Location = @Location, Tags = @Tags,
            Status = @Status, UpdateTime = CURRENT_TIMESTAMP
        WHERE TransactionID = @TransactionID";

            var parameters = new MySqlParameter[]
            {
        new MySqlParameter("@TransactionTime", transaction.TransactionTime),
        new MySqlParameter("@AccountID", transaction.AccountID),
        new MySqlParameter("@CategoryID", transaction.CategoryID),
        new MySqlParameter("@Amount", transaction.Amount),
        new MySqlParameter("@TransactionType", transaction.TransactionType),
        new MySqlParameter("@TargetAccount", transaction.TargetAccount ?? (object)DBNull.Value),
        new MySqlParameter("@Remark", transaction.Remark ?? (object)DBNull.Value),
        new MySqlParameter("@ImagePath", transaction.ImagePath ?? (object)DBNull.Value),
        new MySqlParameter("@Location", transaction.Location ?? (object)DBNull.Value),
        new MySqlParameter("@Tags", transaction.Tags ?? (object)DBNull.Value),
        new MySqlParameter("@Status", transaction.Status),
        new MySqlParameter("@TransactionID", transaction.TransactionID)
            };

            return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        }

        public List<Transaction> GetTransactionsByAccountId(int accountId)
        {
            var transactions = new List<Transaction>();
            string sql = @"
        SELECT t.*, a.AccountName, c.CategoryName 
        FROM Transactions t
        LEFT JOIN Accounts a ON t.AccountID = a.AccountID
        LEFT JOIN Categories c ON t.CategoryID = c.CategoryID
        WHERE t.AccountID = @AccountID
        ORDER BY t.TransactionTime DESC";

            var parameter = new MySqlParameter("@AccountID", accountId);

            var dataTable = _dbHelper.ExecuteQuery(sql, new[] { parameter });
            foreach (DataRow row in dataTable.Rows)
            {
                transactions.Add(new Transaction
                {
                    TransactionID = Convert.ToInt32(row["TransactionID"]),
                    TransactionTime = Convert.ToDateTime(row["TransactionTime"]),
                    AccountID = Convert.ToInt32(row["AccountID"]),
                    CategoryID = Convert.ToInt32(row["CategoryID"]),
                    Amount = Convert.ToDecimal(row["Amount"]),
                    TransactionType = row["TransactionType"].ToString(),
                    TargetAccount = row["TargetAccount"] == DBNull.Value ? null : row["TargetAccount"].ToString(),
                    Remark = row["Remark"] == DBNull.Value ? null : row["Remark"].ToString(),
                    ImagePath = row["ImagePath"] == DBNull.Value ? null : row["ImagePath"].ToString(),
                    Location = row["Location"] == DBNull.Value ? null : row["Location"].ToString(),
                    Tags = row["Tags"] == DBNull.Value ? null : row["Tags"].ToString(),
                    Status = row["Status"].ToString(),
                    CreateTime = Convert.ToDateTime(row["CreateTime"]),
                    UpdateTime = row["UpdateTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(row["UpdateTime"]),
                    SyncStatus = row["SyncStatus"] == DBNull.Value ? "" : row["SyncStatus"].ToString(),
                    SerialNumber = row["SerialNumber"] == DBNull.Value ? "" : row["SerialNumber"].ToString(),
                    AccountName = row["AccountName"].ToString(),
                    CategoryName = row["CategoryName"].ToString()
                });
            }

            return transactions;
        }


        public bool HasExpenseTransaction(int accountId)
        {
            string sql = "SELECT COUNT(*) FROM Transactions WHERE AccountID = @AccountID AND TransactionType = '支出'";
            var parameter = new MySqlParameter("@AccountID", accountId);
            var result = _dbHelper.ExecuteScalar(sql, new[] { parameter });
            return Convert.ToInt32(result) > 0;
        }



    }
}