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

        //public bool AddTransaction(Transaction transaction)
        //{
        //    string sql = @"
        //        INSERT INTO Transactions (TransactionTime, AccountID, CategoryID, Amount, 
        //                                TransactionType, TargetAccount, Remark, ImagePath, 
        //                                Location, Tags, Status, SerialNumber)
        //        VALUES (@TransactionTime, @AccountID, @CategoryID, @Amount, 
        //               @TransactionType, @TargetAccount, @Remark, @ImagePath, 
        //               @Location, @Tags, @Status, @SerialNumber)";

        //    var parameters = new MySqlParameter[]
        //    {
        //        new MySqlParameter("@TransactionTime", transaction.TransactionTime),
        //        new MySqlParameter("@AccountID", transaction.AccountID),
        //        new MySqlParameter("@CategoryID", transaction.CategoryID),
        //        new MySqlParameter("@Amount", transaction.Amount),
        //        new MySqlParameter("@TransactionType", transaction.TransactionType),
        //        new MySqlParameter("@TargetAccount", transaction.TargetAccount ?? (object)DBNull.Value),
        //        new MySqlParameter("@Remark", transaction.Remark ?? (object)DBNull.Value),
        //        new MySqlParameter("@ImagePath", transaction.ImagePath ?? (object)DBNull.Value),
        //        new MySqlParameter("@Location", transaction.Location ?? (object)DBNull.Value),
        //        new MySqlParameter("@Tags", transaction.Tags ?? (object)DBNull.Value),
        //        new MySqlParameter("@Status", transaction.Status),
        //        new MySqlParameter("@SerialNumber", transaction.SerialNumber)
        //    };

        //    return _dbHelper.ExecuteNonQuery(sql, parameters) > 0;
        //}

        /// <summary>
        /// 新增交易记录，并自动更新账户余额
        /// </summary>
        //public bool AddTransaction(Transaction transaction)
        //{
        //    using (var conn = _dbHelper.GetConnection())
        //    {
        //        conn.Open();
        //        using (var transactionScope = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                // 插入交易记录
        //                string insertSql = @"
        //                    INSERT INTO Transactions (TransactionTime, AccountID, CategoryID, Amount, 
        //                                            TransactionType, TargetAccount, Remark, ImagePath, 
        //                                            Location, Tags, Status, SerialNumber)
        //                    VALUES (@TransactionTime, @AccountID, @CategoryID, @Amount, 
        //                           @TransactionType, @TargetAccount, @Remark, @ImagePath, 
        //                           @Location, @Tags, @Status, @SerialNumber)";

        //                var parameters = new MySqlParameter[]
        //                {
        //                    new MySqlParameter("@TransactionTime", transaction.TransactionTime),
        //                    new MySqlParameter("@AccountID", transaction.AccountID),
        //                    new MySqlParameter("@CategoryID", transaction.CategoryID),
        //                    new MySqlParameter("@Amount", transaction.Amount),
        //                    new MySqlParameter("@TransactionType", transaction.TransactionType),
        //                    new MySqlParameter("@TargetAccount", transaction.TargetAccount ?? (object)DBNull.Value),
        //                    new MySqlParameter("@Remark", transaction.Remark ?? (object)DBNull.Value),
        //                    new MySqlParameter("@ImagePath", transaction.ImagePath ?? (object)DBNull.Value),
        //                    new MySqlParameter("@Location", transaction.Location ?? (object)DBNull.Value),
        //                    new MySqlParameter("@Tags", transaction.Tags ?? (object)DBNull.Value),
        //                    new MySqlParameter("@Status", transaction.Status),
        //                    new MySqlParameter("@SerialNumber", transaction.SerialNumber)
        //                };

        //                int rows = _dbHelper.ExecuteNonQuery(insertSql, parameters, conn, transactionScope);
        //                if (rows <= 0)
        //                    throw new Exception("插入交易记录失败");

        //                // 更新账户余额逻辑
        //                string updateBalanceSql = "";
        //                if (transaction.TransactionType == "收入")
        //                {
        //                    updateBalanceSql = "UPDATE Accounts SET CurrentBalance = CurrentBalance + @Amount WHERE AccountID = @AccountID";
        //                }
        //                else if (transaction.TransactionType == "支出")
        //                {
        //                    updateBalanceSql = "UPDATE Accounts SET CurrentBalance = CurrentBalance - @Amount WHERE AccountID = @AccountID";
        //                }

        //                if (!string.IsNullOrEmpty(updateBalanceSql))
        //                {
        //                    var balanceParams = new MySqlParameter[]
        //                    {
        //                        new MySqlParameter("@Amount", transaction.Amount),
        //                        new MySqlParameter("@AccountID", transaction.AccountID)
        //                    };
        //                    _dbHelper.ExecuteNonQuery(updateBalanceSql, balanceParams, conn, transactionScope);
        //                }

        //                transactionScope.Commit();
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                transactionScope.Rollback();
        //                Console.WriteLine($"添加交易失败: {ex.Message}");
        //                return false;
        //            }
        //        }
        //    }
        //}
        //        public bool AddTransaction(Transaction transaction)
        //{
        //    using (var conn = _dbHelper.GetConnection())
        //    {
        //        conn.Open();
        //        using (var transactionScope = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                // 插入交易记录
        //                string insertSql = @"
        //                    INSERT INTO Transactions (TransactionTime, AccountID, CategoryID, Amount, 
        //                                              TransactionType, TargetAccount, Remark, ImagePath, 
        //                                              Location, Tags, Status, SerialNumber)
        //                    VALUES (@TransactionTime, @AccountID, @CategoryID, @Amount, 
        //                           @TransactionType, @TargetAccount, @Remark, @ImagePath, 
        //                           @Location, @Tags, @Status, @SerialNumber)";

        //                var parameters = new MySqlParameter[]
        //                {
        //                    new MySqlParameter("@TransactionTime", transaction.TransactionTime),
        //                    new MySqlParameter("@AccountID", transaction.AccountID),
        //                    new MySqlParameter("@CategoryID", transaction.CategoryID),
        //                    new MySqlParameter("@Amount", transaction.Amount),
        //                    new MySqlParameter("@TransactionType", transaction.TransactionType),
        //                    new MySqlParameter("@TargetAccount", transaction.TargetAccount ?? (object)DBNull.Value),
        //                    new MySqlParameter("@Remark", transaction.Remark ?? (object)DBNull.Value),
        //                    new MySqlParameter("@ImagePath", transaction.ImagePath ?? (object)DBNull.Value),
        //                    new MySqlParameter("@Location", transaction.Location ?? (object)DBNull.Value),
        //                    new MySqlParameter("@Tags", transaction.Tags ?? (object)DBNull.Value),
        //                    new MySqlParameter("@Status", transaction.Status),
        //                    new MySqlParameter("@SerialNumber", transaction.SerialNumber)
        //                };

        //                int rows = _dbHelper.ExecuteNonQuery(insertSql, parameters, conn, transactionScope);
        //                if (rows <= 0)
        //                    throw new Exception("插入交易记录失败");

        //                // 更新账户余额逻辑，确保只在收入或支出时更新余额
        //                string updateBalanceSql = "";
        //                if (transaction.TransactionType == "收入")
        //                {
        //                    updateBalanceSql = "UPDATE Accounts SET CurrentBalance = CurrentBalance + @Amount WHERE AccountID = @AccountID";
        //                }
        //                else if (transaction.TransactionType == "支出")
        //                {
        //                    updateBalanceSql = "UPDATE Accounts SET CurrentBalance = CurrentBalance - @Amount WHERE AccountID = @AccountID";
        //                }

        //                if (!string.IsNullOrEmpty(updateBalanceSql))
        //                {
        //                    var balanceParams = new MySqlParameter[]
        //                    {
        //                        new MySqlParameter("@Amount", transaction.Amount),
        //                        new MySqlParameter("@AccountID", transaction.AccountID)
        //                    };
        //                    _dbHelper.ExecuteNonQuery(updateBalanceSql, balanceParams, conn, transactionScope);
        //                }

        //                transactionScope.Commit();
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                transactionScope.Rollback();
        //                Console.WriteLine($"添加交易失败: {ex.Message}");
        //                return false;
        //            }
        //        }
        //    }
        //}
        //public bool AddTransaction(Transaction transaction)
        //{
        //    using (var conn = _dbHelper.GetConnection())
        //    {
        //        conn.Open();
        //        using (var transactionScope = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                // 插入交易记录
        //                string insertSql = @"
        //            INSERT INTO Transactions (TransactionTime, AccountID, CategoryID, Amount, 
        //                                    TransactionType, TargetAccount, Remark, ImagePath, 
        //                                    Location, Tags, Status, SerialNumber)
        //            VALUES (@TransactionTime, @AccountID, @CategoryID, @Amount, 
        //                   @TransactionType, @TargetAccount, @Remark, @ImagePath, 
        //                   @Location, @Tags, @Status, @SerialNumber)";

        //                var parameters = new MySqlParameter[]
        //                {
        //            new MySqlParameter("@TransactionTime", transaction.TransactionTime),
        //            new MySqlParameter("@AccountID", transaction.AccountID),
        //            new MySqlParameter("@CategoryID", transaction.CategoryID),
        //            new MySqlParameter("@Amount", transaction.Amount),
        //            new MySqlParameter("@TransactionType", transaction.TransactionType),
        //            new MySqlParameter("@TargetAccount", transaction.TargetAccount ?? (object)DBNull.Value),
        //            new MySqlParameter("@Remark", transaction.Remark ?? (object)DBNull.Value),
        //            new MySqlParameter("@ImagePath", transaction.ImagePath ?? (object)DBNull.Value),
        //            new MySqlParameter("@Location", transaction.Location ?? (object)DBNull.Value),
        //            new MySqlParameter("@Tags", transaction.Tags ?? (object)DBNull.Value),
        //            new MySqlParameter("@Status", transaction.Status),
        //            new MySqlParameter("@SerialNumber", transaction.SerialNumber)
        //                };

        //                int rows = _dbHelper.ExecuteNonQuery(insertSql, parameters, conn, transactionScope);
        //                if (rows <= 0)
        //                    throw new Exception("插入交易记录失败");

        //                // 日志记录
        //                Console.WriteLine($"[AddTransaction] 交易类型：{transaction.TransactionType}，账户ID：{transaction.AccountID}，金额：{transaction.Amount}");

        //                // 更新账户余额逻辑
        //                string updateBalanceSql = "";
        //                if (transaction.TransactionType == "收入")
        //                {
        //                    updateBalanceSql = "UPDATE Accounts SET CurrentBalance = CurrentBalance + @Amount WHERE AccountID = @AccountID";
        //                }
        //                else if (transaction.TransactionType == "支出")
        //                {
        //                    updateBalanceSql = "UPDATE Accounts SET CurrentBalance = CurrentBalance - @Amount WHERE AccountID = @AccountID";
        //                }

        //                if (!string.IsNullOrEmpty(updateBalanceSql))
        //                {
        //                    var balanceParams = new MySqlParameter[]
        //                    {
        //                new MySqlParameter("@Amount", transaction.Amount),
        //                new MySqlParameter("@AccountID", transaction.AccountID)
        //                    };
        //                    _dbHelper.ExecuteNonQuery(updateBalanceSql, balanceParams, conn, transactionScope);
        //                    // 日志记录账户余额更新
        //                    Console.WriteLine($"[AddTransaction] 更新账户余额：账户ID：{transaction.AccountID}，新余额：{GetAccountBalance(transaction.AccountID)}");
        //                }

        //                transactionScope.Commit();
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                transactionScope.Rollback();
        //                Console.WriteLine($"添加交易失败: {ex.Message}");
        //                return false;
        //            }
        //        }
        //    }
        //}

        //public bool AddTransaction(Transaction transaction)
        //{
        //    // 日志：进入 AddTransaction 方法
        //    Console.WriteLine($"[AddTransaction] 开始处理交易：交易类型={transaction.TransactionType}, 账户ID={transaction.AccountID}, 金额={transaction.Amount}");

        //    using (var conn = _dbHelper.GetConnection())
        //    {
        //        conn.Open();
        //        using (var transactionScope = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                // 插入交易记录
        //                string insertSql = @"
        //            INSERT INTO Transactions (TransactionTime, AccountID, CategoryID, Amount, 
        //                                      TransactionType, TargetAccount, Remark, ImagePath, 
        //                                      Location, Tags, Status, SerialNumber)
        //            VALUES (@TransactionTime, @AccountID, @CategoryID, @Amount, 
        //                   @TransactionType, @TargetAccount, @Remark, @ImagePath, 
        //                   @Location, @Tags, @Status, @SerialNumber)";

        //                var parameters = new MySqlParameter[]
        //                {
        //            new MySqlParameter("@TransactionTime", transaction.TransactionTime),
        //            new MySqlParameter("@AccountID", transaction.AccountID),
        //            new MySqlParameter("@CategoryID", transaction.CategoryID),
        //            new MySqlParameter("@Amount", transaction.Amount),
        //            new MySqlParameter("@TransactionType", transaction.TransactionType),
        //            new MySqlParameter("@TargetAccount", transaction.TargetAccount ?? (object)DBNull.Value),
        //            new MySqlParameter("@Remark", transaction.Remark ?? (object)DBNull.Value),
        //            new MySqlParameter("@ImagePath", transaction.ImagePath ?? (object)DBNull.Value),
        //            new MySqlParameter("@Location", transaction.Location ?? (object)DBNull.Value),
        //            new MySqlParameter("@Tags", transaction.Tags ?? (object)DBNull.Value),
        //            new MySqlParameter("@Status", transaction.Status),
        //            new MySqlParameter("@SerialNumber", transaction.SerialNumber)
        //                };

        //                int rows = _dbHelper.ExecuteNonQuery(insertSql, parameters, conn, transactionScope);
        //                if (rows <= 0)
        //                {
        //                    Console.WriteLine("[AddTransaction] 插入交易记录失败！");
        //                    throw new Exception("插入交易记录失败");
        //                }

        //                // 日志：交易记录插入成功
        //                Console.WriteLine("[AddTransaction] 交易记录插入成功");

        //                // 更新账户余额逻辑
        //                string updateBalanceSql = "";
        //                if (transaction.TransactionType == "收入")
        //                {
        //                    updateBalanceSql = "UPDATE Accounts SET CurrentBalance = CurrentBalance + @Amount WHERE AccountID = @AccountID";
        //                }
        //                else if (transaction.TransactionType == "支出")
        //                {
        //                    updateBalanceSql = "UPDATE Accounts SET CurrentBalance = CurrentBalance - @Amount WHERE AccountID = @AccountID";
        //                }

        //                if (!string.IsNullOrEmpty(updateBalanceSql))
        //                {
        //                    var balanceParams = new MySqlParameter[]
        //                    {
        //                new MySqlParameter("@Amount", transaction.Amount),
        //                new MySqlParameter("@AccountID", transaction.AccountID)
        //                    };

        //                    // 日志：更新账户余额
        //                    Console.WriteLine($"[AddTransaction] 开始更新账户余额：账户ID={transaction.AccountID}, 金额={transaction.Amount}, 类型={transaction.TransactionType}");

        //                    _dbHelper.ExecuteNonQuery(updateBalanceSql, balanceParams, conn, transactionScope);

        //                    // 日志：账户余额更新完成
        //                    decimal newBalance = GetAccountBalance(transaction.AccountID); // 获取更新后的账户余额
        //                    Console.WriteLine($"[AddTransaction] 账户余额更新完成：账户ID={transaction.AccountID}, 新余额={newBalance}");
        //                }

        //                // 提交事务
        //                transactionScope.Commit();
        //                Console.WriteLine("[AddTransaction] 事务提交成功");
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                // 回滚事务
        //                transactionScope.Rollback();
        //                Console.WriteLine($"[AddTransaction] 异常发生，事务回滚：{ex.Message}");
        //                return false;
        //            }
        //        }
        //    }
        //}
        //public bool AddTransaction(Transaction transaction)
        //{
        //    // 日志：进入 AddTransaction 方法
        //    Console.WriteLine($"[AddTransaction] 开始处理交易：交易类型={transaction.TransactionType}, 账户ID={transaction.AccountID}, 金额={transaction.Amount}");

        //    using (var conn = _dbHelper.GetConnection())
        //    {
        //        conn.Open();
        //        using (var transactionScope = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                // 插入交易记录
        //                string insertSql = @"
        //            INSERT INTO Transactions (TransactionTime, AccountID, CategoryID, Amount, 
        //                                      TransactionType, TargetAccount, Remark, ImagePath, 
        //                                      Location, Tags, Status, SerialNumber)
        //            VALUES (@TransactionTime, @AccountID, @CategoryID, @Amount, 
        //                   @TransactionType, @TargetAccount, @Remark, @ImagePath, 
        //                   @Location, @Tags, @Status, @SerialNumber)";

        //                var parameters = new MySqlParameter[]
        //                {
        //            new MySqlParameter("@TransactionTime", transaction.TransactionTime),
        //            new MySqlParameter("@AccountID", transaction.AccountID),
        //            new MySqlParameter("@CategoryID", transaction.CategoryID),
        //            new MySqlParameter("@Amount", transaction.Amount),
        //            new MySqlParameter("@TransactionType", transaction.TransactionType),
        //            new MySqlParameter("@TargetAccount", transaction.TargetAccount ?? (object)DBNull.Value),
        //            new MySqlParameter("@Remark", transaction.Remark ?? (object)DBNull.Value),
        //            new MySqlParameter("@ImagePath", transaction.ImagePath ?? (object)DBNull.Value),
        //            new MySqlParameter("@Location", transaction.Location ?? (object)DBNull.Value),
        //            new MySqlParameter("@Tags", transaction.Tags ?? (object)DBNull.Value),
        //            new MySqlParameter("@Status", transaction.Status),
        //            new MySqlParameter("@SerialNumber", transaction.SerialNumber)
        //                };

        //                int rows = _dbHelper.ExecuteNonQuery(insertSql, parameters, conn, transactionScope);
        //                if (rows <= 0)
        //                {
        //                    Console.WriteLine("[AddTransaction] 插入交易记录失败！");
        //                    throw new Exception("插入交易记录失败");
        //                }

        //                // 日志：交易记录插入成功
        //                Console.WriteLine("[AddTransaction] 交易记录插入成功");

        //                // 更新账户余额逻辑
        //                string updateBalanceSql = "";
        //                if (transaction.TransactionType == "收入")
        //                {
        //                    updateBalanceSql = "UPDATE Accounts SET CurrentBalance = CurrentBalance + @Amount WHERE AccountID = @AccountID";
        //                }
        //                else if (transaction.TransactionType == "支出")
        //                {
        //                    updateBalanceSql = "UPDATE Accounts SET CurrentBalance = CurrentBalance - @Amount WHERE AccountID = @AccountID";
        //                }

        //                if (!string.IsNullOrEmpty(updateBalanceSql))
        //                {
        //                    var balanceParams = new MySqlParameter[]
        //                    {
        //                new MySqlParameter("@Amount", transaction.Amount),
        //                new MySqlParameter("@AccountID", transaction.AccountID)
        //                    };

        //                    // 日志：更新账户余额
        //                    Console.WriteLine($"[AddTransaction] 开始更新账户余额：账户ID={transaction.AccountID}, 金额={transaction.Amount}, 类型={transaction.TransactionType}");

        //                    _dbHelper.ExecuteNonQuery(updateBalanceSql, balanceParams, conn, transactionScope);

        //                    // 日志：账户余额更新完成
        //                    decimal newBalance = GetAccountBalance(transaction.AccountID); // 获取更新后的账户余额
        //                    Console.WriteLine($"[AddTransaction] 账户余额更新完成：账户ID={transaction.AccountID}, 新余额={newBalance}");
        //                }

        //                // 提交事务
        //                transactionScope.Commit();
        //                Console.WriteLine("[AddTransaction] 事务提交成功");
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                // 回滚事务
        //                transactionScope.Rollback();
        //                Console.WriteLine($"[AddTransaction] 异常发生，事务回滚：{ex.Message}");
        //                return false;
        //            }
        //        }
        //    }
        //}

        //public bool AddTransaction(Transaction transaction)
        //{
        //    using (var conn = _dbHelper.GetConnection())
        //    {
        //        conn.Open();
        //        using (var transactionScope = conn.BeginTransaction())
        //        {
        //            try
        //            {
        //                // 插入交易记录
        //                string insertSql = @"
        //            INSERT INTO Transactions (TransactionTime, AccountID, CategoryID, Amount, 
        //                                      TransactionType, TargetAccount, Remark, ImagePath, 
        //                                      Location, Tags, Status, SerialNumber)
        //            VALUES (@TransactionTime, @AccountID, @CategoryID, @Amount, 
        //                   @TransactionType, @TargetAccount, @Remark, @ImagePath, 
        //                   @Location, @Tags, @Status, @SerialNumber)";
        //                var parameters = new MySqlParameter[]
        //                {
        //            new MySqlParameter("@TransactionTime", transaction.TransactionTime),
        //            new MySqlParameter("@AccountID", transaction.AccountID),
        //            new MySqlParameter("@CategoryID", transaction.CategoryID),
        //            new MySqlParameter("@Amount", transaction.Amount),
        //            new MySqlParameter("@TransactionType", transaction.TransactionType),
        //            new MySqlParameter("@TargetAccount", transaction.TargetAccount ?? (object)DBNull.Value),
        //            new MySqlParameter("@Remark", transaction.Remark ?? (object)DBNull.Value),
        //            new MySqlParameter("@ImagePath", transaction.ImagePath ?? (object)DBNull.Value),
        //            new MySqlParameter("@Location", transaction.Location ?? (object)DBNull.Value),
        //            new MySqlParameter("@Tags", transaction.Tags ?? (object)DBNull.Value),
        //            new MySqlParameter("@Status", transaction.Status),
        //            new MySqlParameter("@SerialNumber", transaction.SerialNumber)
        //                };
        //                int rows = _dbHelper.ExecuteNonQuery(insertSql, parameters, conn, transactionScope);
        //                if (rows <= 0)
        //                    throw new Exception("插入交易记录失败");

        //                // 确认插入前余额
        //                decimal previousBalance = GetAccountBalance(transaction.AccountID);
        //                Console.WriteLine($"[AddTransaction] 插入前账户余额：账户ID={transaction.AccountID}，原余额={previousBalance}");

        //                // 更新账户余额逻辑
        //                string updateBalanceSql = "";
        //                if (transaction.TransactionType == "收入")
        //                {
        //                    updateBalanceSql = "UPDATE Accounts SET CurrentBalance = CurrentBalance + @Amount WHERE AccountID = @AccountID";
        //                }
        //                else if (transaction.TransactionType == "支出")
        //                {
        //                    updateBalanceSql = "UPDATE Accounts SET CurrentBalance = CurrentBalance - @Amount WHERE AccountID = @AccountID";
        //                }

        //                if (!string.IsNullOrEmpty(updateBalanceSql))
        //                {
        //                    var balanceParams = new MySqlParameter[]
        //                    {
        //                new MySqlParameter("@Amount", transaction.Amount),
        //                new MySqlParameter("@AccountID", transaction.AccountID)
        //                    };
        //                    int updatedRows = _dbHelper.ExecuteNonQuery(updateBalanceSql, balanceParams, conn, transactionScope);

        //                    // 确保 SQL 更新操作成功
        //                    if (updatedRows == 0)
        //                    {
        //                        Console.WriteLine($"[AddTransaction] 未能更新账户余额：账户ID={transaction.AccountID}，可能是没有匹配到账户");
        //                    }
        //                    else
        //                    {
        //                        // 记录更新后的余额
        //                        decimal updatedBalance = GetAccountBalance(transaction.AccountID);
        //                        Console.WriteLine($"[AddTransaction] 更新后账户余额：账户ID={transaction.AccountID}，新余额={updatedBalance}");

        //                        // 如果余额还是为0，设置为初始余额
        //                        if (updatedBalance == 0)
        //                        {
        //                            string updateInitialBalanceSql = "UPDATE Accounts SET CurrentBalance = InitialAmount WHERE AccountID = @AccountID";
        //                            _dbHelper.ExecuteNonQuery(updateInitialBalanceSql, balanceParams, conn, transactionScope);
        //                            updatedBalance = GetAccountBalance(transaction.AccountID); // 重新获取更新后的余额
        //                            Console.WriteLine($"[AddTransaction] 当前余额为0，已设置为初始金额，账户ID={transaction.AccountID}，新余额={updatedBalance}");
        //                        }
        //                    }
        //                }

        //                // 提交事务
        //                transactionScope.Commit();
        //                Console.WriteLine("[AddTransaction] 事务提交成功");
        //                return true;
        //            }
        //            catch (Exception ex)
        //            {
        //                transactionScope.Rollback();
        //                Console.WriteLine($"添加交易失败: {ex.Message}");
        //                return false;
        //            }
        //        }
        //    }
        //}
        public bool AddTransaction(Transaction transaction)
        {
            using (var conn = _dbHelper.GetConnection())
            {
                conn.Open();
                using (var transactionScope = conn.BeginTransaction())
                {
                    try
                    {
                        // 插入交易记录
                        string insertSql = @"
                    INSERT INTO Transactions (TransactionTime, AccountID, CategoryID, Amount, 
                                              TransactionType, TargetAccount, Remark, ImagePath, 
                                              Location, Tags, Status, SerialNumber)
                    VALUES (@TransactionTime, @AccountID, @CategoryID, @Amount, 
                           @TransactionType, @TargetAccount, @Remark, @ImagePath, 
                           @Location, @Tags, @Status, @SerialNumber)";
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
                    new MySqlParameter("@SerialNumber", transaction.SerialNumber)
                        };

                        int rows = _dbHelper.ExecuteNonQuery(insertSql, parameters, conn, transactionScope);
                        if (rows <= 0)
                            throw new Exception("插入交易记录失败");

                        // 确认插入前余额
                        decimal previousBalance = GetAccountBalance(transaction.AccountID);
                        Console.WriteLine($"[AddTransaction] 插入前账户余额：账户ID={transaction.AccountID}，原余额={previousBalance}");

                        // 更新账户余额逻辑
                        string updateBalanceSql = "";
                        if (transaction.TransactionType == "收入")
                        {
                            updateBalanceSql = "UPDATE Accounts SET CurrentBalance = CurrentBalance + @Amount WHERE AccountID = @AccountID";
                        }
                        else if (transaction.TransactionType == "支出")
                        {
                            updateBalanceSql = "UPDATE Accounts SET CurrentBalance = CurrentBalance - @Amount WHERE AccountID = @AccountID";
                        }

                        if (!string.IsNullOrEmpty(updateBalanceSql))
                        {
                            var balanceParams = new MySqlParameter[]
                            {
                        new MySqlParameter("@Amount", transaction.Amount),
                        new MySqlParameter("@AccountID", transaction.AccountID)
                            };
                            int updatedRows = _dbHelper.ExecuteNonQuery(updateBalanceSql, balanceParams, conn, transactionScope);

                            // 确保 SQL 更新操作成功
                            if (updatedRows == 0)
                            {
                                Console.WriteLine($"[AddTransaction] 未能更新账户余额：账户ID={transaction.AccountID}，可能是没有匹配到账户");
                            }
                            else
                            {
                                // 记录更新后的余额
                                decimal updatedBalance = GetAccountBalance(transaction.AccountID);
                                Console.WriteLine($"[AddTransaction] 更新后账户余额：账户ID={transaction.AccountID}，新余额={updatedBalance}");

                                // 如果余额为0，设置为初始金额
                                if (updatedBalance == 0)
                                {
                                    string updateInitialBalanceSql = "UPDATE Accounts SET CurrentBalance = InitialAmount WHERE AccountID = @AccountID";
                                    _dbHelper.ExecuteNonQuery(updateInitialBalanceSql, balanceParams, conn, transactionScope);
                                    updatedBalance = GetAccountBalance(transaction.AccountID); // 重新获取更新后的余额
                                    Console.WriteLine($"[AddTransaction] 当前余额为0，已设置为初始金额，账户ID={transaction.AccountID}，新余额={updatedBalance}");
                                }
                            }
                        }

                        // 提交事务
                        transactionScope.Commit();
                        Console.WriteLine("[AddTransaction] 事务提交成功");
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transactionScope.Rollback();
                        Console.WriteLine($"添加交易失败: {ex.Message}");
                        return false;
                    }
                }
            }
        }




        //public decimal GetAccountBalance(int accountId)
        //{
        //    string sql = "SELECT CurrentBalance FROM Accounts WHERE AccountID = @AccountID";
        //    var parameter = new MySqlParameter("@AccountID", accountId);
        //    var result = _dbHelper.ExecuteScalar(sql, new[] { parameter });
        //    return Convert.ToDecimal(result);
        //}
        //public decimal GetAccountBalance(int accountId)
        //{
        //    string sql = "SELECT CurrentBalance FROM Accounts WHERE AccountID = @AccountID";
        //    var parameter = new MySqlParameter("@AccountID", accountId);
        //    var result = _dbHelper.ExecuteScalar(sql, new[] { parameter });

        //    decimal balance = result == DBNull.Value ? 0 : Convert.ToDecimal(result);
        //    Console.WriteLine($"[GetAccountBalance] 账户ID={accountId} 当前余额={balance}");
        //    return balance;
        //}

        public decimal GetAccountBalance(int accountId)
        {
            string sql = "SELECT CurrentBalance FROM Accounts WHERE AccountID = @AccountID";
            var parameter = new MySqlParameter("@AccountID", accountId);
            var result = _dbHelper.ExecuteScalar(sql, new[] { parameter });

            decimal balance = result == DBNull.Value ? 0 : Convert.ToDecimal(result);
            Console.WriteLine($"[GetAccountBalance] 账户ID={accountId} 当前余额={balance}");
            return balance;
        }


        //public decimal GetAccountBalance(int accountId)
        //{
        //    string sql = "SELECT CurrentBalance FROM Accounts WHERE AccountID = @AccountID";
        //    var parameter = new MySqlParameter("@AccountID", accountId);
        //    var result = _dbHelper.ExecuteScalar(sql, new[] { parameter });
        //    return Convert.ToDecimal(result);
        //}


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