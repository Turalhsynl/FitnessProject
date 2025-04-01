
using Application.CQRS.Products.ResponseDto;
using static Application.CQRS.Products.Handlers.AddProduct;
using Application.CQRS.Categories.Handlers;
using Application.CQRS.Categories.ResponseDto;
using Application.CQRS.Users.ResponseDto;
using AutoMapper;
using Domain.Entities;
using static Application.CQRS.Categories.Handlers.Add;
using static Application.CQRS.Users.Handlers.Register;
using Application.CQRS.Carts.ResponseDto;
using System.Drawing;

namespace Application.AutoMapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterCommand, User>().ReverseMap();
        CreateMap<User, RegisterDto>();
        CreateMap<User, GetAllDto>();
        CreateMap<User, UpdateDto>();
        CreateMap<Product, GetAllProductDto>()
     .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color.ToString()));  // Converts enum to string

        CreateMap<Product, GetProductByIdDto>();
        CreateMap<Product, UpdateProductDto>();
        CreateMap<Product, AddProductDto>();
        CreateMap<AddProductCommand, Product>();
        CreateMap<AddCommand, Category>();
        CreateMap<Category, AddDto>();
        CreateMap<Category, GetAllCategoryDto>();
        CreateMap<Category, UpdateCategoryDto>();
        CreateMap<Cart, CartDto>()
                .ForMember(dest => dest.CartLines, opt => opt.MapFrom(src => src.CartLines));
        CreateMap<CartLine, CartLineDto>();
    }
}
