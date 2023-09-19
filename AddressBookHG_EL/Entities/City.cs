using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_EL.Entities
{
    [Table("CITY")]
    public class City : BaseNumeric<byte>
    {

        [StringLength(50)]
        [MinLength(3)]
        public string Name { get; set; }

        [StringLength(2, MinimumLength = 2)]
        //Unique olmalı
        public string PlateCode { get; set; }
        public bool IsDeleted { get; set; }

    }
}
