using Common.GlobalResponses;
using MediatR;
using Repository.Common;

namespace Application.CQRS.FitnessPrograms.Handlers;

public class DeleteFitnessProgram
{
    public class DeleteFitnessProgramCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }

    public class DeleteFitnessProgramCommandHandler : IRequestHandler<DeleteFitnessProgramCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFitnessProgramCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteFitnessProgramCommand request, CancellationToken cancellationToken)
        {
            var fitnessProgram = await _unitOfWork.FitnessProgramRepository.GetByIdAsync(request.Id);

            if (fitnessProgram == null)
            {
                return new Result { IsSuccess = false, Errors = new List<string> { "Fitness program not found" } };
            }

            fitnessProgram.IsDeleted = true;
            fitnessProgram.DeletedDate = DateTime.UtcNow;

            _unitOfWork.FitnessProgramRepository.Update(fitnessProgram);
            await _unitOfWork.SaveChangeAsync();

            return new Result { IsSuccess = true , Errors = []};
        }
    }

}
