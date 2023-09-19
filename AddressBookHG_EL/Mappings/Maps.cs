using AddressBookHG_EL.Entities;
using AddressBookHG_EL.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBookHG_EL.Mappings
{
    public class Maps : Profile
    {
        //BL katmanında _mapper dönüşüm yapabilsin diye buraya maps içine
        //kimin kime dönüşeceğini yazdık
        public Maps()
        {
            CreateMap<City, CityDTO>().ReverseMap();
            CreateMap<District, DistrictDTO>().ReverseMap();
            CreateMap<Neighborhood, NeighborhoodDTO>().ReverseMap();
            CreateMap<UserAddress, UserAddressDTO>().ReverseMap();
            //CreateMap<UserForgotPasswordsHistorical, UserForgotPasswordsHistoricalDTO>().ReverseMap();
            //CreateMap<UserForgotPasswordTokens, UserForgotPasswordTokensDTO>().ReverseMap();
        }
    }
}
