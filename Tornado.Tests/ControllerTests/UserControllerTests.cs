using System.Web.Mvc;
using NUnit.Framework;
using Tornado.Website.Controllers.Users;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class UserControllerTests
    {
        [Test]
        public void IndexShouldReturnView()
        {
            //Arrange
            var controller = new UsersController();

            //ACT
            var result = controller.Index() as ViewResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Index"));
        }
    }
}
