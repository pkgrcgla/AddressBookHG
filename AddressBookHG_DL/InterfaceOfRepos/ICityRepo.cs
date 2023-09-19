using AddressBookHG_DL.ImplementationsOfRepos;
using AddressBookHG_EL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_DL.InterfaceOfRepos
{
    public interface ICityRepo : IRepository<City, byte>
    {

    }
}
