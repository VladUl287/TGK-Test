using AutoMapper;
using TestApi.Database.Models;
using TestApi.Dtos;

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