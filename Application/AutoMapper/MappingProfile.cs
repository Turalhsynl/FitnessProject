using Application.CQRS.Products.ResponseDto;
using Application.CQRS.Users.ResponseDto;
using AutoMapper;
using Domain.Entities;
using static Application.CQRS.Products.Handlers.AddProduct;
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

        CreateMap<Product, GetAllProductDto>();
        CreateMap<Product, GetProductByIdDto>();
        CreateMap<Product, UpdateProductDto>();
        CreateMap<Product, AddProductDto>();
        CreateMap<AddProductCommand, Product>();
    }
}
