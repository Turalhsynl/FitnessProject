using Application.CQRS.Categories.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using Microsoft.EntityFrameworkCore;
using Application.CQRS.Products.ResponseDto;
using Application.Security;

namespace Application.CQRS.Categories.Handlers
{
    public record struct GetAllCategoryQuery : IRequest<Result<List<GetAllCategoryDto>>> { }

    public sealed class Handler : IRequestHandler<GetAllCategoryQuery, Result<List<GetAllCategoryDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper, IUserContext userContext)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userContext = userContext;
        }

        public async Task<Result<List<GetAllCategoryDto>>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            var categories = await _unitOfWork.CategoryRepository
               
                .GetAll()
                 .Where(x => !x.IsDeleted)
                .Include(c => c.Products)
                .ToListAsync();

            if (categories == null || !categories.Any())
            {
                return new Result<List<GetAllCategoryDto>>
                {
                    Data = new List<GetAllCategoryDto>(),
                    Errors = new List<string> { "Category doesn't exist" },
                    IsSuccess = false,
                };
            }

            var response = _mapper.Map<List<GetAllCategoryDto>>(categories);

            foreach (var category in response)
            {
                category.Products = category.Products.Select(p => new GetAllProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Quantity = p.Quantity,
                    Description = p.Description,
                    Price = p.Price,
                    ImageUrl = p.ImageUrl,
                    Color = p.Color,
                    CategoryId = p.CategoryId
                }).ToList();
            }

            return new Result<List<GetAllCategoryDto>>
            {
                Data = response,
                Errors = new List<string>(),
                IsSuccess = true,
            };
        }
    }
}

