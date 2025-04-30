using Application.CQRS.Carts.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

public class GetCartQuery : IRequest<Result<CartDto>>
{
    public int CartId { get; set; }
}

public sealed class GetCartHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetCartQuery, Result<CartDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<CartDto>> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _unitOfWork.CartRepository.GetByIdWithProductsAsync(request.CartId);

        if (cart == null)
        {
            return new Result<CartDto> { IsSuccess = false, Errors = { "Cart not found" } };
        }

        var response = _mapper.Map<CartDto>(cart);
        response.CartLines = _mapper.Map<List<CartLineDto>>(cart.CartLines);

        return new Result<CartDto>() { Data = response, IsSuccess = true };

    }
}
