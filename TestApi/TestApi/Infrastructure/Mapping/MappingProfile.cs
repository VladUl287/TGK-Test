using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApi.Database.Models;
using TestApi.ViewModels;

namespace TestApi.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PersonalAccount, AccountModel>().ReverseMap();
        }
    }
}