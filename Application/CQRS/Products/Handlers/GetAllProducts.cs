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
    public record struct GetAllProductsQuery : IRequest<Result<List<GetAllDto>>> { }
    public sealed class Handler : IRequestHandler<GetAllProductsQuery, Result<List<GetAllDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public Handler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<List<GetAllDto>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var products = _unitOfWork.ProductRepository.GetAll();
            if (products == null || !products.Any())
                return new Result<List<GetAllDto>>
                {
                    Data = new List<GetAllDto>(),
                    Errors = new List<string> { "No products found" },
                    IsSuccess = false 
                };

            var response = _mapper.Map<List<GetAllDto>>(products);

            return new Result<List<GetAllDto>>
            {
                Data = response,
                Errors = [],
                IsSuccess = true 
            };
        }
    }
}
