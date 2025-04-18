using Application.CQRS.Orders.ResponseDto;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Orders.Handlers;

public class GetOrdersByUserId
{
    public class GetOrdersByUserIdQuery : IRequest<Result<List<OrderDto>>>
    {
        public int UserId { get; set; }
    }
    public class GetOrdersByUserIdHandler : IRequestHandler<GetOrdersByUserIdQuery, Result<List<OrderDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrdersByUserIdHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<OrderDto>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.OrderRepository.GetOrdersByUserIdAsync(request.UserId);

            if (orders == null || !orders.Any())
            {
                return new Result<List<OrderDto>>
                {
                    IsSuccess = false,
                    Errors = ["Bu istifadəçiyə aid sifariş tapılmadı."]
                };
            }
            var orderDtos = orders.Select(order => new OrderDto
            {
                OrderId = order.Id,
                UserId = order.UserId,
                TotalPrice = order.TotalPrice,
                OrderLines = order.OrderLines.Select(ol => new OrderLineDto
                {
                    ProductId = ol.ProductId,
                    Quantity = ol.Quantity,
                    UnitPrice = ol.UnitPrice
                }).ToList()
            }).ToList();

            return new Result<List<OrderDto>>
            {
                IsSuccess = true,
                Errors = [],
                Data = orderDtos
            };
        }
    }
}
