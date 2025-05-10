using Application.CQRS.FitnessPrograms.ResponseDto;
using Application.CQRS.Recipes.ResponseDto;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.FitnessPrograms.Handlers;

public class GetFitnessProgramById
{
    public class GetFitnessProgramByIdQuery : IRequest<Result<FitnessProgramDto>>
    {
        public int Id { get; set; }
    }

    public class GetFitnessProgramByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetFitnessProgramByIdQuery, Result<FitnessProgramDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<FitnessProgramDto>> Handle(GetFitnessProgramByIdQuery request, CancellationToken cancellationToken)
        {
            var program = await _unitOfWork.FitnessProgramRepository.GetByIdAsync(request.Id);

            if (program == null)
            {
                return new Result<FitnessProgramDto>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Fitness program not found" }
                };
            }

            var recipes = await _unitOfWork.FitnessProgramRecipeRepository.GetRecipesByFitnessProgramIdAsync(program.Id);

            var recipeDtos = recipes.Select(r => new RecipeDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                Ingredients = r.Ingredients,
                Instructions = r.Instructions,
                ImageId = r.ImageId,
                Calories = r.Calories,
                MealType = r.MealType
            }).ToList();

            var fitnessProgramDto = new FitnessProgramDto
            {
                Id = program.Id,
                Name = program.Name,
                Description = program.Description,
                Level = program.Level,
                DurationInWeeks = program.DurationInWeeks,
                Gender = program.Gender,
                Price = program.Price,
                VideoUrl = program.VideoUrl,
                ImageId = program.ImageId,
                Recipes = recipeDtos
            };

            return new Result<FitnessProgramDto>
            {
                IsSuccess = true,
                Data = fitnessProgramDto,
                Errors = []
            };
        }

    }

}
