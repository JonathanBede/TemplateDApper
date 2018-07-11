using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Swashbuckle.Swagger.Annotations;
using Tornado.API.Models;
using Tornado.Domain.Entities.Api;
using Tornado.Logic.Interfaces.Api;

namespace Tornado.API.Controllers
{
    /// <summary>
    /// Game
    /// </summary>
    [Authorize]
    [RoutePrefix("api/v1")]
    public class GameController : ApiController
    {
        private readonly IGameLogic _logic;

        /// <summary>
        /// Games
        /// </summary>
        /// <param name="logic"></param>
        public GameController(IGameLogic logic)
        {
            _logic = logic;
        }

        #region private helper methods

        private static Game MapGameEntityToModel(GameEntity gameEntity)
        {
            var model = new Game
            {
                Id = gameEntity.Id,
                Description = gameEntity.Description,
                Type = gameEntity.Type,
                AddWordsBand1 = gameEntity.AddWordsBand1,
                AddWordsBand2 = gameEntity.AddWordsBand2,
                PercentBandMin = gameEntity.PercentBandMin,
                PercentBandMax = gameEntity.PercentBandMax,
                ReEnterPositionMin = gameEntity.ReEnterPositionMin,
                ReEnterPositionMax = gameEntity.ReEnterPositionMax
            };

            return model;
        }

        #endregion

        /// <summary>
        /// Gets all the games for an app
        /// </summary>
        /// <remarks>This end point wil return all of the games</remarks>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/games")]
        [SwaggerResponse(201, "Returns the games for an app.", typeof(IEnumerable<Game>))]
        public IHttpActionResult GetAll()
        {
            var games = _logic.GetAll();

            var model = games.Select(MapGameEntityToModel).ToList();

            return Ok(model);
        }


        /// <summary>
        /// Gets the game
        /// </summary>
        /// <remarks>This will return a game for an app</remarks>
        /// <param name="id">The ID of the value you want to return</param>
        /// <response code="400">Bad request</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found</response>
        /// <response code="500">Internal Server Error</response>
        [HttpGet]
        [Route("/games/{id}")]
        [SwaggerResponse(201, "Returns a game.", typeof(Game))]
        public IHttpActionResult Get(Guid id)
        {
            var gameEntity = _logic.Get(id);

            if (gameEntity == null) return NotFound();

            var model = MapGameEntityToModel(gameEntity);

            return Ok(model);
        }
    }
}
