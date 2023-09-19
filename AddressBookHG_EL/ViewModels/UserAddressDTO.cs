using AddressBookHG_EL.IdentityModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_EL.ViewModels
{
    public class UserAddressDTO
    {
        public int Id { get; set; }
        public DateTime InsertedDate { get; set; }

        [StringLength(50, ErrorMessage = "Adres başlığı mak 50 karakter olmalıdır!")]
        [MinLength(2, ErrorMessage = "Adres başlığı min 2 karakter olmalıdır!")]
        public string Title { get; set; }

        [StringLength(400, ErrorMessage = "Adres detayı mak 400 karakter olmalıdır!")]
        public string Details { get; set; }
        public int NeighborhoodId { get; set; } //FK

        public string UserId { get; set; }
        public bool IsDefaultAddress { get; set; }
        public bool IsDeleted { get; set; }
        public NeighborhoodDTO? Neighborhood { get; set; }
        public AppUser? AppUser { get; set; }
    }
}
