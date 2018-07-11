using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities;
using Tornado.Logic.Interfaces;
using Tornado.Website.Controllers.Roles;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class RolesControllerTests
    {

        [Test]
        public void IndexShouldDisplayRoles()
        {
            //ARRANGE
            var model = new List<Role>();

            var logic = new Mock<IRoleLogic>();
            logic
                .Setup(x=>x.GetAll())
                .Returns(model)
                .Verifiable("Should get the roles to display.");

            var controller = new RolesController(logic.Object);

            //ACT
            var result = controller.Index() as ViewResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Index"));
            Assert.That(result.Model, Is.EqualTo(model));

        }
    }
}
