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
                    Status = "Pending",
                };

                await _unitOfWork.OrderRepository.AddAsync(order);
                await _unitOfWork.SaveChangeAsync();

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
                }

                await _unitOfWork.SaveChangeAsync();

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
                    Errors = []
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
