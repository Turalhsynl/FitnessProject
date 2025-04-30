using Application.CQRS.Carts.ResponseDto;
using Application.Security;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

public class CreateCartCommand : IRequest<Result<CartDto>>
{
    public int UserId { get; set; }
}

public sealed class CreateCartHandler(IUnitOfWork unitOfWork, IUserContext userContext)
    : IRequestHandler<CreateCartCommand, Result<CartDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUserContext _userContext = userContext;

    public async Task<Result<CartDto>> Handle(CreateCartCommand request, CancellationToken cancellationToken)
    {
        var cart = new Cart { UserId = _userContext.MustGetUserId() };

        await _unitOfWork.CartRepository.AddAsync(cart);
        await _unitOfWork.SaveChangeAsync();

        var response = new Result<CartDto>
        {
            Data = new CartDto { Id = cart.Id, UserId = cart.UserId },
            Errors = [],
            IsSuccess = true
        };

        return response;
    }
}
