using BankRPEF.DataLayer;
using BankRPEF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankRPEF.ServicesBusiness
{
    public class BusinessAuthentication : IBusinessAuthentication
    {
        IRepositoryAuthentication _irepauth = null;
        public BusinessAuthentication(IRepositoryAuthentication irepauth)
        {
            _irepauth = irepauth;
        }
        public BusinessAuthentication(MYBANKContext context) : this(new RepositoryEF(context))
        { }
        public UserInfo GetUserInfo(string username)
        {
            return _irepauth.GetUserInfo(username);
        }
    }
}
