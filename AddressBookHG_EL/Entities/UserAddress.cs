using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AddressBookHG_EL.IdentityModels;

namespace AddressBookHG_EL.Entities
{
    [Table("USERADDRESS")]
    public class UserAddress : BaseNumeric<int>
    {
        [StringLength(50)]
        [MinLength(2)]
        public string Title { get; set; }

        [StringLength(400)]
        public string Details { get; set; }
        public int NeighborhoodId { get; set; } //FK

        public string UserId { get; set; }
        public bool IsDefaultAddress { get; set; }
        public bool IsDeleted { get; set; }
        [ForeignKey("NeighborhoodId")]
        public virtual Neighborhood Neighborhood { get; set; }

        [ForeignKey("UserId")]
        public virtual AppUser AppUser { get; set; }
    }
}
