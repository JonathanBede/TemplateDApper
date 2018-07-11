using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Http.Results;
using Moq;
using NUnit.Framework;
using Tornado.API.Controllers;
using Tornado.API.Models;
using Tornado.Domain.Entities.Api;
using Tornado.Logic.Interfaces.Api;

namespace Tornado.Api.Tests.ControllerTests
{
    [TestFixture]
    public class GameControllerTests
    {
        [Test]
        public void GetAllShouldReturnGames()
        {
            //ARRANGE
            var game = new GameEntity
            {
                Description = "A new game"
            };

            var logic = new Mock<IGameLogic>();
            logic
                .Setup(x => x.Where(It.IsAny<Expression<Func<GameEntity, bool>>>()))
                .Returns(new List<GameEntity> { game })
                .Verifiable("should get games");

            var controller = new GameController(logic.Object);

            //ACT
            var result = controller.GetAll() as OkNegotiatedContentResult<List<Game>>;

            //ASSERT
            logic.Verify();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual("A new game", result.Content[0].Description);
        }

        [Test]
        public void GetShouldReturn404WhenGameNotFound()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var logic = new Mock<IGameLogic>();
            logic
                .Setup(x => x.Get(id))
                .Returns((GameEntity)null)
                .Verifiable("should get game");

            var controller = new GameController(logic.Object);

            //ACT
            var result = controller.Get(id) as NotFoundResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
        }

        [Test]
        public void GetShouldReturnGameWhenFound()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var profile = new GameEntity
            {
                Description = "new game"
            };

            var logic = new Mock<IGameLogic>();
            logic
                .Setup(x => x.Get(id))
                .Returns(profile)
                .Verifiable("should get game");

            var controller = new GameController(logic.Object);

            //ACT
            var result = controller.Get(id) as OkNegotiatedContentResult<Game>;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual("new game", result.Content.Description);
        }
    }
}
