using Application.CQRS.FitnessPrograms.ResponseDto;
using Application.CQRS.Recipes.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using Repository.Repositories;

namespace Application.CQRS.FitnessPrograms.Handlers;

public class GetAllFitnessProgram
{
    public class GetAllFitnessProgramsQuery : IRequest<Result<List<FitnessProgramDto>>> { }

    public class GetAllFitnessProgramsHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllFitnessProgramsQuery, Result<List<FitnessProgramDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<List<FitnessProgramDto>>> Handle(GetAllFitnessProgramsQuery request, CancellationToken cancellationToken)
        {
            var fitnessPrograms = await _unitOfWork.FitnessProgramRepository.GetAllAsync();

            if (fitnessPrograms == null || !fitnessPrograms.Any())
            {
                return new Result<List<FitnessProgramDto>>(new List<string> { "Fitness proqramları tapılmadı." });
            }

            var fitnessProgramDtos = new List<FitnessProgramDto>();

            foreach (var program in fitnessPrograms.Where(p => !p.IsDeleted))
            {
                var fitnessProgramDto = new FitnessProgramDto
                {
                    Id = program.Id,
                    Name = program.Name,
                    Description = program.Description,
                    VideoUrl = program.VideoUrl,
                    Level = program.Level,
                    DurationInWeeks = program.DurationInWeeks,
                    Price = program.Price,
                    Gender = program.Gender,
                    ImageUrl = program.ImageUrl,
                    Recipes = new List<RecipeDto>()
                };

                var recipes = await _unitOfWork.FitnessProgramRecipeRepository
                    .GetRecipesByFitnessProgramIdAsync(program.Id);

                foreach (var recipe in recipes)
                {
                    fitnessProgramDto.Recipes.Add(new RecipeDto
                    {
                        Id = recipe.Id,
                        Name = recipe.Name,
                        Description = recipe.Description,
                        Ingredients = recipe.Ingredients,
                        Instructions = recipe.Instructions,
                        ImageUrl = recipe.ImageUrl,
                        Calories = recipe.Calories,
                        MealType = recipe.MealType
                    });
                }

                fitnessProgramDtos.Add(fitnessProgramDto);
            }

            return new Result<List<FitnessProgramDto>> { Data = fitnessProgramDtos, IsSuccess = true };
        }
    }
}
