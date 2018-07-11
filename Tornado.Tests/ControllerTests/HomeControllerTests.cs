using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Website.Controllers.Home;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void StudentShouldBeDirectedToCorrectPage()
        {
            //ARRANGE
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(p => p.IsInRole("Student")).Returns(true);

            var contextMock = new Mock<HttpContextBase>();
            contextMock
                .Setup(ctx => ctx.User)
                .Returns(userMock.Object);

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock
                .Setup(con => con.HttpContext)
                .Returns(contextMock.Object);

            var controller = new HomeController(null) {ControllerContext = controllerContextMock.Object};

            //ACT
            var result = controller.Index() as RedirectToRouteResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("Student", result.RouteValues["Controller"]);
        }

        [Test]
        public void TeacherShouldBeDirectedToCorrectPage()
        {
            //ARRANGE
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(p => p.IsInRole("Student")).Returns(false);
            userMock.Setup(p => p.IsInRole("Teacher")).Returns(true);

            var contextMock = new Mock<HttpContextBase>();
            contextMock
                .Setup(ctx => ctx.User)
                .Returns(userMock.Object);

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock
                .Setup(con => con.HttpContext)
                .Returns(contextMock.Object);

            var controller = new HomeController(null) {ControllerContext = controllerContextMock.Object};

            //ACT
            var result = controller.Index() as RedirectToRouteResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("Teacher", result.RouteValues["Controller"]);
        }

        [Test]
        public void AdministratorShouldBeDirectedToCorrectPage()
        {
            //ARRANGE
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(p => p.IsInRole("Student")).Returns(false);
            userMock.Setup(p => p.IsInRole("Teacher")).Returns(false);
            userMock.Setup(p => p.IsInRole("Administrator")).Returns(true);

            var contextMock = new Mock<HttpContextBase>();
            contextMock
                .Setup(ctx => ctx.User)
                .Returns(userMock.Object);

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock
                .Setup(con => con.HttpContext)
                .Returns(contextMock.Object);

            var controller = new HomeController(null) { ControllerContext = controllerContextMock.Object};

            //ACT
            var result = controller.Index() as RedirectToRouteResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("Admin", result.RouteValues["Controller"]);
        }

        [Test]
        public void UserWithNoRoleShouldBeDirectedToCorrectPage()
        {
            //ARRANGE
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(p => p.IsInRole("Student")).Returns(false);
            userMock.Setup(p => p.IsInRole("Teacher")).Returns(false);
            userMock.Setup(p => p.IsInRole("Administrator")).Returns(false);

            var contextMock = new Mock<HttpContextBase>();
            contextMock
                .Setup(ctx => ctx.User)
                .Returns(userMock.Object);

            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock
                .Setup(con => con.HttpContext)
                .Returns(contextMock.Object);

            var controller = new HomeController(null) { ControllerContext = controllerContextMock.Object };

            //ACT
            var result = controller.Index() as RedirectToRouteResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.AreEqual("Login", result.RouteValues["Action"]);
            Assert.AreEqual("Account", result.RouteValues["Controller"]);
        }
    }
}
