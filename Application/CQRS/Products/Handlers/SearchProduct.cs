using Domain.Entities;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Products.Handlers;

public class SearchProduct
{
    public class SearchProductQuery : IRequest<IEnumerable<Product>>
    {
        public string Text { get; }

        public SearchProductQuery(string text)
        {
            Text = text.ToLower();
        }
    }
    public class SearchProductQueryHandler : IRequestHandler<SearchProductQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _productRepository;

        public SearchProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> Handle(SearchProductQuery request, CancellationToken cancellationToken)
        {
            return await _productRepository.SearchProduct(request.Text);
        }
    }


}
