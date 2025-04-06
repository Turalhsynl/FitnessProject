using Application.CQRS.Favorites.ResponseDto;
using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Favorites.Handlers;

public class GetFavorites
{
    public class GetFavoritesQuery : IRequest<Result<List<FavoriteDto>>>
    {
        public int UserId { get; set; }
    }

    public sealed class Handler : IRequestHandler<GetFavoritesQuery, Result<List<FavoriteDto>>>
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public Handler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public Task<Result<List<FavoriteDto>>> Handle(GetFavoritesQuery request, CancellationToken cancellationToken)
        {
            var favorites = _favoriteRepository.GetByUserId(request.UserId);

            if (favorites == null || !favorites.Any())
            {
                return Task.FromResult(new Result<List<FavoriteDto>>
                {
                    Data = new List<FavoriteDto>(),
                    Errors = new List<string> { "No favorites found." },
                    IsSuccess = false
                });
            }

            var favoriteDtos = favorites.Select(f => new FavoriteDto
            {
                ProductId = f.ProductId
            }).ToList();


            return Task.FromResult(new Result<List<FavoriteDto>>
            {
                Data = favoriteDtos,
                Errors = new List<string>(),
                IsSuccess = true
            });
        }
    }
}


