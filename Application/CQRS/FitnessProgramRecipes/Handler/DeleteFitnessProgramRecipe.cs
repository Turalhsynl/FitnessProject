using Application.Security;
using Common.GlobalResponses;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.FitnessProgramRecipes.Handler;

public class DeleteFitnessProgramRecipe
{
    public class DeleteFitnessProgramRecipeCommand : IRequest<Result>
    {
        public int FitnessProgramId { get; set; }
        public int RecipeId { get; set; }
    }

    public class DeleteFitnessProgramRecipeCommandHandler : IRequestHandler<DeleteFitnessProgramRecipeCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteFitnessProgramRecipeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(DeleteFitnessProgramRecipeCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var fitnessProgramRecipe = await _unitOfWork.FitnessProgramRecipeRepository
                    .GetByProgramAndRecipeAsync(request.FitnessProgramId ,request.RecipeId);

                if (fitnessProgramRecipe == null)
                {
                    return new Result(new List<string> { "Fitness program-recipe relationship not found." });
                }

                fitnessProgramRecipe.IsDeleted = true;
                fitnessProgramRecipe.DeletedDate = DateTime.Now;

                await _unitOfWork.SaveChangeAsync();

                return new Result();
            }
            catch (Exception ex)
            {
                return new Result(new List<string> { "An error occurred while deleting the record.", ex.Message });
            }
        }
    }
}
