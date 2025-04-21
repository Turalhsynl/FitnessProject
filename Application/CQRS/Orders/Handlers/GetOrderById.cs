using Application.CQRS.Orders.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using Repository.Repositories;

namespace Application.CQRS.Orders.Handlers;

public class GetOrderById
{
    public class GetOrderQuery : IRequest<Result<OrderDto>>
    {
        public int OrderId { get; set; }

        public GetOrderQuery(int orderId)
        {
            OrderId = orderId;
        }
    }

    public class GetOrderQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetOrderQuery, Result<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<OrderDto>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var order = await _unitOfWork.OrderRepository.GetOrderByIdAsync(request.OrderId);

                if (order == null)
                {
                    return new Result<OrderDto>(){ Errors = ["Order not found"], Data = null, IsSuccess = false};
                }

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
                    Errors = [],
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new Result<OrderDto>() { IsSuccess = false, Errors = [ex.Message], Data = null };
            }
        }
    }
}
