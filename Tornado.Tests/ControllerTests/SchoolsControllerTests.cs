using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities;
using Tornado.Logic.Interfaces;
using Tornado.Website.Controllers.Schools;
using Tornado.Website.Models.Schools;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class SchoolsControllerTests
    {
        [Test]
        public void IndexShouldDisplayTheCorrectView()
        {
            //ARRANGE
            var model = new List<School>();

            var logic = new Mock<ISchoolLogic>();
            logic
                .Setup(x => x.GetAll())
                .Returns(model)
                .Verifiable("should get schools to display");

            var controller = new SchoolsController(logic.Object);

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
            var controller = new SchoolsController(null );

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
            var model = new SchoolViewModel { Name = "School One" };

            var logic = new Mock<ISchoolLogic>();
            logic
                .Setup(x => x.Create(It.IsAny<School>()))
                .Verifiable("Should create class");

            var controller = new SchoolsController(logic.Object);

            //ACT
            var result = controller.Create(model) as RedirectToRouteResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }
    }
}
