using Application.CQRS.Recipes.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Recipes.Handlers;

public class GetAllRecipes
{
    public class GetAllRecipesQuery : IRequest<Result<IEnumerable<RecipeDto>>>
    {
    }

    public class GetAllRecipesQueryHandler : IRequestHandler<GetAllRecipesQuery, Result<IEnumerable<RecipeDto>>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public GetAllRecipesQueryHandler(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<RecipeDto>>> Handle(GetAllRecipesQuery request, CancellationToken cancellationToken)
        {
            var recipes = await _recipeRepository.GetAllAsync();

            if (recipes == null)
            {
                return new Result<IEnumerable<RecipeDto>>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "No recipes found" }
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
