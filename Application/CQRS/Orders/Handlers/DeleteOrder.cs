using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Orders.Handlers;

public class DeleteOrder
{
    public class DeleteOrderCommand : IRequest<Result<Unit>>
    {
        public int OrderId { get; set; }
    }
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, Result<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Unit>> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _unitOfWork.OrderRepository.GetByIdAsync(request.OrderId);

            if (order == null)
            {
                return new Result<Unit> { IsSuccess = false, Errors = ["Sifariş tapılmadı"] };
            }

            await _unitOfWork.OrderRepository.DeleteAsync(request.OrderId);

            return new Result<Unit> { IsSuccess = true };
        }
    }
}
