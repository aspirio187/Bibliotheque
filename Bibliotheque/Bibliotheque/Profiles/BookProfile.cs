﻿using AutoMapper;
using Bibliotheque.EntityFramework.Entities;
using Bibliotheque.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bibliotheque.UI.Profiles
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<BookEntity, BookMiniatureModel>()
                .ForMember(
                    dest => dest.Image,
                    opt => opt.MapFrom(
                        src => Path.GetFullPath(src.Preface)))
                .ForMember(
                    dest => dest.Title,
                    opt => opt.MapFrom(
                        src => src.Title))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<BookEntity, BookAdminMiniatureModel>()
                .ForMember(
                    dest => dest.Image,
                    opt => opt.MapFrom(
                        src => Path.GetFullPath(src.Preface)))
                .ForMember(
                    dest => dest.Title,
                    opt => opt.MapFrom(
                        src => src.Title))
                .IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<BookModel, BookEntity>()
                .ForMember(
                    dest => dest.Preface,
                    opt => opt.MapFrom(
                        src => src.PrefacePath));

            CreateMap<BookEntity, BookModel>()
                .ForMember(
                    dest => dest.PrefacePath,
                    opt => opt.MapFrom(
                        src => src.Preface))
                .ForMember(
                    dest => dest.Genres,
                    opt => opt.MapFrom(
                        src => src.BookGenres.Select(bg => bg.Genre)));

            CreateMap<BookCopyEntity, BookStateModel>().ReverseMap();
        }
    }
}
