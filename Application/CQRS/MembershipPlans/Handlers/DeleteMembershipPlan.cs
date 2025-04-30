using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.MembershipPlans.Handlers;

public class DeleteMembershipPlan
{
    public class DeleteMembershipPlanCommand : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }

    public class Handler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteMembershipPlanCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<bool>> Handle(DeleteMembershipPlanCommand request, CancellationToken cancellationToken)
        {
            var plan = await _unitOfWork.MembershipPlanRepository.GetByIdAsync(request.Id);
            if (plan == null)
            {
                return new Result<bool>
                {
                    IsSuccess = false,
                    Errors = ["Membership plan not found."]
                };
            }

            _unitOfWork.MembershipPlanRepository.Remove(plan); // soft delete
            await _unitOfWork.SaveChangeAsync();

            return new Result<bool> { IsSuccess = true, Data = true };
        }
    }
}
