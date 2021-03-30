using AutoMapper;
using Bibliotheque.EntityFramework.Entities;
using Bibliotheque.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Profiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile()
        {
            CreateMap<AddressEntity, AddressRecord>()
                .ForMember(
                    dest => dest.FullAddress,
                    opt => opt.MapFrom(src =>
                        string.IsNullOrEmpty(src.Appartment) ? $"{src.Street}" : $"{src.Street} - {src.Appartment}"))
                .ForMember(
                    dest => dest.FullCity,
                    opt => opt.MapFrom(src =>
                        $"{src.ZipCode} - {src.City}"));

            CreateMap<AddressModel, AddressEntity>().ReverseMap();
        }
    }
}
