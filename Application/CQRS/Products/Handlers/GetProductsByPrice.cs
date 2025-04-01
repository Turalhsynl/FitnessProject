using Domain.Entities;
using Domain.Enums;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Products.Handlers;

public class GetProductsByPrice
{
    public class GetProductsByPriceQuery : IRequest<IQueryable<Product>>
    {
        public int CategoryId { get; }
        public PriceSortOrder SortOrder { get; }

        public GetProductsByPriceQuery(int categoryId, PriceSortOrder sortOrder)
        {
            CategoryId = categoryId;
            SortOrder = sortOrder;
        }
    }

    public class GetProductsByPriceQueryHandler : IRequestHandler<GetProductsByPriceQuery, IQueryable<Product>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsByPriceQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IQueryable<Product>> Handle(GetProductsByPriceQuery request, CancellationToken cancellationToken)
        {
            var productsQuery = _productRepository.GetProductsByPrice(request.CategoryId, request.SortOrder);
            return productsQuery;
        }
    }


}
