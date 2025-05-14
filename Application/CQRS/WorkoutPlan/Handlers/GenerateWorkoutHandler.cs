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
            Məlumatlar:
            - Fitness kateqoriyası: {request.RequestDto.FitnessCategory}
            - Yaş: {request.RequestDto.Age}
            - Cinsiyyət: {request.RequestDto.Gender}
            - Məqsəd: {request.RequestDto.Goal}
            - Səviyyə: {request.RequestDto.Level}
            - Həftədə məşq sayı: {request.RequestDto.DaysPerWeek}
            - Bədən tipi: {request.RequestDto.BodyType}
            - Arzulanan bədən: {request.RequestDto.DreamBody}
            - Fokus bölgə: {request.RequestDto.TargetZone}
            - Yuxu saatı: {request.RequestDto.SleepTime}
            - Boy: {request.RequestDto.Height} sm
            - Çəki: {request.RequestDto.Weight} kq

            Zəhmət olmasa bu məlumatlara uyğun olaraq 1 aylıq fərdi fitness proqramı hazırla.
            Sonda bədənim haqqında qısa bir analiz və məsləhət də əlavə et.
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
