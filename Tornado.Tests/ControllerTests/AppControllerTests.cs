using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities.Api;
using Tornado.Logic.Interfaces.Api;
using Tornado.Website.Controllers.Apps;
using Tornado.Website.Models.App;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class AppControllerTests
    {
        [Test]
        public void IndexShouldReturnCorrectView()
        {
            //ARRANGE
            var logic = new Mock<IAppLogic>();
            logic.Setup(x=>x.GetAll())
                .Returns(new List<AppEntity>())
                .Verifiable("Should check settings exist");

            var controller = new AppController(logic.Object);

            //ACT
            var result = controller.Index() as ViewResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Index"));
        }

        [Test]
        public void CreateShouldReturnTheCorrectView()
        {
            //ARRANGE
            var controller = new AppController(null);

            //ACT
            var result = controller.Create() as ViewResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Create"));
        }

        [Test]
        public void CreateShouldSaveNewApp()
        {
            // ARRANGE
            var model = new CreateAppViewModel();
            
            var logic = new Mock<IAppLogic>();
            logic
                .Setup(x => x.Create(It.IsAny<AppEntity>()))
                .Verifiable("Should create the app");

            var controller = new AppController(logic.Object);

            //ACT
            var result = controller.Create(model) as RedirectToRouteResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("App", result.RouteValues["Controller"]);
        }
    }
}
