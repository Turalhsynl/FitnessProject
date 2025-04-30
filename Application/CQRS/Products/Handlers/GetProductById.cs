using Application.CQRS.Products.ResponseDto;
using Application.CQRS.Users.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Products.Handlers;

public class GetProductById
{
    public class ProductQuery : IRequest<Result<GetProductByIdDto>>
    {
        public int Id { get; set; }
    }

    public sealed class Handler : IRequestHandler<ProductQuery, Result<GetProductByIdDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetProductByIdDto>> Handle(ProductQuery request, CancellationToken cancellationToken)
        {
            var currentProduct = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id);


            if (currentProduct == null)
            {
                return new Result<GetProductByIdDto>()
                {
                    Errors = [ "Product not found" ],
                    IsSuccess = false
                };
            }

            var response = _mapper.Map<GetProductByIdDto>(currentProduct);

            return new Result<GetProductByIdDto>
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }
    }
}
