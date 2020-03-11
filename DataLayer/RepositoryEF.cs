using BankRPEF.Models;
using BankRPEF.Models.ViewModels;
using BankRPEF.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRPEF.DataLayer
{
    public class RepositoryEF : IRepositoryBanking, IRepositoryAuthentication
    {
        MYBANKContext _dbContext = null;
        public RepositoryEF(DbContext dbContext)
        {
            _dbContext = dbContext as MYBANKContext;
        }
        public long GetCheckingAccountNumForUser(string username)
        {
            try
            {
                var chkacctNum = (from rec in _dbContext.CheckingAccounts
                                  where rec.Username == username
                                  select rec.CheckingAccountNumber).FirstOrDefault<long>();
                return chkacctNum;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public long GetSavingAccountNumForUser(string username)
        {
            try
            {
                var savingAcctNum = (from rec in _dbContext.SavingAccounts
                                     where rec.Username == username
                                     select rec.SavingAccountNumber).FirstOrDefault<long>();
                return savingAcctNum;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public decimal GetCheckingBalance(long checkingAccountNum)
        {
            try
            {
                var res = (from rec in _dbContext.CheckingAccounts
                           where rec.CheckingAccountNumber == checkingAccountNum
                           select rec.Balance).FirstOrDefault<decimal>();
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public decimal GetSavingBalance(long savingAccountNum)
        {
            try
            {
                var res = (from rec in _dbContext.SavingAccounts
                           where rec.SavingAccountNumber == savingAccountNum
                           select rec.Balance).FirstOrDefault<decimal>();
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<TransactionHistoryVM> GetTransactionHistory(long checkingAccountNum)
        {
            try
            {
                // check if it exists in cache, if so retrieve it from there
                List<TransactionHistoryVM> THist = null;
                string checkAcctNum = SessionFacade.USERINFO.CheckingAccountNumber.ToString();
                THist = CacheAbstractionHelper.CABS.Retrieve<List<TransactionHistoryVM>>("TRHISTORY_" + checkAcctNum);
                if (THist == null)
                { // obtain it from DB
                    var res = (from rec in _dbContext.TransactionHistories
                               join trtype in _dbContext.TransactionTypes on rec.TransactionTypeId equals
                               trtype.TransactionTypeId
                               where rec.CheckingAccountNumber == checkingAccountNum
                               select new TransactionHistoryVM
                               {
                                   CheckingAccountNumber = rec.CheckingAccountNumber,
                                   SavingAccountNumber = rec.SavingAccountNumber,
                                   Amount = rec.Amount,
                                   TransactionFee = rec.TransactionFee,
                                   TransactionTypeName = trtype.TransactionTypeName,
                                   TransactionDate = rec.TransactionDate
                               }).ToList<TransactionHistoryVM>();
                    THist = res;
                    CacheAbstractionHelper.CABS.Insert("TRHISTORY_" + checkAcctNum, THist);
                }
                return THist;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool TransferCheckingToSaving(long checkingAccountNum, long savingAccountNum, decimal amount, decimal transactionFee)
        {
            bool ret = false;
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var reccheck = (from r in _dbContext.CheckingAccounts
                                    where r.CheckingAccountNumber == checkingAccountNum
                                    select r).FirstOrDefault<CheckingAccounts>();
                    if (reccheck != null)
                        reccheck.Balance = reccheck.Balance - amount;
                    _dbContext.SaveChanges();
                    var bal = (from r in _dbContext.CheckingAccounts
                               where r.CheckingAccountNumber == checkingAccountNum
                               select r.Balance).FirstOrDefault<decimal>();
                    if (bal < 0)
                        throw new Exception("insufficient amount in Checking Account..");
                    var recsav = (from r in _dbContext.SavingAccounts
                                  where r.SavingAccountNumber == savingAccountNum
                                  select r).FirstOrDefault<SavingAccounts>();
                    if (recsav != null)
                        recsav.Balance = recsav.Balance + amount;
                    _dbContext.SaveChanges();
                    TransactionHistories th = new TransactionHistories();
                    th.CheckingAccountNumber = checkingAccountNum;
                    th.SavingAccountNumber = savingAccountNum;
                    th.Amount = amount;
                    th.TransactionTypeId = 100;
                    th.TransactionFee = transactionFee;
                    th.TransactionDate = DateTime.Now;
                    _dbContext.TransactionHistories.Add(th);
                    _dbContext.SaveChanges();
                    dbContextTransaction.Commit();
                    ret = true;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
            return ret;
        }
        public bool TransferSavingToChecking(long savingAccountNum, long checkingAccountNum, decimal amount, decimal transactionFee)
        {
            bool ret = false;
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var recsav = (from r in _dbContext.SavingAccounts
                                    where r.SavingAccountNumber == savingAccountNum
                                    select r).FirstOrDefault<SavingAccounts>();
                    if (recsav != null)
                        recsav.Balance = recsav.Balance - amount;
                    _dbContext.SaveChanges();
                    var bal = (from r in _dbContext.SavingAccounts
                               where r.SavingAccountNumber == savingAccountNum
                               select r.Balance).FirstOrDefault<decimal>();
                    if (bal < 0)
                        throw new Exception("insufficient amount in Saving Account..");
                    var reccheck = (from r in _dbContext.CheckingAccounts
                                  where r.CheckingAccountNumber == checkingAccountNum
                                  select r).FirstOrDefault<CheckingAccounts>();
                    if (reccheck != null)
                        reccheck.Balance = reccheck.Balance + amount;
                    _dbContext.SaveChanges();
                    TransactionHistories th = new TransactionHistories();
                    th.CheckingAccountNumber = checkingAccountNum;
                    th.SavingAccountNumber = savingAccountNum;
                    th.Amount = amount;
                    th.TransactionTypeId = 101;
                    th.TransactionFee = transactionFee;
                    th.TransactionDate = DateTime.Now;
                    _dbContext.TransactionHistories.Add(th);
                    _dbContext.SaveChanges();
                    dbContextTransaction.Commit();
                    ret = true;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
            return ret;
        }
        public UserInfo GetUserInfo(string username)
        {
            UserInfo uinfo = null;
            try
            {
                long chkAcctNum = 0;
                chkAcctNum = (from rec in _dbContext.CheckingAccounts
                              where rec.Username == username
                              select rec.CheckingAccountNumber).FirstOrDefault<long>();
                if (chkAcctNum > 0)
                {
                    long savAcctNum = (from rec in _dbContext.SavingAccounts
                                       where rec.Username == username
                                       select rec.SavingAccountNumber).FirstOrDefault<long>();
                    uinfo = new UserInfo();
                    uinfo.UserName = username;
                    uinfo.CheckingAccountNumber = chkAcctNum;
                    uinfo.SavingAccountNumber = savAcctNum;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return uinfo;
        }

        public bool BillPayment(long checkingAccountNum, long payeeAccountNum, decimal amount, decimal transactionFee)
        {
            bool ret = false;
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    var reccheck = (from r in _dbContext.CheckingAccounts
                                    where r.CheckingAccountNumber == checkingAccountNum
                                    select r).FirstOrDefault<CheckingAccounts>();
                    if (reccheck != null)
                        reccheck.Balance = reccheck.Balance - amount;
                    _dbContext.SaveChanges();
                    var bal = (from r in _dbContext.CheckingAccounts
                               where r.CheckingAccountNumber == checkingAccountNum
                               select r.Balance).FirstOrDefault<decimal>();
                    if (bal < 0)
                        throw new Exception("insufficient amount in Checking Account..");
                    var recpayee = (from r in _dbContext.CheckingAccounts
                                  where r.CheckingAccountNumber == payeeAccountNum
                                  select r).FirstOrDefault<CheckingAccounts>();
                    if (recpayee != null)
                        recpayee.Balance = recpayee.Balance + amount;
                    else
                    {
                        //contact 
                    }
                    _dbContext.SaveChanges();
                    TransactionHistories th = new TransactionHistories();
                    th.CheckingAccountNumber = checkingAccountNum;
                    th.SavingAccountNumber = payeeAccountNum;
                    th.Amount = amount;
                    th.TransactionTypeId = 102;
                    th.TransactionFee = transactionFee;
                    th.TransactionDate = DateTime.Now;
                    _dbContext.TransactionHistories.Add(th);
                    _dbContext.SaveChanges();
                    dbContextTransaction.Commit();
                    ret = true;
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
            return ret;
        }
    }
}

