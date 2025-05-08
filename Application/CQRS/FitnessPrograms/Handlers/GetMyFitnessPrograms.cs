using Application.CQRS.FitnessPrograms.ResponseDto;
using Application.Security;
using Common.GlobalResponses.Generics;
using Domain.Enums;
using MediatR;
using Repository.Common;

namespace Application.CQRS.FitnessPrograms.Handlers;


    public class GetMyFitnessPrograms
    {
        public class GetMyFitnessProgramsQuery : IRequest<Result<List<FitnessProgramDto>>>
        {
            public int UserId { get; set; } 
        }

        public class GetMyFitnessProgramsQueryHandler : IRequestHandler<GetMyFitnessProgramsQuery, Result<List<FitnessProgramDto>>>
        {
            private readonly IUnitOfWork _unitOfWork;

            public GetMyFitnessProgramsQueryHandler(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<List<FitnessProgramDto>>> Handle(GetMyFitnessProgramsQuery request, CancellationToken cancellationToken)
            {
                var user = await _unitOfWork.UserRepository.GetByIdAsync(request.UserId);

                if (user == null)
                    return new Result<List<FitnessProgramDto>>
                    {
                        IsSuccess = false,
                        Errors = ["Kullanıcı bulunamadı."]
                    };

                if (user.UserRole != UserRoles.Coach) 
                    return new Result<List<FitnessProgramDto>>
                    {
                        IsSuccess = false,
                        Errors = ["Sadece eğitmenler kendi fitness programlarını görebilir."]
                    };

                var fitnessPrograms = await _unitOfWork.FitnessProgramRepository.GetByUserIdAsync(request.UserId);

                var result = fitnessPrograms.Select(x => new FitnessProgramDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    Level = x.Level,
                    DurationInWeeks = x.DurationInWeeks,
                    Gender = x.Gender,
                    Price = x.Price,
                    VideoUrl = x.VideoUrl,
                    ImageId = x.ImageId,
                    UserId = x.UserId,
                }).ToList();

                return new Result<List<FitnessProgramDto>>
                {
                    IsSuccess = true,
                    Data = result
                };
            }
        }
    }

