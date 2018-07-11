using System.Web.Mvc;
using NUnit.Framework;
using Tornado.Website.Controllers.Admin;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class AdminControllerTests
    {
        [Test]
        public void IndexShouldReturnView()
        {
            //Arrange
            var controller = new AdminController();

            //ACT
            var result = controller.Index() as ViewResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Index"));
        }
    }
}
