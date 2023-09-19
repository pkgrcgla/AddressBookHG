using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_EL.Entities
{
    [Table("DISTRICT")]
    public class District : BaseNumeric<int>
    {
        [StringLength(50)]
        [MinLength(2)]
        public string Name { get; set; }

        public byte CityId { get; set; } //FK

        public bool IsDeleted { get; set; }

        [ForeignKey("CityId")]
        public virtual City City { get; set; }
    }
}
