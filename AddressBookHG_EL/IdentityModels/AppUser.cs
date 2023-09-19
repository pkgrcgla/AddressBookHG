using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_EL.IdentityModels
{
    public class AppUser : IdentityUser
    {
        [StringLength(50)]
        [MinLength(2)]
        public string Name { get; set; }
        [StringLength(50, MinimumLength = 2)]
        public string Surname { get; set; }
        public DateTime? BirthDate { get; set; }
        //Gender
        //Height

    }
}
