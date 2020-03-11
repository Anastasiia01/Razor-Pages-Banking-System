using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankRPEF.Models;
using BankRPEF.Utils;
using BankRPEF.ServicesBusiness;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankRPEF.Pages
{
    public class XferStoCModel : PageModel
    {
        IBusinessBanking _ibusbank = null;

        public XferStoCModel(IBusinessBanking ibusbank)
        {
            _ibusbank = ibusbank;
        }
        public decimal SavingBalance { get; set; }
        public decimal CheckingBalance { get; set; }

        [BindProperty]
        public decimal TransferAmount { get; set; }

        public string Message { get; set; }

        public IActionResult OnGet()
        {

            if (SessionFacade.USERINFO == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            else
            {
                UserInfo userInfo = SessionFacade.USERINFO;
                SavingBalance = _ibusbank.GetSavingBalance(userInfo.SavingAccountNumber);
                CheckingBalance = _ibusbank.GetCheckingBalance(userInfo.CheckingAccountNumber);
            }
            Message = "";
            TransferAmount = 0;
            return Page();
        }
        public IActionResult OnPost()
        {
            if (SessionFacade.USERINFO == null) // not logged in
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            else
            {
                UserInfo userInfo = SessionFacade.USERINFO;
                bool res = _ibusbank.TransferSavingToChecking(userInfo.SavingAccountNumber, userInfo.CheckingAccountNumber, TransferAmount);
                if (res)
                {
                        CheckingBalance = _ibusbank.GetCheckingBalance(userInfo.CheckingAccountNumber);
                        SavingBalance = _ibusbank.GetSavingBalance(userInfo.SavingAccountNumber);
                        Message = "Transfer succeeded..";
                        // clear history cache
                        string key = String.Format("TRHISTORY_{0}", userInfo.CheckingAccountNumber);
                        CacheAbstractionHelper.CABS.Remove(key);
                }            
            }
            return Page();
        }
    }
}