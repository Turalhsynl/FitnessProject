using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.UserPrograms.Handlers;

public class RemoveUserProgram
{
    public class RemoveUserProgramCommand : IRequest<Result<string>>
    {
        public int UserId { get; set; }
        public int ProgramId { get; set; }

        public class Handler(IUnitOfWork unitOfWork) : IRequestHandler<RemoveUserProgramCommand, Result<string>>
        {
            private readonly IUnitOfWork _unitOfWork = unitOfWork;

            public async Task<Result<string>> Handle(RemoveUserProgramCommand request, CancellationToken cancellationToken)
            {
                _unitOfWork.UserProgramRepository.RemoveUserProgram(request.UserId, request.ProgramId);
                await _unitOfWork.SaveChangeAsync();

                return new Result<string> { Data = "The user was deleted from the program.", IsSuccess = true };
            }
        }
    }

}
