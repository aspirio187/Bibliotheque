using AutoMapper;
using Bibliotheque.EntityFramework.Entities;
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
            CreateMap<UserForCreationRecord, UserEntity>()
                .ForMember(
                    dest => dest.BirthDate,
                    opt => opt.MapFrom(src => src.BirthDate));
            CreateMap<UserConnectionRecord, LoginRequest>();
        }
    }
}
