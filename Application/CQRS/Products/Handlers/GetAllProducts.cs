using Application.CQRS.Products.ResponseDto;
using Application.CQRS.Users.ResponseDto;
using AutoMapper;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.CQRS.Products.Handlers;
public class GetAllProducts
{
    public record struct GetAllProductsQuery : IRequest<Result<List<GetAllProductDto>>> { }
    public sealed class Handler : IRequestHandler<GetAllProductsQuery, Result<List<GetAllProductDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllProductDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = _unitOfWork.ProductRepository.GetAll();
            if (products == null || !products.Any())
                return new Result<List<GetAllProductDto>>
                {
                    Data = new List<GetAllProductDto>(),
                    Errors = new List<string> { "No products found" },
                    IsSuccess = false 
                };

            var response = _mapper.Map<List<GetAllProductDto>>(products);

            return new Result<List<GetAllProductDto>>
            {
                Data = response,
                Errors = [],
                IsSuccess = true 
            };
        }
    }
}
