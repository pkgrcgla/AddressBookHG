using AddressBookHG_EL.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_EL.Entities
{
    public class UserForgotPasswordTokens : BaseNumeric<int>
    {
        public string UserId { get; set; }
        public string Url { get; set; }
        public string Token { get; set; }
        public bool IsActive { get; set; }
        public DateTime? TokenUsedDate { get; set; }
        //dateoftokenuse

        [ForeignKey("UserId")]
        public virtual AppUser User { get; set; }

    }
}
