using Application.CQRS.Products.ResponseDto;
using AutoMapper;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace Application.CQRS.Products.Handlers;

public class GetProductsByColor
{
    public class GetProductsByColorQuery : IRequest<List<GetAllProductDto>>
    {
        public ProductColors Color { get; set; }
        public bool Ascending { get; set; }
    }
    public class GetProductsByColorQueryHandler : IRequestHandler<GetProductsByColorQuery, List<GetAllProductDto>>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public GetProductsByColorQueryHandler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<GetAllProductDto>> Handle(GetProductsByColorQuery request, CancellationToken cancellationToken)
        {
            var query = _repository.GetProductsByColor(request.Color);

            if (request.Ascending)
            {
                query = query.OrderBy(p => p.Color);
            }
            else
            {
                query = query.OrderByDescending(p => p.Color);
            }

            var products = await query.ToListAsync(cancellationToken);

            var productDtos = _mapper.Map<List<GetAllProductDto>>(products);

            return productDtos;
        }
    }


}
