using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities;
using Tornado.Logic.Interfaces;
using Tornado.Website.Controllers.TeachingResources;
using Tornado.Website.Models.TeachingResource;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class TeachingResourceControllerTests
    {
        [Test]
        public void IndexShouldReturnViewWithTeachingResources()
        {
            //Arrange
            var model = new List<TeachingResource>();

            var logic = new Mock<ITeachingResourceLogic>();
            logic
                .Setup(x => x.GetAll())
                .Returns(model)
                .Verifiable("should get resources to display");

            var controller = new TeachingResourceController(logic.Object);

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
            var controller = new TeachingResourceController(null);

            //ACT
            var result = controller.Create() as ViewResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("Create"));
        }

        [Test]
        public void CreateShoudCreateTeachingResource()
        {
            //ARRANGE
            var model = new CreateTeachingResourceViewModel { Name = "Resource one" };
            

            var logic = new Mock<ITeachingResourceLogic>();
            logic
                .Setup(x => x.Create(It.IsAny<TeachingResource>()))
                .Verifiable("should save animation");

            var controller = new TeachingResourceController(logic.Object);

            //ACT
            var result = controller.Create(model) as RedirectToRouteResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);

        }
        
    }
}
