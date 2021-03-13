using AutoMapper;
using Bibliotheque.EntityFramework.Services.Authentication;
using Bibliotheque.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Profiles
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<LoginModel, LoginRequest>();
        }
    }
}
