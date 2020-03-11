using BankRPEF.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRPEF.ServicesBusiness
{
    public interface IBusinessBanking
    {
        decimal GetCheckingBalance(long checkingAccountNum);
        decimal GetSavingBalance(long savingAccountNum);
        long GetCheckingAccountNumForUser(string username);
        long GetSavingAccountNumForUser(string username);
        bool TransferCheckingToSaving(long checkingAccountNum, long savingAccountNum, decimal amount);
        bool TransferSavingToChecking(long savingAccountNum, long checkingAccountNum, decimal amount);
        bool BillPayment(long checkingAccountNum, long payeeAccountNum, decimal amount);
        List<TransactionHistoryVM> GetTransactionHistory(long checkingAccountNum);
    }
}
