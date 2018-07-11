using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities.Api;
using Tornado.Logic.Interfaces.Api;
using Tornado.Website.Controllers.Levels;
using Tornado.Website.Models.Level;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class LevelControllerTest
    {
        [Test]
        public void IndexShouldReturnViewWithLevels()
        {
            //Arrange
            var model = new List<LevelEntity>();

            var logic = new Mock<ILevelLogic>();
            logic
                .Setup(x => x.GetAll())
                .Returns(model)
                .Verifiable("should get levels to display");

            var controller = new LevelController(logic.Object, null);

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
            var gameLogic = new Mock<IGameLogic>();
            gameLogic.Setup(x=>x.GetAll())
                .Returns(new List<GameEntity>())
                .Verifiable("Should get the games to add the level to.");

            var controller = new LevelController(null, gameLogic.Object);

            //ACT
            var result = controller.Create() as ViewResult;

            //ASSERT
            gameLogic.Verify();
            
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("Create"));
        }

        [Test]
        public void CreateShoudCreateGame()
        {
            //ARRANGE
            var model = new CreateLevelViewModel
            {
                Name = "level one",
                Game = new GameEntity { Id = Guid.NewGuid()}
            };
            
            var logic = new Mock<ILevelLogic>();
            logic
                .Setup(x => x.Create(It.IsAny<LevelEntity>()))
                .Verifiable("should save level");

            var controller = new LevelController(logic.Object, null);

            //ACT
            var result = controller.Create(model) as RedirectToRouteResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);

        }
    }
}
