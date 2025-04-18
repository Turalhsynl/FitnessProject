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

    public IRecipeRepository RecipeRepository { get; }

    public IFitnessProgramRepository FitnessProgramRepository { get; }
    public IFitnessProgramRecipeRepository FitnessProgramRecipeRepository { get; }
    public IUserProgramRepository UserProgramRepository { get; }
    public IMembershipPlanRepository MembershipPlanRepository { get; }
    public IFileUploadRepository FileUploadRepository { get; }

    Task<int> SaveChangeAsync();
}
