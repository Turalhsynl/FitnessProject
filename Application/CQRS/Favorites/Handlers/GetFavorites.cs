using Application.CQRS.Favorites.ResponseDto;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Favorites.Handlers;

public class GetFavorites
{
    public class GetFavoritesQuery : IRequest<List<FavoriteDto>>
    {
        public int UserId { get; set; }
    }
    public class GetFavoritesQueryHandler : IRequestHandler<GetFavoritesQuery, List<FavoriteDto>>
    {
        private readonly IFavoriteRepository _repository;

        public GetFavoritesQueryHandler(IFavoriteRepository repository)
        {
            _repository = repository;
        }

        public Task<List<FavoriteDto>> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
        {
            var favorites = _repository.GetByUserId(request.UserId);
            var result = favorites.Select(f => new FavoriteDto
            {
                ProductId = f.ProductId
            }).ToList();

            return Task.FromResult(result);
        }
    }

}
