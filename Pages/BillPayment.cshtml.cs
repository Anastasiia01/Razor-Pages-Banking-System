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
    public class BillPaymentModel : PageModel
    {
        IBusinessBanking _ibusbank = null;

        public BillPaymentModel(IBusinessBanking ibusbank)
        {
            _ibusbank = ibusbank;
        }
        public decimal CheckingBalance { get; set; }

        [BindProperty]
        public decimal PaymentAmount { get; set; }

        [BindProperty]
        public long PayeeAccountNumber { get; set; }
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
                CheckingBalance = _ibusbank.GetCheckingBalance(userInfo.CheckingAccountNumber);
            }
            Message = "";
            PaymentAmount = 0;
            return Page();
        }
        public IActionResult OnPost()
        {
            if (SessionFacade.USERINFO == null) // not logged in
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            else
            {
                UserInfo uinfo = SessionFacade.USERINFO;
                bool ret = _ibusbank.BillPayment(uinfo.CheckingAccountNumber, PayeeAccountNumber, PaymentAmount);
                if (ret == true)
                {
                    CheckingBalance = _ibusbank.GetCheckingBalance(uinfo.CheckingAccountNumber);
                    Message = "Transaction is in the progress...Might take up to 2 days..";
                    // clear history cache
                    string key = String.Format("TRHISTORY_{0}", uinfo.CheckingAccountNumber);
                    CacheAbstractionHelper.CABS.Remove(key);
                }
            }
            return Page();
        }
    }
}