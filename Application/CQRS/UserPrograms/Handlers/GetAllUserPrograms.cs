using Application.CQRS.UserProgram.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.UserPrograms.Handlers;
public class GetAllUserPrograms
{
    public class GetAllUserProgramsQuery : IRequest<Result<List<UserProgramResponseDto>>>
    {
        public class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetAllUserProgramsQuery, Result<List<UserProgramResponseDto>>>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;
            private readonly IMapper _mapper = mapper;

            public async Task<Result<List<UserProgramResponseDto>>> Handle(GetAllUserProgramsQuery request, CancellationToken cancellationToken)
            {
                var userPrograms = _unitOfWork.UserProgramRepository.GetAllUserPrograms();
                var dtoList = _mapper.Map<List<UserProgramResponseDto>>(userPrograms);

                return new Result<List<UserProgramResponseDto>> { Data = dtoList, IsSuccess = true };
            }
        }
    }

}
