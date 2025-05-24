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
            var userInput = $"""
            Information:
            - Fitness category: {request.RequestDto.FitnessCategory}
            - Age: {request.RequestDto.Age}
            - Gender: {request.RequestDto.Gender}
            - Goal: {request.RequestDto.Goal}
            - Level: {request.RequestDto.Level}
            - Workout frequency (per week): {request.RequestDto.DaysPerWeek}
            - Body type: {request.RequestDto.BodyType}
            - Dream body: {request.RequestDto.DreamBody}
            - Target zone: {request.RequestDto.TargetZone}
            - Sleep duration: {request.RequestDto.SleepTime} hours
            - Height: {request.RequestDto.Height} cm
            - Weight: {request.RequestDto.Weight} kg

            Please create a personalized 1-month fitness program based on this information.
            At the end, include a short body analysis and give some advice.
            """;


            var aiResponse = await _aiService.GetResponseAsync(userInput);

            var plan = new Domain.Entities.WorkoutPlan
            {
                UserId = request.RequestDto.UserId,
                FitnessCategory = request.RequestDto.FitnessCategory,
                Age = request.RequestDto.Age,
                Gender = request.RequestDto.Gender,
                Goal = request.RequestDto.Goal,
                Level = request.RequestDto.Level,
                DaysPerWeek = request.RequestDto.DaysPerWeek,
                BodyType = request.RequestDto.BodyType,
                DreamBody = request.RequestDto.DreamBody,
                TargetZone = request.RequestDto.TargetZone,
                SleepTime = request.RequestDto.SleepTime,
                Height = request.RequestDto.Height,
                Weight = request.RequestDto.Weight,
                AiGeneratedContent = aiResponse
            };

            await _unitOfWork.WorkoutPlanRepository.AddAsync(plan);
            await _unitOfWork.SaveChangeAsync();

            return aiResponse;
        }
    }

}
