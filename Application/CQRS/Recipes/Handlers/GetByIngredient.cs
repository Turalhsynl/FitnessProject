using Application.CQRS.Recipes.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Recipes.Handlers;

public class GetByIngredient
{
    public class GetByIngredientQuery : IRequest<Result<IEnumerable<RecipeDto>>>
    {
        public string Ingredient { get; set; }

        public GetByIngredientQuery(string ingredient)
        {
            Ingredient = ingredient;
        }
    }

    public class GetByIngredientQueryHandler : IRequestHandler<GetByIngredientQuery, Result<IEnumerable<RecipeDto>>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public GetByIngredientQueryHandler(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<RecipeDto>>> Handle(GetByIngredientQuery request, CancellationToken cancellationToken)
        {
            var recipes = await _recipeRepository.GetByIngredientAsync(request.Ingredient);

            if (recipes == null || !recipes.Any())
            {
                return new Result<IEnumerable<RecipeDto>>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "No recipes found for the specified ingredient." }
                };
            }

            var recipeDtos = _mapper.Map<IEnumerable<RecipeDto>>(recipes);

            return new Result<IEnumerable<RecipeDto>>
            {
                Data = recipeDtos,
                Errors = new List<string>(),
                IsSuccess = true
            };
        }
    }
}
