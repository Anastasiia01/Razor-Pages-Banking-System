using System;
using System.Collections.Generic;

namespace BankRPEF.Models
{
    public partial class TransactionHistories
    {
        public long TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public long CheckingAccountNumber { get; set; }
        public long SavingAccountNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal TransactionFee { get; set; }
        public int TransactionTypeId { get; set; }
    }
}
