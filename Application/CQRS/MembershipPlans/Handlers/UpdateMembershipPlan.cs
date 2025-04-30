using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.MembershipPlans.Handlers;

public class UpdateMembershipPlan
{
    public class UpdateMembershipPlanCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int MaxProgramsAllowed { get; set; }
    }

    public class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateMembershipPlanCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<bool>> Handle(UpdateMembershipPlanCommand request, CancellationToken cancellationToken)
        {
            var plan = await _unitOfWork.MembershipPlanRepository.GetByIdAsync(request.Id);
            if (plan == null)
                return new Result<bool> { IsSuccess = false, Errors = ["Membership plan not found."] };

            _mapper.Map(request, plan);
            plan.UpdatedDate = DateTime.UtcNow;

            _unitOfWork.MembershipPlanRepository.Update(plan);
            await _unitOfWork.SaveChangeAsync();

            return new Result<bool> { IsSuccess = true, Data = true };
        }
    }
}

