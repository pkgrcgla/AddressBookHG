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
    public class UserAddressRepo : Repository<UserAddress, int>, IUserAddressRepo
    {
        public UserAddressRepo(AddressbookContext context) : base(context)
        {

        }
    }
}
