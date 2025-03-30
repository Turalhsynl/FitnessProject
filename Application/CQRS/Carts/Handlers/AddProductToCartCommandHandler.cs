using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

public class AddProductToCartCommand : IRequest<Result<Unit>>
{
    public int CartId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}

public sealed class AddProductToCartHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<AddProductToCartCommand, Result<Unit>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<Unit>> Handle(AddProductToCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _unitOfWork.CartRepository.GetByIdAsync(request.CartId);
        var product = await _unitOfWork.ProductRepository.GetByIdAsync(request.ProductId);

        if (cart == null || product == null)
        {
            return new Result<Unit> { IsSuccess = false, Errors = ["Cart or Product not found"] };
        }

        var cartLine = new CartLine
        {
            CartId = cart.Id,
            ProductId = product.Id,
            Quantity = request.Quantity,
            Product = product
        };

        await _unitOfWork.CartLineRepository.AddAsync(cartLine);
        await _unitOfWork.SaveChangeAsync();

        return new Result<Unit> { IsSuccess = true };
    }
}
