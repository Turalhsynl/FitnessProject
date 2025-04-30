using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.MembershipPlans.Handlers;

public class CheckMembershipPlanExists
{
    public class CheckMembershipPlanExistsQuery : IRequest<Result<bool>>
    {
        public int Id { get; set; }
    }

    public class CheckMembershipPlanExistsHandler(IUnitOfWork unitOfWork) : IRequestHandler<CheckMembershipPlanExistsQuery, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<bool>> Handle(CheckMembershipPlanExistsQuery request, CancellationToken cancellationToken)
        {
            var exists = await _unitOfWork.MembershipPlanRepository.ExistsAsync(request.Id);
            return new Result<bool> { IsSuccess = true, Data = exists };
        }
    }
}
