using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities.Api;
using Tornado.Logic.Interfaces.Api;
using Tornado.Website.Controllers.Words;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class WordControllerTests
    {
        [Test]
        public void IndexShouldDisplayTheProfiles()
        {
            //ARRANGE
            var logic = new Mock<IWordLogic>();
            logic
                .Setup(x=>x.GetAll())
                .Returns(new List<WordEntity>())
                .Verifiable("Should get the words to display");

            var controller = new WordController(logic.Object, null);

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
            var controller = new WordController(null, null);

            //ACT
            var result = controller.Create() as ViewResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("Create"));
        }

        [Test]
        public void CreateShouldSaveTheProfile()
        {
            //ARRANGE
            var model = new WordEntity();

            var logic = new Mock<IWordLogic>();
            logic
                .Setup(x => x.Create(model))
                .Verifiable("Should save the new word.");

            var controller = new WordController(logic.Object, null);

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

            var logic = new Mock<IWordLogic>();
            logic
                .Setup(x=>x.Get(id))
                .Returns(new WordEntity())
                .Verifiable("Should get the word to update");

            var controller = new WordController(logic.Object, null);

            //ACT
            var result = controller.Update(id) as ViewResult;

            logic.Verify();//ASSERT

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("Update"));
        }

        [Test]
        public void UpdateShouldSaveTheProfile()
        {
            //ARRANGE
            var id = Guid.NewGuid();
            var model = new WordEntity{Id = id};
            var profileToUpdate = new WordEntity();

            var logic = new Mock<IWordLogic>();
            logic
                .Setup(x => x.Get(id))
                .Returns(profileToUpdate)
                .Verifiable("Should get the word to update");
            logic
                .Setup(x=>x.Update(profileToUpdate))
                .Verifiable("Should save the changes");

            var controller = new WordController(logic.Object, null);

            //ACT
            var result = controller.Update(model) as RedirectToRouteResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }
    }
}
