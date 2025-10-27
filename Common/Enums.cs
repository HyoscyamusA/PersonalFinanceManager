namespace PersonalFinanceManager.Common
{
    public static class Enums
    {
        public enum AccountType
        {
            现金,
            银行卡,
            信用卡,
            支付宝,
            微信,
            其他
        }

        public enum TransactionType
        {
            收入,
            支出,
            转账
        }

        public enum BudgetStatus
        {
            激活,
            暂停,
            完成
        }
    }
}