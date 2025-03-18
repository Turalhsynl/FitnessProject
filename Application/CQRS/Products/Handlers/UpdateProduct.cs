using Application.CQRS.Users.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;

namespace Application.CQRS.Products.Handlers;

public class UpdateProduct
{
    public record struct UpdateProductCommand : IRequest<Result<UpdateDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public int Stock { get; set; }
        public string Category { get; set; }
    }

    public sealed class Handler : IRequestHandler<UpdateProductCommand, Result<UpdateDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<UpdateDto>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var currentProduct = await _unitOfWork.ProductRepository.GetByIdAsync(request.Id);
            if (currentProduct == null) throw new Exception("Product not found");
            currentProduct.Name = request.Name;
            currentProduct.Description = request.Description;
            currentProduct.Price = request.Price;
            currentProduct.UpdatedBy = 1;
            _unitOfWork.ProductRepository.Update(currentProduct);

            var response = _mapper.Map<UpdateDto>(currentProduct);

            return new Result<UpdateDto>
            {
                Data = response,
                Errors = [],
                IsSuccess = true
            };
        }
    }
}