using Application.CQRS.Categories.ResponseDto;
using Application.Security;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

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
            var categories = _unitOfWork.CategoryRepository.GetAll();

            if (categories == null || !categories.Any())
            {
                return new Result<List<GetAllCategoryDto>>
                {
                    Data = [],
                    Errors = ["Category doesn't exist"],
                    IsSuccess = false,
                };
            }

            var response = _mapper.Map<List<GetAllCategoryDto>>(categories);

            return new Result<List<GetAllCategoryDto>>
            {
                Data = response,
                Errors = [],
                IsSuccess = true,
            };
        }
    }
}
