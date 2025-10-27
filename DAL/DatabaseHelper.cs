using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace PersonalFinanceManager.DAL
{
    public class DatabaseHelper
    {
        private readonly string connectionString;

        public DatabaseHelper()
        {
            connectionString = Common.Constants.ConnectionString;
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public bool TestConnection()
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"数据库连接测试失败: {ex.Message}");
            }
        }

        public int ExecuteNonQuery(string sql, MySqlParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return command.ExecuteNonQuery();
                }
            }
        }

        public object ExecuteScalar(string sql, MySqlParameter[] parameters = null)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    return command.ExecuteScalar();
                }
            }
        }

        public DataTable ExecuteQuery(string sql, MySqlParameter[] parameters = null)
        {
            var dataTable = new DataTable();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }
                    using (var adapter = new MySqlDataAdapter(command))
                    {
                        adapter.Fill(dataTable);
                    }
                }
            }
            return dataTable;
        }
    }
}