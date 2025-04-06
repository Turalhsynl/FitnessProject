using Common.GlobalResponses.Generics;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Favorites.Handlers;

public class RemoveFavorite
{
    public class RemoveFavoriteCommand : IRequest<Result<bool>>
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }

    public sealed class Handler : IRequestHandler<RemoveFavoriteCommand, Result<bool>>
    {
        private readonly IFavoriteRepository _favoriteRepository;

        public Handler(IFavoriteRepository favoriteRepository)
        {
            _favoriteRepository = favoriteRepository;
        }

        public Task<Result<bool>> Handle(RemoveFavoriteCommand request, CancellationToken cancellationToken)
        {
            if (!_favoriteRepository.Exists(request.UserId, request.ProductId))
            {
                return Task.FromResult(new Result<bool>
                {
                    Data = false,
                    Errors = new List<string> { "Favorite does not exist." },
                    IsSuccess = false
                });
            }

            var result = _favoriteRepository.Remove(request.UserId, request.ProductId);

            if (!result)
            {
                return Task.FromResult(new Result<bool>
                {
                    Data = false,
                    Errors = new List<string> { "Failed to remove favorite." },
                    IsSuccess = false
                });
            }

            return Task.FromResult(new Result<bool>
            {
                Data = true,
                Errors = new List<string>(),
                IsSuccess = true
            });
        }
    }

}
