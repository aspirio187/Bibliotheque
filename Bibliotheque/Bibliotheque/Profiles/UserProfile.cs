using AutoMapper;
using Bibliotheque.EntityFramework.Entities;
using Bibliotheque.EntityFramework.Helpers;
using Bibliotheque.EntityFramework.Services.Authentication;
using Bibliotheque.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserRegistrationModel, UserEntity>()
                .ForMember(
                    dest => dest.NormalizedEmail,
                    opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForMember(
                    dest => dest.Password,
                    opt => opt.MapFrom(
                        src => HashingHelper.HashUsingPbkdf2(src.Password, src.Email)))
                .ForMember(
                    dest => dest.BirthDate,
                    opt => opt.MapFrom(src => src.BirthDate));

            CreateMap<UserConnection, LoginRequest>().ReverseMap();

            CreateMap<UserEntity, UserModel>();

            CreateMap<UserModel, UserEntity>()
                .ForMember(
                    dest => dest.BirthDate,
                    opt => opt.MapFrom(
                        src => src.BirthDate));

            CreateMap<UserEntity, UserAdminModel>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(
                        src => $"{src.FirstName} {src.LastName}"))
                .ForMember(
                    dest => dest.FullAddress,
                    opt => opt.MapFrom(
                        src => $"{src.Address.Street} {src.Address.Appartment} | {src.Address.ZipCode} - {src.Address.City}"))
                .ForMember(
                    dest => dest.Role,
                    opt => opt.MapFrom(
                        src => $"{src.Role.Name}"));
        }
    }
}
