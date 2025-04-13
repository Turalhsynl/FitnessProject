using Application.CQRS.FitnessPrograms.ResponseDto;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.FitnessPrograms.Handlers;

public class GetFitnessProgramById
{
    public class GetFitnessProgramByIdQuery : IRequest<Result<FitnessProgramDto>>
    {
        public int Id { get; set; }
    }

    public class GetFitnessProgramByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetFitnessProgramByIdQuery, Result<FitnessProgramDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<FitnessProgramDto>> Handle(GetFitnessProgramByIdQuery request, CancellationToken cancellationToken)
        {
            var program = await _unitOfWork.FitnessProgramRepository.GetByIdAsync(request.Id);

            if (program == null)
            {
                return new Result<FitnessProgramDto>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Fitness program not found" }
                };
            }

            var fitnessProgramDto = new FitnessProgramDto
            {
                Id = program.Id,
                Name = program.Name,
                Description = program.Description,
                Level = program.Level,
                DurationInWeeks = program.DurationInWeeks,
                Gender = program.Gender,
                Price = program.Price,
                VideoUrl = program.VideoUrl,
            };

            return new Result<FitnessProgramDto>
            {
                IsSuccess = true,
                Data = fitnessProgramDto,
                Errors = []
            };
        }
    }

}
