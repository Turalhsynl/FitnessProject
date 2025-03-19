using Application.CQRS.Products.ResponseDto;
using Application.CQRS.Users.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Products.Handlers;

public class UpdateProduct
{
    public class UpdateProductCommand : IRequest<Result<UpdateProductDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public string Description { get; set; } 
        public decimal Price { get; set; }
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
            if (currentProduct == null)
            {
                return new Result<UpdateProductDto>
                {
                    IsSuccess = false,
                    Errors = ["Product not found"]
                };
            }

            currentProduct.Name = request.Name;
            currentProduct.Description = request.Description;
            currentProduct.Price = request.Price;
            currentProduct.UpdatedBy = 1; 

            _unitOfWork.ProductRepository.Update(currentProduct);
            await _unitOfWork.SaveChangeAsync(); 

            var response = _mapper.Map<UpdateProductDto>(currentProduct);

            return new Result<UpdateProductDto>
            {
                Data = response,
                IsSuccess = true
            };
        }
    }
}