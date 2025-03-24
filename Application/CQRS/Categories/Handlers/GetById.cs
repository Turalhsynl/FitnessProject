using Application.CQRS.Categories.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Categories.Handlers;

public class GetById
{
    public class GetByIdCategoryQuery : IRequest<Result<GetByIdDto>>
    {
        public int Id { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<GetByIdCategoryQuery, Result<GetByIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<GetByIdDto>> Handle(GetByIdCategoryQuery request, CancellationToken cancellationToken)
        {
            var currentCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(request.Id);

            if (currentCategory == null)
            {
                return new Result<GetByIdDto>() { Errors = ["Category not found"], IsSuccess = false };
            }

            GetByIdDto response = new()
            {
                Id = currentCategory.Id,
                Name = currentCategory.Name,
                Description = currentCategory.Description,
                ImageUrl = currentCategory.ImageUrl
            };

            return new Result<GetByIdDto>() { Data = response, Errors = [], IsSuccess = true };
        }
    }
}