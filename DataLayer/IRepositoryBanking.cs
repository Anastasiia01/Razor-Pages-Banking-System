using BankRPEF.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRPEF.DataLayer
{
    public interface IRepositoryBanking
    {
        decimal GetCheckingBalance(long checkingAccountNum);
        decimal GetSavingBalance(long savingAccountNum);
        long GetCheckingAccountNumForUser(string username);
        long GetSavingAccountNumForUser(string username);
        bool TransferCheckingToSaving(long checkingAccountNum, long savingAccountNum, decimal amount, decimal transactionFee);
        bool TransferSavingToChecking(long savingAccountNum, long checkingAccountNum, decimal amount, decimal transactionFee);
        bool BillPayment(long checkingAccountNum, long payeeAccountNum, decimal amount, decimal transactionFee);
        List<TransactionHistoryVM> GetTransactionHistory(long checkingAccountNum);
    }
}
