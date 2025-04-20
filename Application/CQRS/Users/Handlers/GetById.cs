using Application.CQRS.Users.ResponseDto;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Users.Handlers;

public class GetById
{
    public class Query : IRequest<Result<GetByIdDto>>
    {
        public int Id { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork) : IRequestHandler<Query, Result<GetByIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<GetByIdDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var currentUser = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);

            if (currentUser == null)
            {
                return new Result<GetByIdDto>() { Errors = ["User not found"], IsSuccess = false };
            }

            GetByIdDto response = new()
            {
                Id = currentUser.Id,
                Firstname = currentUser.Firstname,
                Lastname = currentUser.Lastname,
                Gender = currentUser.Gender,
                Age = currentUser.Age,
                Email = currentUser.Email,
                Password = currentUser.Password,
                Height = currentUser.Height,
                Weight = currentUser.Weight,
                ProfileImageId = currentUser.ProfileImageId
                
            };

            return new Result<GetByIdDto>() { Data = response, Errors = [], IsSuccess = true };
        }
    }
}

