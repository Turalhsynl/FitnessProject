using Application.CQRS.MembershipPlans.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.MembershipPlans.Handlers;

public class GetMembershipPlanById
{
    public class Query : IRequest<Result<MembershipPlanDto>>
    {
        public int Id { get; set; }
    }

    public class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<Query, Result<MembershipPlanDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<MembershipPlanDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var plan = await _unitOfWork.MembershipPlanRepository.GetByIdAsync(request.Id);
            if (plan == null)
                return new Result<MembershipPlanDto> { IsSuccess = false, Errors = ["Membership plan not found."] };

            var dto = _mapper.Map<MembershipPlanDto>(plan);
            return new Result<MembershipPlanDto> { IsSuccess = true, Data = dto };
        }
    }
}

