using Application.CQRS.Orders.ResponseDto;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;
using Repository.Repositories;

namespace Application.CQRS.Orders.Handlers;

public class GetOrderLines
{
    public class GetOrderLinesQuery : IRequest<Result<List<OrderLineDto>>>
    {
        public int OrderId { get; set; }

        public GetOrderLinesQuery(int orderId)
        {
            OrderId = orderId;
        }
    }

    public class GetOrderLinesQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetOrderLinesQuery, Result<List<OrderLineDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<Result<List<OrderLineDto>>> Handle(GetOrderLinesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var orderLines = await _unitOfWork.OrderLineRepository.GetOrderLinesByOrderIdAsync(request.OrderId);

                var orderLineDTOs = orderLines.Select(ol => new OrderLineDto
                {
                    ProductId = ol.ProductId,
                    Quantity = ol.Quantity,
                    Price = ol.Price
                }).ToList();

                return new Result<List<OrderLineDto>>() { IsSuccess = true, Data = orderLineDTOs, Errors = [] };
            }
            catch (Exception ex)
            {
                return new Result<List<OrderLineDto>>() { IsSuccess = false, Data = null, Errors = [ex.Message] };
            }
        }
    }

}
