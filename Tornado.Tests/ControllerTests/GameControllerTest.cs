using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities.Api;
using Tornado.Logic.Interfaces.Api;
using Tornado.Website.Controllers.Games;
using Tornado.Website.Models.Game;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class GameControllerTest
    {
        [Test]
        public void IndexShouldReturnViewWithAnimations()
        {
            //Arrange
            var model = new List<GameEntity>();

            var logic = new Mock<IGameLogic>();
            logic
                .Setup(x => x.GetAll())
                .Returns(model)
                .Verifiable("should get games to display");

            var controller = new GameController(logic.Object, null, null);

            //ACT
            var result = controller.Index() as ViewResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Index"));
            Assert.That(result.Model, Is.EqualTo(model));

        }

        [Test]
        public void CreateShouldDisplayTheCorrectView()
        {
            //ARRANGE
            var appLogic = new Mock<IAppLogic>();
            appLogic.Setup(x=>x.GetAll())
                .Returns(new List<AppEntity>())
                .Verifiable("Should get the app to add the game to.");

            var controller = new GameController(null, appLogic.Object, null);

            //ACT
            var result = controller.Create() as ViewResult;

            //ASSERT
            appLogic.Verify();

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("Create"));
        }

        [Test]
        public void CreateShoudCreateGame()
        {
            //ARRANGE
            var model = new CreateGameViewModel
            {
                Description = "Game one",
                App = new AppEntity{Id = Guid.NewGuid()}
            };
            
            var logic = new Mock<IGameLogic>();
            logic
                .Setup(x => x.Create(It.IsAny<GameEntity>()))
                .Verifiable("should save animation");

            var controller = new GameController(logic.Object, null, null);

            //ACT
            var result = controller.Create(model) as RedirectToRouteResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);

        }
    }
}
