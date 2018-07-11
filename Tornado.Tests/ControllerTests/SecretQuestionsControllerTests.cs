using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities;
using Tornado.Logic.Interfaces;
using Tornado.Website.Controllers.SecretQuestions;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class SecretQuestionsControllerTests
    {
        [Test]
        public void IndexShouldDisplayTheCorrectView()
        {
            //ARRANGE
            //todo rename to SecretQuestion
            var model = new List<SecretQuestion>();

            var logic = new Mock<ISecretQuestionLogic>();
            logic
                .Setup(x => x.GetAll())
                .Returns(model)
                .Verifiable("should get questions to display");

            var controller = new SecretQuestionsController(logic.Object);

            //ACT
            var result = controller.Index() as ViewResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Index"));
        }

        [Test]
        public void CreateShouldDisplayTheCorrectView()
        {
            //ARRANGE
            var controller = new SecretQuestionsController(null);

            //ACT
            var result = controller.Create() as ViewResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("Create"));
        }

        [Test]
        public void CreateShouldSaveAndDisplayCorrectView()
        {
            //ARRANGE
            var model = new SecretQuestion { Name = "School One" };

            var logic = new Mock<ISecretQuestionLogic>();
            logic
                .Setup(x => x.Create(It.IsAny<SecretQuestion>()))
                .Verifiable("Should create sercet question");

            var controller = new SecretQuestionsController(logic.Object);

            //ACT
            var result = controller.Create(model) as RedirectToRouteResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }
    }
}
