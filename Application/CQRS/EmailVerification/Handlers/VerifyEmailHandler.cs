using MediatR;
using Repository.Common;

namespace Application.CQRS.EmailVerification.Handlers;

public class VerifyEmailHandler
{
    public class VerifyEmailCodeCommand : IRequest<bool>
    {
        public string Email { get; set; }
        public string Code { get; set; }
    }

    public class VerifyEmailCodeCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<VerifyEmailCodeCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<bool> Handle(VerifyEmailCodeCommand request, CancellationToken cancellationToken)
        {
            var record = await _unitOfWork.EmailVerificationRepository
                .GetValidCodeAsync(request.Email, request.Code);

            if (record == null) return false;

            record.IsUsed = true;
            record.UpdatedDate = DateTime.UtcNow;

            await _unitOfWork.SaveChangeAsync();
            return true;
        }
    }

}
