using Application.CQRS.Categories.ResponseDto;
using Application.Security;
using AutoMapper;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Categories.Handlers;
public class Add
{
    public class AddCommand : IRequest<Result<AddDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper, IUserContext userContext) : IRequestHandler<AddCommand, Result<AddDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IUserContext _userContext = userContext;

        public async Task<Result<AddDto>> Handle(AddCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "Request cannot be null");
            }

            var newCategory = _mapper.Map<Category>(request);
            newCategory.CreatedBy = _userContext.MustGetUserId();

            if (string.IsNullOrEmpty(newCategory.Name))
            {
                throw new Exception("Category name is required");
            }

            await _unitOfWork.CategoryRepository.AddAsync(newCategory);
            await _unitOfWork.SaveChangeAsync();

            var response = _mapper.Map<AddDto>(newCategory);

            return new Result<AddDto>
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }
    }

}
