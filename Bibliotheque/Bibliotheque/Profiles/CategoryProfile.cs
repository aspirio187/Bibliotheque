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
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryEntity, CategoryModel>().ReverseMap();
            CreateMap<CategoryToCreate, CategoryEntity>();
        }
    }
}
