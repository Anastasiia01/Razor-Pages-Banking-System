using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankRPEF.Models;
using BankRPEF.Models.ViewModels;
using BankRPEF.ServicesBusiness;
using BankRPEF.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankRPEF.Pages
{
    public class TransactionHistoryModel : PageModel
    {
        IBusinessBanking _ibusbank = null;
        public TransactionHistoryModel(IBusinessBanking ibusbank)
        {
            _ibusbank = ibusbank;
        }
        public List<TransactionHistoryVM> TList { get; set; }
        public IActionResult OnGet()
        {
            if (SessionFacade.USERINFO == null) // not logged in
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            else
            {
                UserInfo uinfo = SessionFacade.USERINFO;
                TList = _ibusbank.GetTransactionHistory(uinfo.CheckingAccountNumber);
            }
            return Page();
        }
    }
}