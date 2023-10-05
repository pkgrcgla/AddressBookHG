using AddressBookHG_EL.IdentityModels;

namespace AddressBookHG_PL.Models
{
        public class ProfileViewModel : AppUser
        {
            //resim
            public IFormFile? ChosenPicture { get; set; }

            public string? ProfilePicture { get; set; }

        }
    
}
