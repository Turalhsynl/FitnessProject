using Application.CQRS.Favorites.ResponseDto;
using AutoMapper;
using Common.Exceptions;
using Common.GlobalResponses.Generics;
using Domain.Entities;
using MediatR;
using Repository.Repositories;

namespace Application.CQRS.Favorites.Handlers;

public class AddFavorite
{
    public class AddFavoriteCommand : IRequest<Result<FavoriteAddDto>>
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }

    public sealed class Handler : IRequestHandler<AddFavoriteCommand, Result<FavoriteAddDto>>
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IMapper _mapper;

        public Handler(IFavoriteRepository favoriteRepository, IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
        }

        public async Task<Result<FavoriteAddDto>> Handle(AddFavoriteCommand request, CancellationToken cancellationToken)
        {

            if (_favoriteRepository.Exists(request.UserId, request.ProductId))
            {
                throw new ConflictException("Favorite already exists for this product.");
            }


            var favorite = new Favorite
            {
                UserId = request.UserId,
                ProductId = request.ProductId,
                CreatedAt = DateTime.Now
            };

            var result = _favoriteRepository.Add(favorite);

            if (!result)
            {
                throw new Exception("Failed to add favorite.");
            }

            var favoriteDto = _mapper.Map<FavoriteAddDto>(favorite);

            return new Result<FavoriteAddDto>
            {
                Data = favoriteDto,
                Errors = new List<string>(),
                IsSuccess = true
            };
        }
    }
}

