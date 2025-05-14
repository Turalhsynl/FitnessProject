using Application.Abstractions;
using Application.CQRS.WorkoutPlan.ResponseDto;
using MediatR;
using Repository.Common;
using Repository.Repositories;

namespace Application.CQRS.WorkoutPlan.Handlers;

public class GenerateWorkoutHandler
{
    public class GenerateWorkoutCommand(WorkoutPlanDto dto) : IRequest<string>
    {
        public WorkoutPlanDto RequestDto { get; set; } = dto;
    }

    public class GenerateWorkoutCommandHandler(IOpenAIService aiService, IUnitOfWork unitOfWork) : IRequestHandler<GenerateWorkoutCommand, string>
    {
        private readonly IOpenAIService _aiService = aiService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<string> Handle(GenerateWorkoutCommand request, CancellationToken cancellationToken)
        {
            var aiResponse = await _aiService.GetResponseAsync($"Hazırla: {request.RequestDto.Goal}, Səviyyə: {request.RequestDto.Level}");

            var plan = new Domain.Entities.WorkoutPlan
            {
                UserId = request.RequestDto.UserId,
                Goal = request.RequestDto.Goal,
                Level = request.RequestDto.Level,
                DaysPerWeek = request.RequestDto.DaysPerWeek,
                Gender = request.RequestDto.Gender,
                AiGeneratedContent = aiResponse
            };

            await _unitOfWork.WorkoutPlanRepository.AddAsync(plan);
            await _unitOfWork.SaveChangeAsync();

            return aiResponse;
        }
    }

}
