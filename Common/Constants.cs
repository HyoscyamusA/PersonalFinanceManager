namespace PersonalFinanceManager.Common
{
    public static class Constants
    {
        // MySQL连接字符串 - 请根据你的MySQL配置修改
        public const string ConnectionString = "Server=localhost;Database=PersonalFinance;Uid=root;Pwd=root;Port=3306;Charset=utf8;";

        public static class Colors
        {
            public static readonly string[] CategoryColors = {
                "#FF6B6B", "#4ECDC4", "#45B7D1", "#96CEB4", "#FFEAA7", "#DDA0DD",
                "#98D8C8", "#F7DC6F", "#BB8FCE", "#85C1E9", "#F8C471", "#82E0AA"
            };

            public static readonly string[] AccountColors = {
                "#3498DB", "#2ECC71", "#E74C3C", "#F39C12", "#9B59B6", "#1ABC9C",
                "#34495E", "#16A085", "#27AE60", "#2980B9"
            };
        }

        public static class AccountTypes
        {
            public static readonly string[] Types = {
                "现金", "银行卡", "信用卡", "支付宝", "微信", "其他"
            };
        }

        public static class TransactionTypes
        {
            public const string Income = "收入";
            public const string Expense = "支出";
            public const string Transfer = "转账";
        }
    }
}