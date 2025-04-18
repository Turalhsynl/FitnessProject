using Application.CQRS.Orders.ResponseDto;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Orders.Handlers;

public class GetAllOrders
{
    public class GetAllOrdersQuery : IRequest<Result<List<OrderDto>>>
    {
    }
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, Result<List<OrderDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllOrdersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<List<OrderDto>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var orders = await _unitOfWork.OrderRepository.GetAllAsync();

            if (orders == null || !orders.Any())
            {
                return new Result<List<OrderDto>>
                {
                    IsSuccess = false,
                    Errors = ["Sifarişlər tapılmadı."]
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
