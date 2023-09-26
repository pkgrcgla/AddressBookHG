using AddressBookHG_EL.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_EL.Entities
{
    public class UserForgotPasswordsHistorical : BaseNumeric<int>
    {
        public string UserId { get; set; }
        public string Password { get; set; } // şimdilik hashlemeyeceğim! Sonra bu hashlenme mekanizması düşünülmeli...
        public bool IsDeleted { get; set; }


        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

    }
}
