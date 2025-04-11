using Application.CQRS.Users.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using Common.Security;
using MediatR;
using Repository.Common;
using System.Security;

namespace Application.CQRS.Users.Handlers;

public class Update
{
    public record struct UpdateCommand : IRequest<Result<UpdateDto>>
    {
        public int Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Gender { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Decimal Height { get; set; }
        public Decimal Weight { get; set; }
    }

    public sealed class Handler : IRequestHandler<UpdateCommand, Result<UpdateDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<UpdateDto>> Handle(UpdateCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);
            if (currentUser == null) throw new Exception("User not found");

            var hashPassword = PasswordHasher.ComputeStringToSha256Hash(request.Password);

            currentUser.Firstname = request.Firstname;
            currentUser.Lastname = request.Lastname;
            currentUser.Gender = request.Gender;
            currentUser.Age = request.Age;
            currentUser.Email = request.Email;
            currentUser.Password = hashPassword;
            currentUser.Weight = request.Weight;
            currentUser.Height = request.Height;
            currentUser.UpdatedBy = 1;

            _unitOfWork.UserRepository.Update(currentUser);

            var response = _mapper.Map<UpdateDto>(currentUser);

            return new Result<UpdateDto>
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }
    }
}
