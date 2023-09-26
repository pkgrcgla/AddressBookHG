using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_EL.Entities
{
    [Table("NEIGHBORHOOD")]
    public class Neighborhood : BaseNumeric<int>
    {
        [StringLength(100)]
        [MinLength(2)]
        public string Name { get; set; }

        public int DistrictId { get; set; } //FK

        public bool IsDeleted { get; set; }

        [ForeignKey("DistrictId")]
        public virtual District District { get; set; }
    }
}
