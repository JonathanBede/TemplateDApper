using System;
using Tornado.Domain.Entities.Api;

namespace Tornado.DataAccess.Interfaces.Api
{
    public interface IGameRepository : IRepository<GameEntity>
    {
        //void AddTopic(Guid gameId, Guid topicId);
    }
}
