using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Favorites.Handlers;

public class RemoveFavorite
{
    public class RemoveFavoriteCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
    public class RemoveFavoriteCommandHandler : IRequestHandler<RemoveFavoriteCommand, bool>
    {
        private readonly IFavoriteRepository _repository;

        public RemoveFavoriteCommandHandler(IFavoriteRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(RemoveFavoriteCommand request, CancellationToken cancellationToken)
        {
            var result = _repository.Remove(request.UserId, request.ProductId);
            return Task.FromResult(result);
        }
    }

}
