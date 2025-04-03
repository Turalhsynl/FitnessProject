using Domain.Entities;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Products.Handlers;

public class PageProduct
{
    public class PageProductQuery : IRequest<IEnumerable<Product>>
    {
        public int Page { get; }
        public int PageSize { get; }

        public PageProductQuery(int page, int pageSize)
        {
            Page = page < 1 ? 1 : page;
            PageSize = pageSize;
        }
    }
    public class PageProductQueryHandler : IRequestHandler<PageProductQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _repository;

        public PageProductQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> Handle(PageProductQuery request, CancellationToken cancellationToken)
        {
            return await _repository.PageProductAsync(request.Page, request.PageSize);
        }
    }


}
