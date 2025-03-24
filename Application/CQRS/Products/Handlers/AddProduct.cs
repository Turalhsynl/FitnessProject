using Application.CQRS.Products.ResponseDto;
using Application.Security;
using AutoMapper;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Products.Handlers;

public class AddProduct
{
    public class AddProductCommand : IRequest<Result<AddProductDto>>
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public ProductColors Color { get; set; }
        //public int CategoryId { get; set; }
    }

    public sealed class Handler(IUnitOfWork unitOfWork, IMapper mapper, IUserContext userContext) : IRequestHandler<AddProductCommand, Result<AddProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly IUserContext _userContext = userContext;

        public async Task<Result<AddProductDto>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var newProduct = _mapper.Map<Product>(request);
            newProduct.CreatedBy = _userContext.MustGetUserId();

            if (string.IsNullOrEmpty(newProduct.Name))
            {
                throw new Exception("Product name is required");
            }

            await _unitOfWork.ProductRepository.AddAsync(newProduct);
            await _unitOfWork.SaveChangeAsync();

            var response = _mapper.Map<AddProductDto>(newProduct);

            return new Result<AddProductDto>
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }
    }
}
