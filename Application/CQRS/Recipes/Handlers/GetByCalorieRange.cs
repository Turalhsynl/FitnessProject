using Application.CQRS.Recipes.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Recipes.Handlers;

public class GetByCalorieRange
{
    public class GetByCalorieRangeQuery : IRequest<Result<IEnumerable<RecipeDto>>>
    {
        public int MinCalories { get; set; }
        public int MaxCalories { get; set; }

        public GetByCalorieRangeQuery(int minCalories, int maxCalories)
        {
            MinCalories = minCalories;
            MaxCalories = maxCalories;
        }
    }

    public class GetByCalorieRangeQueryHandler : IRequestHandler<GetByCalorieRangeQuery, Result<IEnumerable<RecipeDto>>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public GetByCalorieRangeQueryHandler(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<RecipeDto>>> Handle(GetByCalorieRangeQuery request, CancellationToken cancellationToken)
        {
            var recipes = await _recipeRepository.GetByCalorieRangeAsync(request.MinCalories, request.MaxCalories);

            if (recipes == null || !recipes.Any())
            {
                return new Result<IEnumerable<RecipeDto>>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "No recipes found for the specified calorie range." }
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
