using System.Web.Mvc;
using NUnit.Framework;
using Tornado.Website.Controllers.ChatBots;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class ChatBotControllerTests
    {
        [Test]
        public void IndexShouldDisplayTheCorrectView()
        {
            //ARRANGE
            var controller = new ChatBotController(null);

            //ACT
            var result = controller.Index() as ViewResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Index"));
        }
    }
}
