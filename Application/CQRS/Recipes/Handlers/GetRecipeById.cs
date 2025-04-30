using Application.CQRS.Recipes.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Recipes.Handlers;

public class GetRecipeById
{
    public class GetRecipeByIdQuery : IRequest<Result<RecipeDto>>
    {
        public int Id { get; set; }
    }
    public class GetRecipeByIdQueryHandler : IRequestHandler<GetRecipeByIdQuery, Result<RecipeDto>>
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;

        public GetRecipeByIdQueryHandler(IRecipeRepository recipeRepository, IMapper mapper)
        {
            _recipeRepository = recipeRepository;
            _mapper = mapper;
        }

        public async Task<Result<RecipeDto>> Handle(GetRecipeByIdQuery request, CancellationToken cancellationToken)
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

            var recipeDto = _mapper.Map<RecipeDto>(recipe);

            return new Result<RecipeDto>
            {
                Data = recipeDto,
                Errors = new List<string>(),
                IsSuccess = true
            };
        }
    }
}
