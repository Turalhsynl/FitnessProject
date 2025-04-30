using Application.CQRS.FitnessPrograms.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.UserPrograms.Handlers;

public class GetUserProgramsByUserId
{
    public class GetUserProgramsByUserIdQuery : IRequest<Result<List<FitnessProgramDto>>>
    {
        public int UserId { get; set; }

        public class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetUserProgramsByUserIdQuery, Result<List<FitnessProgramDto>>>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;
            private readonly IMapper _mapper = mapper;

            public async Task<Result<List<FitnessProgramDto>>> Handle(GetUserProgramsByUserIdQuery request, CancellationToken cancellationToken)
            {
                var programs = _unitOfWork.UserProgramRepository.GetProgramsByUserId(request.UserId);
                var dtoList = _mapper.Map<List<FitnessProgramDto>>(programs);

                return new Result<List<FitnessProgramDto>> { Data = dtoList, IsSuccess = true };
            }
        }
    }

}
