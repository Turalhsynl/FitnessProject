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
        public List<ProductColors> Colors { get; set; } = new();
        public int CategoryId { get; set; }
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
            var query = _repository.GetAll();

            if (request.CategoryId != 0)
            {
                query = query.Where(p => p.CategoryId == request.CategoryId);
            }
            if (request.Colors.Any())
            {
                query = query.Where(p => request.Colors.Contains(p.Color));
            }

            query = request.Ascending
                ? query.OrderBy(p => p.Price)
                : query.OrderByDescending(p => p.Price);

            var products = await query.ToListAsync(cancellationToken);

            var productDtos = _mapper.Map<List<GetAllProductDto>>(products);

            return productDtos;
        }
    }
}

