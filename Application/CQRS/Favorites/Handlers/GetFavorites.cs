using Application.CQRS.Favorites.ResponseDto;
using Application.CQRS.Products.ResponseDto;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public Handler(IFavoriteRepository favoriteRepository, IMapper mapper)
        {
            _favoriteRepository = favoriteRepository;
            _mapper = mapper;
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
                Product = _mapper.Map<GetAllProductDto>(f.Product)
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


