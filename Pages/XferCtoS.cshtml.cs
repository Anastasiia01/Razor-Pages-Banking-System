using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankRPEF.Models;
using BankRPEF.ServicesBusiness;
using BankRPEF.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankRPEF.Pages
{
    public class XferCToSModel : PageModel
    {
        IBusinessBanking _ibusbank = null;
        public XferCToSModel(IBusinessBanking ibusbank)
        {
            _ibusbank = ibusbank;
        }
        public decimal CheckingBalance { get; set; }
        public decimal SavingBalance { get; set; }
        [BindProperty]
        public decimal TransferAmount { get; set; }
        public string Message { get; set; }
        public IActionResult OnGet()
        {
            if (SessionFacade.USERINFO == null) // not logged in
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            else
            {
                UserInfo uinfo = SessionFacade.USERINFO;
                CheckingBalance = _ibusbank.GetCheckingBalance(uinfo.CheckingAccountNumber);
                SavingBalance = _ibusbank.GetSavingBalance(uinfo.SavingAccountNumber);
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
                UserInfo uinfo = SessionFacade.USERINFO;
                bool ret = _ibusbank.TransferCheckingToSaving(uinfo.CheckingAccountNumber, uinfo.SavingAccountNumber, TransferAmount);
                if (ret == true)
                {
                    CheckingBalance = _ibusbank.GetCheckingBalance(uinfo.CheckingAccountNumber);
                    SavingBalance = _ibusbank.GetSavingBalance(uinfo.SavingAccountNumber);
                    Message = "Transfer succeeded..";
                    // clear history cache
                    string key = String.Format("TRHISTORY_{0}", uinfo.CheckingAccountNumber);
                    CacheAbstractionHelper.CABS.Remove(key);
                }
            }
            return Page();
        }
    }
}