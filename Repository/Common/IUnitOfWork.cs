using Repository.Repositories;

namespace Repository.Common;

public interface IUnitOfWork
{
    public IUserRepository UserRepository { get; }
    public IProductRepository ProductRepository { get; }
    public IRefreshTokenRepository RefreshTokenRepository { get; }
    public ICategoryRepository CategoryRepository { get; }
    public ICartRepository CartRepository { get; }
    public ICartLineRepository CartLineRepository { get; }
    public IFavoriteRepository FavoriteRepository { get; }
    public IFitnessProgramRepository FitnessProgramRepository { get; }
    Task<int> SaveChangeAsync();
}
