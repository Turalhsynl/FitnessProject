using Application.CQRS.Categories.ResponseDto;
using Application.CQRS.Products.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Categories.Handlers
{
    public class GetById
    {
        public class GetByIdCategoryQuery : IRequest<Result<GetByIdDto>>
        {
            public int Id { get; set; }
        }

        public sealed class Handler : IRequestHandler<GetByIdCategoryQuery, Result<GetByIdDto>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public Handler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<GetByIdDto>> Handle(GetByIdCategoryQuery request, CancellationToken cancellationToken)
            {
                var currentCategory = await _unitOfWork.CategoryRepository
                    .GetAll()
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.Id == request.Id);

                if (currentCategory == null)
                {
                    return new Result<GetByIdDto>() { Errors = new List<string> { "Category not found" }, IsSuccess = false };
                }

                GetByIdDto response = new()
                {
                    Id = currentCategory.Id,
                    Name = currentCategory.Name,
                    Description = currentCategory.Description,
                    ImageUrl = currentCategory.ImageUrl,
                    Products = currentCategory.Products.Select(p => new GetAllProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Quantity = p.Quantity,
                        Description = p.Description,
                        Price = p.Price,
                        ImageUrl = p.ImageUrl,
                        Color = p.Color,
                        CategoryId = p.CategoryId
                    }).ToList()
                };

                return new Result<GetByIdDto>() { Data = response, Errors = new List<string>(), IsSuccess = true };
            }
        }
    }
}