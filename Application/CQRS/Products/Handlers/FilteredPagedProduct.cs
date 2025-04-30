using Domain.Entities;
using MediatR;
using Repository.Repositories;
using Application.CQRS.Products.ResponseDto;
using AutoMapper;
using Domain.Enums;
using global::Application.CQRS.Products.ResponseDto;
using global::AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;

namespace Application.CQRS.Products.Handlers;

public class FilteredPagedProduct
{
    public class Query : IRequest<FilteredPagedProductResponse>
    {
        public List<ProductColors> Colors { get; set; } = new();
        public int CategoryId { get; set; }
        public bool Ascending { get; set; } = true;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class Handler : IRequestHandler<Query, FilteredPagedProductResponse>
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public Handler(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<FilteredPagedProductResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var query = _repository
                .GetProductsByColorsAndCategory(request.Colors, request.CategoryId)
                .Where(p => !p.IsDeleted);

            query = request.Ascending
                ? query.OrderBy(p => p.Price)
                : query.OrderByDescending(p => p.Price);

            var totalCount = await query.CountAsync(cancellationToken);

            var pagedProducts = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            var productDtos = _mapper.Map<List<GetAllProductDto>>(pagedProducts);

            return new FilteredPagedProductResponse
            {
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize,
                Products = productDtos
            };
        }
    }
}

