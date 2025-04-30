using Application.CQRS.Recipes.ResponseDto;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;
using Repository.Repositories;

namespace Application.CQRS.Recipes.Handlers;

public class AddRecipe
{
    public class AddRecipeCommand : IRequest<Result<RecipeDto>>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Ingredients { get; set; }
        public string Instructions { get; set; }
        public string ImageUrl { get; set; }
        public int Calories { get; set; }
        public string MealType { get; set; }
    }

    public class AddRecipeCommandHandler : IRequestHandler<AddRecipeCommand, Result<RecipeDto>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public AddRecipeCommandHandler(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<Result<RecipeDto>> Handle(AddRecipeCommand request, CancellationToken cancellationToken)
        {

            var newRecipe = new Recipe
            {
                Name = request.Name,
                Description = request.Description,
                Ingredients = request.Ingredients,
                Instructions = request.Instructions,
                ImageUrl = request.ImageUrl,
                Calories = request.Calories,
                MealType = request.MealType,
                CreatedDate = DateTime.Now
            };

            var result = await _recipeRepository.AddAsync(newRecipe);

            if (result == null)
            {
                throw new Exception("Failed to add recipe.");
            }
            var recipeDto = _mapper.Map<RecipeDto>(newRecipe);

            return new Result<RecipeDto>
            {
                Data = recipeDto,
                Errors = new List<string>(),
                IsSuccess = true
            };
        }
    }
}
