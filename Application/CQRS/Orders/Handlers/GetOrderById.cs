using Application.CQRS.Orders.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Orders.Handlers;

public class GetOrderById
{
    public class GetOrderByIdQuery : IRequest<Result<OrderDto>>
    {
        public int OrderId { get; set; }

        public GetOrderByIdQuery(int orderId)
        {
            OrderId = orderId;
        }
    }

    public sealed class GetOrderHandler : IRequestHandler<GetOrderByIdQuery, Result<OrderDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOrderHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<OrderDto>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                return new Result<OrderDto>
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Order not found" }
                };
            }

            var response = _mapper.Map<OrderDto>(order);
            response.OrderLines = _mapper.Map<List<OrderLineDto>>(order.OrderLines);

            return new Result<OrderDto>
            {
                Data = response,
                IsSuccess = true
            };
        }
    }
}
