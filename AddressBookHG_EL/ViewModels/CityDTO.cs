﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_EL.ViewModels
{
    public class CityDTO // ViewModel
    {
        public byte Id { get; set; }
        public DateTime InsertedDate { get; set; }
        [StringLength(50, ErrorMessage = "İl adı mak 50 karakter olmalıdır!")]
        [MinLength(3, ErrorMessage = "İl adı min 3 karakter olmalıdır!")]
        public string Name { get; set; }

        [StringLength(2, MinimumLength = 2, ErrorMessage = ("Plakalar 2 haneli olmak zorundadır!"))]

        public string PlateCode { get; set; }
        public bool IsDeleted { get; set; }
    }
}
