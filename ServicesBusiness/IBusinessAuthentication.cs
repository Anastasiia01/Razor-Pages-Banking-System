using BankRPEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRPEF.ServicesBusiness
{
    public interface IBusinessAuthentication
    {
        UserInfo GetUserInfo(string username);
    }
}
