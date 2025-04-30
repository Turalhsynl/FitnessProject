using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Recipes.Handlers;

public class DeleteRecipe
{
    public class DeleteRecipeCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }
    public class DeleteRecipeCommandHandler : IRequestHandler<DeleteRecipeCommand, Result<bool>>
    {
        private readonly IRecipeRepository _recipeRepository;

        public DeleteRecipeCommandHandler(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository;
        }

        public async Task<Result<bool>> Handle(DeleteRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _recipeRepository.GetByIdAsync(request.Id);

            if (recipe == null)
            {
                return new Result<bool>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Recipe not found" }
                };
            }

            var result = await _recipeRepository.DeleteAsync(request.Id);

            return new Result<bool>
            {
                Data = result,
                Errors = new List<string>(),
                IsSuccess = result
            };
        }
    }
}
