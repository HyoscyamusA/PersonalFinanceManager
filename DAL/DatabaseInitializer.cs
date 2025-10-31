using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PersonalFinanceManager.DAL
{
    public class DatabaseInitializer
    {
        private readonly DatabaseHelper _dbHelper;
        // 添加一个静态属性来控制是否显示调试信息
        public static bool ShowDebugMessages { get; set; } = false;

      

        public DatabaseInitializer()
        {
            _dbHelper = new DatabaseHelper();
        }

        public void InitializeDatabase()
        {
            try
            {
                if (ShowDebugMessages)
                {
                    MessageBox.Show("开始数据库初始化...", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 测试连接
                    MessageBox.Show("正在测试数据库连接...", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _dbHelper.TestConnection();
                    MessageBox.Show("数据库连接测试成功！", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 创建表
                    MessageBox.Show("正在创建数据表...", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CreateTables();
                    MessageBox.Show("数据表创建成功！", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 插入初始数据
                    MessageBox.Show("正在插入初始数据...", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    InsertInitialData();
                    MessageBox.Show("初始数据插入成功！", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    MessageBox.Show("MySQL数据库初始化成功！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                string detailedError = $"数据库初始化失败!\n\n错误详情:\n{ex.Message}\n\n堆栈跟踪:\n{ex.StackTrace}";

                if (ex.InnerException != null)
                {
                    detailedError += $"\n\n内部异常:\n{ex.InnerException.Message}";
                }

                MessageBox.Show(detailedError, "错误详情", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateTables()
        {
            try
            {
                // 创建账户表
                string createAccountTable = @"
                    CREATE TABLE IF NOT EXISTS Accounts (
                        AccountID INT AUTO_INCREMENT PRIMARY KEY,
                        AccountName VARCHAR(50) NOT NULL,
                        AccountType VARCHAR(20) NOT NULL,
                        InitialAmount DECIMAL(18,2) DEFAULT 0,
                        CurrentBalance DECIMAL(18,2) DEFAULT 0,
                        Currency VARCHAR(10) DEFAULT 'CNY',
                        BankName VARCHAR(100),
                        CardNumber VARCHAR(50),
                        Description TEXT,
                        ColorMarker VARCHAR(20),
                        IsActive BOOLEAN DEFAULT TRUE,
                        CreateTime DATETIME DEFAULT CURRENT_TIMESTAMP
                    )";

                MessageBox.Show("正在创建 Accounts 表...", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _dbHelper.ExecuteNonQuery(createAccountTable);
                MessageBox.Show("Accounts 表创建成功！", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 创建类别表
                string createCategoryTable = @"
                    CREATE TABLE IF NOT EXISTS Categories (
                        CategoryID INT AUTO_INCREMENT PRIMARY KEY,
                        CategoryName VARCHAR(50) NOT NULL,
                        CategoryType VARCHAR(10) NOT NULL,
                        ParentCategoryID INT NULL,
                        Icon VARCHAR(100),
                        Color VARCHAR(20),
                        BudgetAmount DECIMAL(18,2) DEFAULT 0,
                        ActualAmount DECIMAL(18,2) DEFAULT 0,
                        SortOrder INT DEFAULT 0,
                        IsSystemPreset BOOLEAN DEFAULT FALSE,
                        Description TEXT
                    )";

                MessageBox.Show("正在创建 Categories 表...", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _dbHelper.ExecuteNonQuery(createCategoryTable);
                MessageBox.Show("Categories 表创建成功！", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 创建交易记录表
                string createTransactionTable = @"
                    CREATE TABLE IF NOT EXISTS Transactions (
                        TransactionID INT AUTO_INCREMENT PRIMARY KEY,
                        TransactionTime DATETIME NOT NULL,
                        AccountID INT NOT NULL,
                        CategoryID INT NOT NULL,
                        Amount DECIMAL(18,2) NOT NULL,
                        TransactionType VARCHAR(10) NOT NULL,
                        TargetAccount VARCHAR(100),
                        Remark TEXT,
                        ImagePath VARCHAR(200),
                        Location VARCHAR(100),
                        Tags VARCHAR(200),
                        Status VARCHAR(20) DEFAULT '正常',
                        CreateTime DATETIME DEFAULT CURRENT_TIMESTAMP,
                        UpdateTime DATETIME DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
                        SyncStatus VARCHAR(20) DEFAULT '未同步',
                        SerialNumber VARCHAR(50),
                        FOREIGN KEY (AccountID) REFERENCES Accounts(AccountID),
                        FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
                    )";

                MessageBox.Show("正在创建 Transactions 表...", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _dbHelper.ExecuteNonQuery(createTransactionTable);
                MessageBox.Show("Transactions 表创建成功！", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // 创建预算表
                string createBudgetTable = @"
                    CREATE TABLE IF NOT EXISTS Budgets (
                        BudgetID INT AUTO_INCREMENT PRIMARY KEY,
                        BudgetYear INT NOT NULL,
                        BudgetMonth INT NOT NULL,
                        CategoryID INT NOT NULL,
                        BudgetAmount DECIMAL(18,2) NOT NULL,
                        ActualAmount DECIMAL(18,2) DEFAULT 0,
                        CompletionRate DECIMAL(5,2) DEFAULT 0,
                        WarningThreshold DECIMAL(5,2) DEFAULT 80,
                        ReminderMethod VARCHAR(20) DEFAULT '应用内',
                        Status VARCHAR(20) DEFAULT '激活',
                        CreatedBy VARCHAR(50),
                        CreateTime DATETIME DEFAULT CURRENT_TIMESTAMP,
                        Remark TEXT,
                        FOREIGN KEY (CategoryID) REFERENCES Categories(CategoryID)
                    )";

                MessageBox.Show("正在创建 Budgets 表...", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _dbHelper.ExecuteNonQuery(createBudgetTable);
                MessageBox.Show("Budgets 表创建成功！", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                throw new Exception($"创建数据表失败: {ex.Message}", ex);
            }
        }

        private void InsertInitialData()
        {
            try
            {
                // 检查是否已存在系统预设类别
                string checkCategories = "SELECT COUNT(*) FROM Categories WHERE IsSystemPreset = TRUE";
                var count = _dbHelper.ExecuteScalar(checkCategories);

                if (Convert.ToInt32(count) == 0)
                {
                    MessageBox.Show("正在插入初始类别数据...", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // 插入系统预设类别
                    string insertCategories = @"
                        INSERT INTO Categories (CategoryName, CategoryType, SortOrder, IsSystemPreset, Color) VALUES
                        ('工资收入', '收入', 1, TRUE, '#4CAF50'),
                        ('投资收益', '收入', 2, TRUE, '#8BC34A'),
                        ('兼职收入', '收入', 3, TRUE, '#CDDC39'),
                        ('其他收入', '收入', 4, TRUE, '#FFEB3B'),
                        ('餐饮美食', '支出', 1, TRUE, '#FF5722'),
                        ('交通出行', '支出', 2, TRUE, '#FF9800'),
                        ('购物消费', '支出', 3, TRUE, '#FFC107'),
                        ('住房房租', '支出', 4, TRUE, '#795548'),
                        ('娱乐休闲', '支出', 5, TRUE, '#9C27B0'),
                        ('医疗健康', '支出', 6, TRUE, '#F44336'),
                        ('教育培训', '支出', 7, TRUE, '#2196F3'),
                        ('其他支出', '支出', 8, TRUE, '#9E9E9E')";

                    _dbHelper.ExecuteNonQuery(insertCategories);
                    MessageBox.Show("初始类别数据插入成功！", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("初始类别数据已存在，跳过插入。", "调试信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"插入初始数据失败: {ex.Message}", ex);
            }
        }
    }
}