using System;
using System.Collections.Generic;

namespace BankRPEF.Models
{
    public partial class CheckingAccounts
    {
        public string Username { get; set; }
        public long CheckingAccountNumber { get; set; }
        public decimal Balance { get; set; }
    }
}
