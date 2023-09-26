﻿using AddressBookHG_EL.IdentityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_EL.ViewModels
{
    public class UserForgotPasswordsHistoricalDTO
    {
        public int Id { get; set; }
        public DateTime InsertedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; } // şimdilik hashlemeyeceğim! Sonra bu hashlenme mekanizması düşünülmeli...

        public AppUser? User { get; set; }

    }
}
