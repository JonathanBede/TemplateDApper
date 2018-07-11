using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities.Api;
using Tornado.Logic.Interfaces.Api;
using Tornado.Website.Controllers.Rounds;
using Tornado.Website.Models.Round;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class RoundControllerTests
    {

        [Test]
        public void IndexShouldDisplayTheRounds()
        {
            //ARRANGE
            var logic = new Mock<IRoundLogic>();
            logic
                .Setup(x=>x.GetAll())
                .Returns(new List<RoundEntity>())
                .Verifiable("Should get the rounds to display");

            var controller = new RoundController(logic.Object, null, null);

            //ACT
            var result = controller.Index() as ViewResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("Index"));
        }

        [Test]
        public void CreateShouldDisplayTheCorrectView()
        {
            //ARRANGE
            var levelLogic = new Mock<ILevelLogic>();
            levelLogic
                .Setup(x=>x.GetAll())
                .Verifiable("Should get the levels to pick from.");

            var controller = new RoundController(null, levelLogic.Object, null);

            //ACT
            var result = controller.Create() as ViewResult;

            //ASSERT
            levelLogic.Verify();

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("Create"));
        }

        [Test]
        public void CreateShouldSaveRound()
        {
            //ARRANGE
            var model = new CreateRoundViewModel{Level = new LevelEntity{Id = Guid.NewGuid()}};

            var logic = new Mock<IRoundLogic>();
            logic
                .Setup(x => x.Create(It.IsAny<RoundEntity>()))
                .Verifiable("Should save round.");

            var controller = new RoundController(logic.Object, null, null);

            //ACT
            var result = controller.Create(model) as RedirectToRouteResult;

            //ASSERT
            logic.Verify();
  
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }


        [Test]
        public void UpdateShouldDisplayTheCorrectView()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var logic = new Mock<IRoundLogic>();
            logic
                .Setup(x=>x.Get(id))
                .Returns(new RoundEntity{Level = new LevelEntity{Id = Guid.NewGuid()}})
                .Verifiable("Should get the round to update");

            var levelLogic = new Mock<ILevelLogic>();
            levelLogic
                .Setup(x=>x.GetAll())
                .Returns(new List<LevelEntity>())
                .Verifiable("Should get the levels to select from.");

            var controller = new RoundController(logic.Object, levelLogic.Object, null);

            //ACT
            var result = controller.Update(id) as ViewResult;

            //ASSERT
            logic.Verify();
            levelLogic.Verify();

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("Update"));
        }

        [Test]
        public void UpdateShouldSaveTheRound()
        {
            //ARRANGE
            var model = new UpdateRoundViewModel{Id = Guid.NewGuid(), Level = new LevelEntity{Id = Guid.NewGuid()}};
            var profileToUpdate = new RoundEntity();

            var logic = new Mock<IRoundLogic>();
            logic
                .Setup(x => x.Get(model.Id))
                .Returns(profileToUpdate)
                .Verifiable("Should get the game to update");
            logic
                .Setup(x=>x.Update(profileToUpdate))
                .Verifiable("Should save the changes"); 

            var controller = new RoundController(logic.Object, null, null);

            //ACT
            var result = controller.Update(model) as RedirectToRouteResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }
    }
}
