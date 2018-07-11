using System;
using Tornado.DataAccess.Interfaces.Api;
using Tornado.Domain.Entities.Api;
using Tornado.Logic.Interfaces.Api;

namespace Tornado.Logic.Logics.Api
{
    public class GameLogic : BaseLogic<GameEntity>, IGameLogic
    {
        private readonly IGameRepository _repositoy;

        public GameLogic(IGameRepository repository) : base(repository)
        {
            _repositoy = repository;
        }

        //public void AddTopic(Guid gameId, Guid topicId)
        //{
        //    _repositoy.AddTopic(gameId, topicId);
        //}

    }
}
