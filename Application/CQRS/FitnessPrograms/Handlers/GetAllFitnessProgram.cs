using Application.CQRS.FitnessPrograms.ResponseDto;
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

            // DTO-ya çevirmə
            var fitnessProgramDtos = fitnessPrograms
                .Where(p => !p.IsDeleted)
                .Select(p => new FitnessProgramDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    VideoUrl = p.VideoUrl,
                    Level = p.Level,
                    DurationInWeeks = p.DurationInWeeks,
                    Price = p.Price,
                    Gender = p.Gender,

                })
                .ToList();

            return new Result<List<FitnessProgramDto>> { Data = fitnessProgramDtos, Errors = [], IsSuccess = true };
        }
    }
}
