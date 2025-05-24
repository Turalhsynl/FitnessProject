using Application.Abstractions;
using MediatR;
using Repository.Common;

namespace Application.CQRS.EmailVerification.Handlers;

public class EmailVerificationHandler
{
    public class GenerateEmailCodeCommand : IRequest
    {
        public string Email { get; set; }
    }

    public class GenerateEmailCodeCommandHandler : IRequestHandler<GenerateEmailCodeCommand>
    {
        private readonly IEmailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public GenerateEmailCodeCommandHandler(IEmailService emailService, IUnitOfWork unitOfWork)
        {
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(GenerateEmailCodeCommand request, CancellationToken cancellationToken)
        {
            var code = new Random().Next(100000, 999999).ToString();

            var verification = new Domain.Entities.EmailVerification
            {
                Email = request.Email,
                Code = code,
                ExpireAt = DateTime.UtcNow.AddMinutes(5),
                CreatedDate = DateTime.UtcNow
            };

            await _unitOfWork.EmailVerificationRepository.AddAsync(verification);
            await _unitOfWork.SaveChangeAsync();

            string body = $@"
            <div style='font-family: Arial, sans-serif; max-width: 600px; margin: auto; border: 1px solid #ddd; padding: 20px; border-radius: 10px;'>
              <div style='text-align: center;'>
                <h1 style='margin-bottom: 20px; font-size: 36px;background: black; '>
            <span style='color: purple;'>FIT</span><span style='color: white;'>GYM</span>
                </h1>
                <h2 style='color: #4CAF50;'>Email Təsdiqi</h2>
              </div>
              <p>Salam!</p>
              <p>Qeydiyyatı tamamlamak üçün aşağıdakı təsdiq kodunu istifadə edin:</p>
              <div style='font-size: 24px; font-weight: bold; color: #333; background: #f9f9f9; padding: 10px; text-align: center; border-radius: 6px; margin: 20px 0;'>
                {code}
              </div>
              <p style='color: #888;'>Bu kod <strong>5 dəqiqə</strong> ərzində etibarlıdır.</p>
              <hr />
              <p style='font-size: 12px; color: #aaa; text-align: center;'>Bu mesaj <strong>FITGYM</strong> tərəfindən göndərilmişdir.</p>
            </div>";


            await _emailService.SendEmailAsync(request.Email, "Təsdiq Kodu", body);

            return Unit.Value;
        }
    }

}
