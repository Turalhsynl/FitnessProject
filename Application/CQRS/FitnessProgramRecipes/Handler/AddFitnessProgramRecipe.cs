using Application.CQRS.FitnessProgramRecipes.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.FitnessProgramRecipes.Handler;

public class AddFitnessProgramRecipeCommand : IRequest<Result<FitnessProgramRecipeResponseDto>>
{
    public int FitnessProgramId { get; set; }
    public int RecipeId { get; set; }

    public class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AddFitnessProgramRecipeCommand, Result<FitnessProgramRecipeResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<FitnessProgramRecipeResponseDto>> Handle(AddFitnessProgramRecipeCommand request, CancellationToken cancellationToken)
        {
            var fitnessProgram = await _unitOfWork.FitnessProgramRepository.GetByIdAsync(request.FitnessProgramId);
            var recipe = await _unitOfWork.RecipeRepository.GetByIdAsync(request.RecipeId);

            if (fitnessProgram == null || recipe == null)
                return new Result<FitnessProgramRecipeResponseDto>(["Fitness program or recipe not found."]);

            var entity = new FitnessProgramRecipe
            {
                FitnessProgramId = request.FitnessProgramId,
                RecipeId = request.RecipeId
            };

            await _unitOfWork.FitnessProgramRecipeRepository.AddAsync(entity);
            await _unitOfWork.SaveChangeAsync();

            var dto = _mapper.Map<FitnessProgramRecipeResponseDto>(recipe);
            return new Result<FitnessProgramRecipeResponseDto> { Data = dto, Errors = [], IsSuccess = true };
        }
    }
}