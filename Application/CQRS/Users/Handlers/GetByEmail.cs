using Application.CQRS.Users.ResponseDto;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Users.Handlers;

public class GetByEmail
{
    public class EmailQuery : IRequest<Result<GetByEmailDto>>
    {
        public string Email { get; set; }
    }

    public sealed class Handler : IRequestHandler<EmailQuery, Result<GetByEmailDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<GetByEmailDto>> Handle(EmailQuery request, CancellationToken cancellationToken)
        {
            var currentUser = await _unitOfWork.UserRepository.GetUserByEmailAsync(request.Email);

            if (currentUser == null)
            {
                return new Result<GetByEmailDto>() { Errors = ["User not found"], IsSuccess = false };
            }

            GetByEmailDto response = new()
            {
                Id = currentUser.Id,
                Firstname = currentUser.Firstname,
                Lastname = currentUser.Lastname,
                Gender = currentUser.Gender,
                Age = currentUser.Age,
                Email = currentUser.Email
            };

            return new Result<GetByEmailDto>() { Data = response, Errors = [], IsSuccess = true };
        }
    }
}
