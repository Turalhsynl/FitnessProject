using Domain.Entities;

namespace Repository.Repositories;

public interface IFavoriteRepository
{
    bool Exists(int userId, int productId);
    bool Add(Favorite favorite);
    bool Remove(int userId, int productId);
    List<Favorite> GetByUserId(int userId);
}

