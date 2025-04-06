using Domain.Entities;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Favorites.Handlers;

public class AddFavorite
{
    public class AddFavoriteCommand : IRequest<bool>
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }
    public class AddFavoriteCommandHandler : IRequestHandler<AddFavoriteCommand, bool>
    {
        private readonly IFavoriteRepository _repository;

        public AddFavoriteCommandHandler(IFavoriteRepository repository)
        {
            _repository = repository;
        }

        public Task<bool> Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
        {
            var favorite = new Favorite
            {
                UserId = request.UserId,
                ProductId = request.ProductId,
                CreatedAt = DateTime.Now
            };

            var result = _repository.Add(favorite);
            return Task.FromResult(result);
        }
    }

}
