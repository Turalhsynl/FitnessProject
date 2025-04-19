using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Orders.Handlers;

public class CreateOrder
{
    public class CreateOrderCommand : IRequest<Result<bool>>
    {
        public int UserId { get; set; }
    }

    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Result<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<bool>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(request.UserId);
            if (cart == null || !cart.CartLines.Any())
            {
                return new Result<bool>
                {
                    IsSuccess = false,
                    Errors = ["Cart is empty"]
                };
            }

            var order = new Order
            {
                UserId = request.UserId,
                TotalPrice = cart.CartLines.Sum(x => x.Product.Price * x.Quantity),
                OrderLines = cart.CartLines.Select(x => new OrderLine
                {
                    //ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    UnitPrice = x.Product.Price
                }).ToList()
            };

            foreach (var item in cart.CartLines)
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                if (product == null || product.Quantity < item.Quantity)
                {
                    return new Result<bool>
                    {
                        IsSuccess = false,
                        Errors = [$"Stock not enough for {product?.Name ?? "Unknown Product"}"]
                    };
                }

                product.Quantity -= item.Quantity;
                _unitOfWork.ProductRepository.Update(product);
            }

            await _unitOfWork.OrderRepository.AddOrderAsync(order);

            // await _unitOfWork.CartRepository.ClearCart(cart);

            await _unitOfWork.SaveChangeAsync();

            return new Result<bool>
            {
                IsSuccess = true,
                Errors = []
            };
        }
    }
}
