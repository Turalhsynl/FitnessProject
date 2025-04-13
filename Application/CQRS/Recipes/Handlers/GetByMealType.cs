using Application.CQRS.Recipes.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Recipes.Handlers;

public class GetByMealType
{
    public class GetByMealTypeQuery : IRequest<Result<IEnumerable<RecipeDto>>>
    {
        public string MealType { get; set; }

        public GetByMealTypeQuery(string mealType)
        {
            MealType = mealType;
        }
    }

    public class GetByMealTypeQueryHandler : IRequestHandler<GetByMealTypeQuery, Result<IEnumerable<RecipeDto>>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public GetByMealTypeQueryHandler(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<RecipeDto>>> Handle(GetByMealTypeQuery request, CancellationToken cancellationToken)
        {
            var recipes = await _recipeRepository.GetByMealTypeAsync(request.MealType);

            if (recipes == null || !recipes.Any())
            {
                return new Result<IEnumerable<RecipeDto>>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "No recipes found for the specified meal type." }
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
