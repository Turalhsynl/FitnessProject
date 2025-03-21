using Application.CQRS.Categories.Handlers;
using Application.CQRS.Categories.ResponseDto;
using Application.CQRS.Users.ResponseDto;
using AutoMapper;
using Domain.Entities;
using static Application.CQRS.Categories.Handlers.Add;
using static Application.CQRS.Users.Handlers.Register;

namespace Application.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterCommand, User>().ReverseMap();
        CreateMap<User, RegisterDto>();
        CreateMap<User, GetAllDto>();
        CreateMap<User, UpdateDto>();
        CreateMap<AddCommand, Category>();
        CreateMap<Category, AddDto>();
        CreateMap<Category, GetAllCategoryDto>();
        CreateMap<Category, UpdateCategoryDto>();
    }
}
