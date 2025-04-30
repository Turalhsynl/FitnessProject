using Application.CQRS.UserProgram.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.UserProgram.Handlers;

public class AddUserProgram
{
    public class AddUserProgramCommand : IRequest<Result<UserProgramResponseDto>>
    {
        public int UserId { get; set; }
        public int ProgramId { get; set; }

        public class Handler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AddUserProgramCommand, Result<UserProgramResponseDto>>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;
            private readonly IMapper _mapper = mapper;

            public async Task<Result<UserProgramResponseDto>> Handle(AddUserProgramCommand request, CancellationToken cancellationToken)
            {
                if (_unitOfWork.UserProgramRepository.Exists(request.UserId, request.ProgramId))
                    return new Result<UserProgramResponseDto>(["This user is already registered in this program."]);

                var entity = new Domain.Entities.UserProgram
                {
                    UserId = request.UserId,
                    ProgramId = request.ProgramId,
                    CreatedDate = DateTime.Now
                };

                _unitOfWork.UserProgramRepository.AddUserProgram(entity);
                await _unitOfWork.SaveChangeAsync();

                var dto = _mapper.Map<UserProgramResponseDto>(entity);
                return new Result<UserProgramResponseDto> { Data = dto, IsSuccess = true };
            }
        }
    }

}
