using AddressBookHG_DL.ContextInfo;
using AddressBookHG_DL.InterfaceOfRepos;
using AddressBookHG_EL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_DL.ImplementationsOfRepos
{
    public class UserForgotPasswordTokensRepo :
       Repository<UserForgotPasswordTokens, int>, IUserForgotPasswordTokensRepo
    {
        public UserForgotPasswordTokensRepo(AddressbookContext context) : base(context)
        {

        }
    }
}
