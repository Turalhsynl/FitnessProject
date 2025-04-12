using Common.GlobalResponses.Generics;
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
        public string VideoUrl { get; set; }
    }

    public class CreateFitnessProgramCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateFitnessProgramCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<int>> Handle(CreateFitnessProgramCommand request, CancellationToken cancellationToken)
        {
            var fitnessProgram = new FitnessProgram
            {
                Name = request.Name,
                Description = request.Description,
                Level = request.Level,
                DurationInWeeks = request.DurationInWeeks,
                Gender = request.Gender,
                Price = request.Price,
                VideoUrl = request.VideoUrl,
                CreatedDate = DateTime.UtcNow,
            };

            await _unitOfWork.FitnessProgramRepository.AddAsync(fitnessProgram);
            await _unitOfWork.SaveChangeAsync();

            return new Result<int> { IsSuccess = true, Data = fitnessProgram.Id, Errors = [] };
        }
    }

}
