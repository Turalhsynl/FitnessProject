using DAL.SqlServer.Context;
using DAL.SqlServer.Infastructure;
using Repository.Common;
using Repository.Repositories;

namespace DAL.SqlServer.UnitOfWork;

public class SqlUnitOfWork(string connectionString, AppDbContext context) : IUnitOfWork
{
    private readonly string _connectionString = connectionString;
    private readonly AppDbContext _context = context;

    public SqlProductRepository _productRepository;
    public SqlUserRepository _userRepository;
    public SqlRefreshTokenRepository _refreshTokenRepository;
    public SqlCategoryRepository _categoryRepository;
    public SqlCartRepository _cartRepository;
    public SqlCartLineRepository _cartLineRepository;
    public SqlFavoriteRepository _favoriteRepository;
    public SqlRecipeRepository _recipeRepository;
    public SqlFitnessProgramRepository _fitnessProgramRepository;
    public SqlFitnessProgramRecipeRepository _fitnessProgramRecipeRepository;
    public SqlUserProgramRepository _userProgramRepository;
    public SqlMembershipPlanRepository _membershipPlanRepository;
    public SqlFileUploadRepository _fileUploadRepository;
    public SqlOrderRepository _orderRepository;
    public SqlChatMessageRepository _chatMessageRepository;
    public SqlOrderLineRepository _orderLineRepository;
    public SqlWorkoutPlanRepository _workoutPlanRepository;
    public SqlEmailVerificationRepository _emailVerificationRepository;


    public IUserRepository UserRepository => _userRepository ?? new SqlUserRepository(_context);
    public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository ?? new SqlRefreshTokenRepository(_context);
    public IProductRepository ProductRepository => _productRepository ?? new SqlProductRepository(_context);
    public ICategoryRepository CategoryRepository => _categoryRepository ?? new SqlCategoryRepository(_context);
    public ICartRepository CartRepository => _cartRepository ??= new SqlCartRepository(_context);
    public ICartLineRepository CartLineRepository => _cartLineRepository ??= new SqlCartLineRepository(_context);
    public IFavoriteRepository FavoriteRepository => _favoriteRepository ??= new SqlFavoriteRepository(_context);
    public IFitnessProgramRepository FitnessProgramRepository => _fitnessProgramRepository ??= new SqlFitnessProgramRepository(_context);
    public IRecipeRepository RecipeRepository => _recipeRepository ??= new SqlRecipeRepository(_context);
    public IFitnessProgramRecipeRepository FitnessProgramRecipeRepository => _fitnessProgramRecipeRepository ??= new SqlFitnessProgramRecipeRepository(_context);
    public IUserProgramRepository UserProgramRepository => _userProgramRepository ??= new SqlUserProgramRepository(_context);
    public IMembershipPlanRepository MembershipPlanRepository => _membershipPlanRepository ??= new SqlMembershipPlanRepository(_context);
    public IFileUploadRepository FileUploadRepository => _fileUploadRepository ??= new SqlFileUploadRepository(_context);

    public IOrderRepository OrderRepository => _orderRepository ??= new SqlOrderRepository(_context);
    public IOrderLineRepository OrderLineRepository => _orderLineRepository ??= new SqlOrderLineRepository(_context);

    public IChatMessageRepository ChatMessageRepository => _chatMessageRepository ??= new SqlChatMessageRepository(_context);
    public IWorkoutRepository WorkoutPlanRepository => _workoutPlanRepository ??= new SqlWorkoutPlanRepository(_context);
    public IEmailVerificationRepository EmailVerificationRepository => _emailVerificationRepository ??= new SqlEmailVerificationRepository(_context);

    public async Task<int> SaveChangeAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
