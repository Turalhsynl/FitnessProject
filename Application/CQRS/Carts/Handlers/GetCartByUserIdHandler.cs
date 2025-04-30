using Application.CQRS.Carts.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Common;

public class GetCartByUserIdQuery : IRequest<Result<CartDto>>
{
    public int UserId { get; set; }
}

public sealed class GetCartByUserIdHandler : IRequestHandler<GetCartByUserIdQuery, Result<CartDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GetCartByUserIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<CartDto>> Handle(GetCartByUserIdQuery request, CancellationToken cancellationToken)
    {
        var cart = await _unitOfWork.CartRepository.GetCartByUserIdAsync(request.UserId);

        if (cart == null)
        {
            cart = new Cart { UserId = request.UserId };
            await _unitOfWork.CartRepository.AddAsync(cart);
            await _unitOfWork.SaveChangeAsync();
        }

        var response = _mapper.Map<CartDto>(cart);
        response.CartLines = _mapper.Map<List<CartLineDto>>(cart.CartLines);

        return new Result<CartDto>() { Data = response, IsSuccess = true };
    }
}
