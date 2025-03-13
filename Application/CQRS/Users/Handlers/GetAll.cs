using Application.CQRS.Users.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Users.Handlers;

public class GetAll
{
    public record struct GetAllUsersQuery : IRequest<Result<List<GetAllDto>>> { }

    public sealed class Handler : IRequestHandler<GetAllUsersQuery, Result<List<GetAllDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllDto>>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = _unitOfWork.UserRepository.GetAll();
            if (users == null || !users.Any())
                return new Result<List<GetAllDto>>
                {
                    Data = [],
                    Errors = ["No users found"],
                    IsSuccess = false
                };

            var response = _mapper.Map<List<GetAllDto>>(users);

            return new Result<List<GetAllDto>>
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }
    }
}
