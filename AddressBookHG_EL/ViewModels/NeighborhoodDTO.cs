﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_EL.ViewModels
{
    public class NeighborhoodDTO
    {
        public int Id { get; set; }
        public DateTime InsertedDate { get; set; }
        [StringLength(50, ErrorMessage = "Mahalle adı mak 50 karakter olmalıdır!")]
        [MinLength(2, ErrorMessage = "Mahalle adı min 2 karakter olmalıdır!")]
        public string Name { get; set; }
        public int DistrictId { get; set; } //FK
        public bool IsDeleted { get; set; }
        public DistrictDTO? District { get; set; }
    }
}
