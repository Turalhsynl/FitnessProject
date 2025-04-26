using Application.CQRS.Orders.ResponseDto;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Orders.Handlers;

public class CreateOrder
{
    public class CreateOrderCommand : IRequest<Result<OrderDto>>
    {
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderLineDto> OrderLines { get; set; }
    }

    public class CreateOrderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, Result<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = new Order
                {
                    UserId = request.UserId,
                    TotalAmount = request.TotalAmount,
                    Status = "Completed",
                };

                await _unitOfWork.OrderRepository.AddAsync(order);
                await _unitOfWork.SaveChangeAsync();

                // OrderLine əlavə etmə və stoku azaltma
                foreach (var item in request.OrderLines)
                {
                    var orderLine = new OrderLine
                    {
                        OrderId = order.Id,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Price = item.Price
                    };

                    await _unitOfWork.OrderLineRepository.AddAsync(orderLine);

                    // Məhsulun stokunu azaldırıq
                    var product = await _unitOfWork.ProductRepository.GetByIdAsync(item.ProductId);
                    if (product != null && product.Quantity >= item.Quantity)
                    {
                        product.Quantity -= item.Quantity;  // Məhsulun stokunu azaldırıq
                        _unitOfWork.ProductRepository.Update(product); // Yenilənmiş məhsulu database-ə yazırıq
                    }
                    else
                    {
                        // Əgər stok kifayət etmirsə, xəta mesajı göndərə bilərik
                        return new Result<OrderDto>
                        {
                            IsSuccess = false,
                            Errors = new List<string> { "Not enough stock for product: " + item.ProductId }
                        };
                    }
                }

                await _unitOfWork.SaveChangeAsync();  // Bütün dəyişiklikləri qeyd edirik

                var orderDTO = new OrderDto
                {
                    Id = order.Id,
                    UserId = order.UserId,
                    TotalAmount = order.TotalAmount,
                    Status = order.Status,
                    OrderLines = order.OrderLines.Select(ol => new OrderLineDto
                    {
                        ProductId = ol.ProductId,
                        Quantity = ol.Quantity,
                        Price = ol.Price
                    }).ToList()
                };

                return new Result<OrderDto>()
                {
                    Data = orderDTO,
                    IsSuccess = true,
                    Errors = new List<string>()
                };
            }
            catch (Exception ex)
            {
                return new Result<OrderDto>()
                {
                    Data = null,
                    IsSuccess = false,
                    Errors = new List<string> { ex.Message }
                };
            }
        }
    }

}
