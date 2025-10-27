using System;

namespace PersonalFinanceManager.Models
{
    public class Transaction
    {
        public int TransactionID { get; set; }
        public DateTime TransactionTime { get; set; }
        public int AccountID { get; set; }
        public int CategoryID { get; set; }
        public decimal Amount { get; set; }
        public string TransactionType { get; set; } // "收入", "支出", "转账"
        public string TargetAccount { get; set; }
        public string Remark { get; set; }
        public string ImagePath { get; set; }
        public string Location { get; set; }
        public string Tags { get; set; }
        public string Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public string SyncStatus { get; set; }
        public string SerialNumber { get; set; }

        // 导航属性（用于显示）
        public string AccountName { get; set; }
        public string CategoryName { get; set; }

        public Transaction()
        {
            TransactionTime = DateTime.Now;
            CreateTime = DateTime.Now;
            UpdateTime = DateTime.Now;
            Status = "正常";
            SyncStatus = "未同步";
            SerialNumber = GenerateSerialNumber();
        }

        private string GenerateSerialNumber()
        {
            return "T" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
    }
}