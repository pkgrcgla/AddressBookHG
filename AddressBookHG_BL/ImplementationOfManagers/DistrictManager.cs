using AddressBookHG_BL.InterfacesOfManagers;
using AddressBookHG_DL.InterfaceOfRepos;
using AddressBookHG_EL.Entities;
using AddressBookHG_EL.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_BL.ImplementationOfManagers
{
    public class DistrictManager : Manager<DistrictDTO, District, int>, IDistrictManager
    {
        public DistrictManager(IDistrictRepo repo, IMapper mapper) : base(repo, mapper, new string[] { "City" })
        {

        }
    }
}
