using Tornado.DataAccess.Interfaces.Api;
using Tornado.Domain.Entities.Api;

namespace Tornado.DataAccess.Repositories.Api
{
    public class GameRepository : BaseRepository<GameEntity>, IGameRepository
    {
       public GameRepository() : base("Games"){}

    }
}
