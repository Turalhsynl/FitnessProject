using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

public class DeleteProduct
{
    public class DeleteProductCommand : IRequest<Result<Unit>>
    {
        public int Id { get; set; } 
    }

    public class Handler : IRequestHandler<DeleteProductCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public Handler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.ProductRepository.RemoveAsync(request.Id);
            await _unitOfWork.SaveChangeAsync();

            return new Result<Unit> { Errors = [], IsSuccess = true };
        }
    }
}
