using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRPEF.Models
{
    public class UserInfo
    {
        public long CheckingAccountNumber { get; set; }
        public long SavingAccountNumber { get; set; }
        public string UserName { get; set; }
    }
}
