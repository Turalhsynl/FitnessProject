using Application.CQRS.Recipes.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Recipes.Handlers;

public class SearchRecipeByName
{
    public class SearchRecipeByNameQuery : IRequest<Result<IEnumerable<RecipeDto>>>
    {
        public string Name { get; set; }
    }
    public class SearchRecipeByNameQueryHandler : IRequestHandler<SearchRecipeByNameQuery, Result<IEnumerable<RecipeDto>>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public SearchRecipeByNameQueryHandler(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<RecipeDto>>> Handle(SearchRecipeByNameQuery request, CancellationToken cancellationToken)
        {
            var recipes = await _recipeRepository.SearchByNameAsync(request.Name);

            if (!recipes.Any())
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
