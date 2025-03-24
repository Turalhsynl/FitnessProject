using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using static Application.CQRS.Users.Handlers.Delete;

namespace Application.CQRS.Categories.Handlers;

public class DeleteCategory
{
    public class DeleteCategoryCommand : IRequest<Result<Unit>>
    {
        public int Id { get; set; }
        public int DeletedBy { get; set; }
    }

    public class Handler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteCategoryCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<Unit>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            _unitOfWork.CategoryRepository.Delete(request.Id, request.DeletedBy);
            await _unitOfWork.SaveChangeAsync();
            return new Result<Unit> { Errors = [], IsSuccess = true };
        }
    }
}
