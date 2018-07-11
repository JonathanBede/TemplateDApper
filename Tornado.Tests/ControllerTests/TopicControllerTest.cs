using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities.Api;
using Tornado.Logic.Interfaces.Api;
using Tornado.Website.Controllers.Topics;
using Tornado.Website.Models.Shared;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class TopicControllerTest
    {
        [Test]
        public void IndexShouldReturnViewWithTopics()
        {
            //Arrange
            var model = new List<TopicEntity>();

            var logic = new Mock<ITopicLogic>();
            logic
                .Setup(x => x.GetAll())
                .Returns(model)
                .Verifiable("should get topics to display");

            var controller = new TopicController(logic.Object, null, null, null);

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
            var controller = new TopicController(null,null,null, null);

            //ACT
            var result = controller.Create() as ViewResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("Create"));
        }

        [Test]
        public void CreateShoudCreateTopic()
        {
            //ARRANGE
            var model = new TopicEntity
            {
                Description = "Topic one"
            };
            
            var logic = new Mock<ITopicLogic>();
            logic
                .Setup(x => x.Create(It.IsAny<TopicEntity>()))
                .Verifiable("should save topic");

            var controller = new TopicController(logic.Object, null, null, null);

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

            var logic = new Mock<ITopicLogic>();
            logic
                .Setup(x=>x.Get(id))
                .Returns(new TopicEntity())
                .Verifiable("Should get the topic to edit.");

            var controller = new TopicController(logic.Object, null, null, null);

            //ACT
            var result = controller.Update(id) as ViewResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("Update"));
        }

        [Test]
        public void UpdateShouldSaveTheTopic()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var model = new TopicEntity
            {
                Id = id,
                Description = "Topic one"
            };

            var logic = new Mock<ITopicLogic>();
            logic
                .Setup(x => x.Get(id))
                .Returns(new TopicEntity())
                .Verifiable("Should get the topic to edit.");
            logic
                .Setup(x => x.Update(It.IsAny<TopicEntity>()))
                .Verifiable("should save topic");

            var controller = new TopicController(logic.Object, null, null, null);

            //ACT
            var result = controller.Update(model) as RedirectToRouteResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [Test]
        public void AddWordShouldDisplayCorrectView()
        {
            //ARRANGE
            var id = Guid.NewGuid();

            var wordLogic = new Mock<IWordLogic>();
            wordLogic
                .Setup(x=>x.GetAll())
                .Returns(new List<WordEntity>())
                .Verifiable("Should get all the word in the system");

            var controller = new TopicController(null, wordLogic.Object, null, null);

            //ACT
            var result = controller.AddWord(id) as ViewResult;

            //ASSERT
            wordLogic.Verify();

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("AddWord"));
        }

        [Test]
        public void AddWordShouldSaveTheRelationship()
        {
            //ARRANGE
            var model = new AddWordViewModel
            {
                Word = new WordEntity {Id = Guid.NewGuid()}
            };

            var logic = new Mock<ITopicWordLogic>();
            logic
                .Setup(x=>x.Create(It.IsAny<TopicWord>()))
                .Verifiable("Should create the relationship.");

            var controller = new TopicController(null, null, logic.Object, null);

            //ACT
            var result = controller.AddWord(model) as RedirectToRouteResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);

        }
    }
}
