using Application.CQRS.MembershipPlans.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.MembershipPlans.Handlers;

public class GetAllMembershipPlans
{
    public class Query : IRequest<Result<IEnumerable<MembershipPlanDto>>> { }

    public class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<Query, Result<IEnumerable<MembershipPlanDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        public async Task<Result<IEnumerable<MembershipPlanDto>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var plans = await _unitOfWork.MembershipPlanRepository.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<MembershipPlanDto>>(plans);

            return new Result<IEnumerable<MembershipPlanDto>> { IsSuccess = true, Data = dtos };
        }
    }
}

