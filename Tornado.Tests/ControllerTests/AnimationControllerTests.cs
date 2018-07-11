using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities;
using Tornado.Logic.Interfaces;
using Tornado.Website.Controllers.Animations;
using Tornado.Website.Models.Animation;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class AnimationControllerTests
    {
        [Test]
        public void IndexShouldReturnViewWithAnimations()
        {
            //Arrange
            var model = new List<Animation>();

            var animationLogic = new Mock<IAnimationLogic>();
            animationLogic
                .Setup(x => x.GetAll())
                .Returns(model)
                .Verifiable("should get animations to display");

            var controller = new AnimationController(animationLogic.Object);

            //ACT
            var result = controller.Index() as ViewResult;

            //ASSERT
            animationLogic.Verify();

            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Index"));
            Assert.That(result.Model, Is.EqualTo(model));
        }

        [Test]
        public void CreateShouldDisplayTheCorrectView()
        {
            //ARRANGE
            var controller = new AnimationController(null);

            //ACT
            var result = controller.Create() as ViewResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Create"));
        }

        [Test]
        public void CreateShoudCreateAnimation()
        {
            //ARRANGE
            var model = new CreateAnimationViewModel {Name = "Animation one"};
            

            var logic = new Mock<IAnimationLogic>();
            logic
                .Setup(x => x.Create(It.IsAny<Animation>()))
                .Verifiable("should save animation");

            var controller = new AnimationController(logic.Object);
            
            //ACT
            var result = controller.Create(model) as RedirectToRouteResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);

        }
    }
}
