using Common.GlobalResponses.Generics;
using Common.Security;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Users.Handlers;

public class UpdatePassword
{
    public record struct UpdatePasswordCommand : IRequest<Result<string>>
    {
        public int UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public sealed class Handler : IRequestHandler<UpdatePasswordCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new Exception("User not found");

            var isValid = PasswordHasher.VerifyPassword(request.CurrentPassword, user.Password);
            if (!isValid)
            {
                return new Result<string>
                {
                    Data = null,
                    Errors = ["Current password is incorrect"],
                    IsSuccess = false
                };
            }

            user.Password = PasswordHasher.ComputeStringToSha256Hash(request.NewPassword);
            user.UpdatedBy = 1;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangeAsync();

            return new Result<string>
            {
                Data = "Password updated successfully",
                Errors = [],
                IsSuccess = true
            };
        }
    }
}
