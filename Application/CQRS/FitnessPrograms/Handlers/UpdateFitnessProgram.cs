using Application.CQRS.FitnessPrograms.ResponseDto;
using Common.GlobalResponses;
using MediatR;
using Repository.Common;

namespace Application.CQRS.FitnessPrograms.Handlers;

public class UpdateFitnessProgram
{
    public class UpdateFitnessProgramCommand : IRequest<Result>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Level { get; set; }
        public int DurationInWeeks { get; set; }
        public string Gender { get; set; }
        public decimal Price { get; set; }
        public string VideoUrl { get; set; }
    }

    public class UpdateFitnessProgramCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<UpdateFitnessProgramCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result> Handle(UpdateFitnessProgramCommand request, CancellationToken cancellationToken)
        {
            var fitnessProgram = await _unitOfWork.FitnessProgramRepository.GetByIdAsync(request.Id);

            if (fitnessProgram == null)
            {
                return new Result { IsSuccess = false, Errors = new List<string> { "Fitness program not found" } };
            }

            fitnessProgram.Name = request.Name;
            fitnessProgram.Description = request.Description;
            fitnessProgram.Level = request.Level;
            fitnessProgram.DurationInWeeks = request.DurationInWeeks;
            fitnessProgram.Gender = request.Gender;
            fitnessProgram.Price = request.Price;
            fitnessProgram.VideoUrl = request.VideoUrl;
            fitnessProgram.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.FitnessProgramRepository.Update(fitnessProgram);

            await _unitOfWork.SaveChangeAsync();

            return new Result();
        }
    }


}
