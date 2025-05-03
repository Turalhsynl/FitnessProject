using Application.CQRS.MembershipPlans.ResponseDto;
using Application.Security;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponses;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.MembershipPlans.Handlers;

public class SubscribeToMembershipPlan
{
    public class SubscribeToMembershipCommand(SubscribeToMembershipDto dto) : IRequest<Result<string>>
    {
        public SubscribeToMembershipDto Dto { get; set; } = dto;
    }

    public class SubscribeToMembershipCommandHandler : IRequestHandler<SubscribeToMembershipCommand, Result<string>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserContext _userContext;
        private readonly IMapper _mapper;

        public SubscribeToMembershipCommandHandler(IUnitOfWork unitOfWork, IUserContext userContext, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userContext = userContext;
            _mapper = mapper;
        }

        public async Task<Result<string>> Handle(SubscribeToMembershipCommand request, CancellationToken cancellationToken)
        {
            var userId = _userContext.MustGetUserId();

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
                return new Result<string>() { Data = null, Errors = new List<string> { "İstifadəçi tapılmadı" }, IsSuccess = false };

            var membership = await _unitOfWork.MembershipPlanRepository.GetByIdAsync(request.Dto.Id);
            if (membership == null)
                return new Result<string>() { Data = null, Errors = new List<string> { "Abunəlik tapılmadı" }, IsSuccess = false };

            if (user.MembershipPlanId == membership.Id)
                return new Result<string>() { Data = null, Errors = new List<string> { "Bu abunəlik artıq təyin olunub" }, IsSuccess = false };

            user.MembershipPlanId = membership.Id;

            _unitOfWork.UserRepository.Update(user);
            await _unitOfWork.SaveChangeAsync();

            return new Result<string>(){
                Data = "Abunəlik uğurla təyin olundu",
                Errors = new List<string>(),
                IsSuccess = true
            };
        }
    }


}
