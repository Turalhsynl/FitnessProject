using Application.CQRS.Recipes.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Recipes.Handlers;

public class UpdateRecipe
{
    public class UpdateRecipeCommand : IRequest<Result<RecipeDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public string ImageUrl { get; set; }
        public int Calories { get; set; }
        public string MealType { get; set; }
    }

    public class UpdateRecipeCommandHandler : IRequestHandler<UpdateRecipeCommand, Result<RecipeDto>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public UpdateRecipeCommandHandler(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<Result<RecipeDto>> Handle(UpdateRecipeCommand request, CancellationToken cancellationToken)
        {
            var recipe = await _recipeRepository.GetByIdAsync(request.Id);

            if (recipe == null)
            {
                return new Result<RecipeDto>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Recipe not found" }
                };
            }

            recipe.Name = request.Name;
            recipe.Description = request.Description;
            recipe.Ingredients = request.Ingredients;
            recipe.Instructions = request.Instructions;
            recipe.ImageUrl = request.ImageUrl;
            recipe.Calories = request.Calories;
            recipe.MealType = request.MealType;

            var updatedRecipe = await _recipeRepository.UpdateAsync(recipe);

            var recipeDto = _mapper.Map<RecipeDto>(updatedRecipe);

            return new Result<RecipeDto>
            {
                Data = recipeDto,
                Errors = new List<string>(),
                IsSuccess = true
            };
        }
    }
}
