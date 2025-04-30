using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

public class DeleteProductFromCartCommand : IRequest<Result<Unit>>
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
}

public sealed class DeleteProductFromCartHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProductFromCartCommand, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Unit>> Handle(DeleteProductFromCartCommand request, CancellationToken cancellationToken)
    {
        await _unitOfWork.CartLineRepository.RemoveByCartAndProductAsync(request.CartId, request.ProductId);
        await _unitOfWork.SaveChangeAsync();

        return new Result<Unit> { IsSuccess = true };
    }
}
