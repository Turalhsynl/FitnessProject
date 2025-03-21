using Application.CQRS.Categories.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Categories.Handlers;

public class UpdateCategory
{
    public record struct UpdateCategoryCommand : IRequest<Result<UpdateCategoryDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        //public List<Product> Products { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateCategoryCommand, Result<UpdateCategoryDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<UpdateCategoryDto>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var currentCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(request.Id);
            if (currentCategory == null) throw new Exception("Category not found or doesn't exist");

            currentCategory.Name = request.Name;
            currentCategory.Description = request.Description;
            currentCategory.ImageUrl = request.ImageUrl;
            currentCategory.UpdatedBy = 1;

            _unitOfWork.CategoryRepository.Update(currentCategory);

            var response = _mapper.Map<UpdateCategoryDto>(currentCategory);

            return new Result<UpdateCategoryDto>
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };

        }
    }
}
