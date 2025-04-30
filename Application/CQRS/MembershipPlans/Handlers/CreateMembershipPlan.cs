using AutoMapper;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.MembershipPlans.Handlers;


public class CreateMembershipPlan
{
    public class CreateMembershipPlanCommand : IRequest<Result<int>>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int MaxProgramsAllowed { get; set; }
    }

    public class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateMembershipPlanCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<int>> Handle(CreateMembershipPlanCommand request, CancellationToken cancellationToken)
        {
            var membershipPlan = _mapper.Map<MembershipPlan>(request);
            membershipPlan.CreatedDate = DateTime.UtcNow;

            await _unitOfWork.MembershipPlanRepository.AddAsync(membershipPlan);
            await _unitOfWork.SaveChangeAsync();

            return new Result<int> { IsSuccess = true, Data = membershipPlan.Id , Errors = []};
        }
    }
}
