using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BankRPEF.Models.ViewModels
{
    public class TransactionHistoryVM : TransactionHistories
    {
        public string TransactionTypeName { get; set; } // added field

    }
}
