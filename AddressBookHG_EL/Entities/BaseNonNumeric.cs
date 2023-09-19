using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_EL.Entities
{
    public class BaseNonNumeric
    {
        [Column(Order = 1)]
        [Key]
        public virtual string Id { get; set; }
        [Column(Order = 2)]
        public virtual DateTime InsertedDate { get; set; }
    }
}
