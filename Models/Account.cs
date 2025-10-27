using System;

namespace PersonalFinanceManager.Models
{
    public class Account
    {
        public int AccountID { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public decimal InitialAmount { get; set; }
        public decimal CurrentBalance { get; set; }
        public string Currency { get; set; }
        public string BankName { get; set; }
        public string CardNumber { get; set; }
        public string Description { get; set; }
        public string ColorMarker { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateTime { get; set; }

        public Account()
        {
            Currency = "CNY";
            IsActive = true;
            CreateTime = DateTime.Now;
        }
    }
}