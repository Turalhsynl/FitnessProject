using Application.Security;
using Common.GlobalResponses.Generics;
using Domain.Enums;
using MediatR;
using Repository.Common;

namespace Application.CQRS.FitnessPrograms.Handlers;

public class CreateFitnessProgram
{
    public class CreateFitnessProgramCommand : IRequest<Result<int>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public int DurationInWeeks { get; set; }
        public string Gender { get; set; }
        public decimal Price { get; set; }
        public int ImageId { get; set; }
        public string VideoUrl { get; set; }
    }

     public class CreateFitnessProgramCommandHandler : IRequestHandler<CreateFitnessProgramCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext; 

        public CreateFitnessProgramCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<Result<int>> Handle(CreateFitnessProgramCommand request, CancellationToken cancellationToken)
        {
            try
            {
              
                var userId = _userContext.MustGetUserId();
                var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

                if (user == null)
                    return new Result<int> { IsSuccess = false, Errors = new List<string> { "Kullanıcı bulunamadı." } };

       
                if (user.UserRole != UserRoles.Coach)
                    return new Result<int> { IsSuccess = false, Errors = new List<string> { "Sadece eğitmenler yeni fitness programı oluşturabilir." } };

          
                var fitnessProgram = new FitnessProgram
                {
                    Name = request.Name,
                    Description = request.Description,
                    Level = request.Level,
                    DurationInWeeks = request.DurationInWeeks,
                    Gender = request.Gender,
                    Price = request.Price,
                    VideoUrl = request.VideoUrl,
                    ImageId = request.ImageId,
                    CreatedDate = DateTime.UtcNow,
                    UserId = user.Id
                };

           
                await _unitOfWork.FitnessProgramRepository.AddAsync(fitnessProgram);
                await _unitOfWork.SaveChangeAsync();

        
                return new Result<int> { IsSuccess = true, Data = fitnessProgram.Id, Errors = new List<string>() };
            }
            catch (Exception ex)
            {
           
                return new Result<int>
                {
                    IsSuccess = false,
                    Errors = new List<string> { $"An error occurred while saving the entity changes. Error: {ex.Message}, InnerException: {ex.InnerException?.Message}" }
                };
            }
        }

    }

}
