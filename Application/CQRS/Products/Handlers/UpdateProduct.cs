using Application.CQRS.Products.ResponseDto;
using Application.CQRS.Users.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using Domain.Enums;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Products.Handlers;

public class UpdateProduct
{
    public class UpdateProductCommand : IRequest<Result<UpdateProductDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public ProductColors Color { get; set; }
    }

    public sealed class Handler : IRequestHandler<UpdateProductCommand, Result<UpdateProductDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<UpdateProductDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var currentProduct = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id);
            if (currentProduct == null) throw new Exception("Product not found");

            currentProduct.Name = request.Name;
            currentProduct.Description = request.Description;
            currentProduct.Price = request.Price;
            currentProduct.Color = request.Color;
            currentProduct.Quantity = request.Quantity;
            currentProduct.ImageUrl = request.ImageUrl;
            currentProduct.UpdatedBy = 1; 

            _unitOfWork.ProductRepository.Update(currentProduct);
  

            var response = _mapper.Map<UpdateProductDto>(currentProduct);

            return new Result<UpdateProductDto>
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }
    }
}