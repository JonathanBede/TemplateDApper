using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities;
using Tornado.Logic.Interfaces;
using Tornado.Website.Controllers.Profile;
using Tornado.Website.Helpers.Interfaces;
using Tornado.Website.Models.Profile;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class ProfileControllerTests
    {
        #region create prfile tests

        [Test]
        public void CreateShouldDisplayCorrectView()
        {
            //ARRANGE
            const string userName = "jburton";
            var user = new User();

            var userMock = new Mock<IPrincipal>();
            userMock
                .Setup(p => p.Identity.Name)
                .Returns(userName);

            var context = new Mock<HttpContextBase>();
            context
                .Setup(ctx => ctx.User)
                .Returns(userMock.Object);

            var controllerContext = new Mock<ControllerContext>();
            controllerContext
                .Setup(con => con.HttpContext)
                .Returns(context.Object);

            var userLogic = new Mock<IUserLogic>();
            userLogic
                .Setup(x => x.FindByName(userName))
                .Returns(user)
                .Verifiable("Should get the user.");

            var secretQuestionLogic = new Mock<ISecretQuestionLogic>();
            secretQuestionLogic
                .Setup(x => x.GetAll())
                .Returns(new List<SecretQuestion>())
                .Verifiable("Should get secret questions to display.");

            var controller = new ProfileController(userLogic.Object, secretQuestionLogic.Object, null)
            {
                ControllerContext = controllerContext.Object
            };

            //ACT
            var result = controller.CreateProfile() as ViewResult;

            //ASSERT
            userLogic.Verify();
            secretQuestionLogic.Verify();

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("CreateProfile"));
        }

        [Test]
        public void CreateProfileShouldCreateTheProfile()
        {
            //ARRANGE
            var id = Guid.NewGuid();
            var model = new CreateProfile{Day = 13, Month = 09, Year = 1984, SecretQuestionId = id , Answer = "test"};
            const string userName = "jburton";
            var user = new User{Id = "9fa22176-d0d4-4746-bc3d-28dd01ffae29" };

            var userMock = new Mock<IPrincipal>();
            userMock
                .Setup(p => p.Identity.Name)
                .Returns(userName);

            var context = new Mock<HttpContextBase>();
            context
                .Setup(ctx => ctx.User)
                .Returns(userMock.Object);

            var controllerContext = new Mock<ControllerContext>();
            controllerContext
                .Setup(con => con.HttpContext)
                .Returns(context.Object);

            var userLogic = new Mock<IUserLogic>();
            userLogic
                .Setup(x => x.FindByName(userName))
                .Returns(user)
                .Verifiable("Should get the user.");

            userLogic
                .Setup(x => x.Update(user))
                .Returns(true)
                .Verifiable("Should create the user");

            var secretQuestionLogic = new Mock<ISecretQuestionLogic>();
            secretQuestionLogic
                .Setup(x => x.SaveSecretQuestionForUser(user.Id, id))
                .Verifiable("Should save secret question choice.");

            var encryptionHelper = new Mock<IEncryptionHelper>();
            encryptionHelper
                .Setup(x => x.EncryptString("test", "JellyTank"))
                .Verifiable();

            var controller = new ProfileController(userLogic.Object, secretQuestionLogic.Object, encryptionHelper.Object)
            {
                ControllerContext = controllerContext.Object
            };

            //ACT
            var result = controller.CreateProfile(model) as RedirectToRouteResult;

            //ASSERT
            userLogic.Verify();
            secretQuestionLogic.Verify();
            encryptionHelper.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("Home", result.RouteValues["Controller"]);
        }

        [Test]
        public void CreateProfileShouldRedirectCorrectlyWhenErroring()
        {
            //ARRANGE
            var model = new CreateProfile { Day = 13, Month = 09, Year = 1984 , Answer = "test"};
            const string userName = "jburton";
            var user = new User();

            var userMock = new Mock<IPrincipal>();
            userMock
                .Setup(p => p.Identity.Name)
                .Returns(userName);

            var context = new Mock<HttpContextBase>();
            context
                .Setup(ctx => ctx.User)
                .Returns(userMock.Object);

            var controllerContext = new Mock<ControllerContext>();
            controllerContext
                .Setup(con => con.HttpContext)
                .Returns(context.Object);

            var userLogic = new Mock<IUserLogic>();
            userLogic
                .Setup(x => x.FindByName(userName))
                .Returns(user)
                .Verifiable("Should get the user.");

            userLogic
                .Setup(x => x.Update(user))
                .Returns(false)
                .Verifiable("Should create the user");

            var encryptionHelper = new Mock<IEncryptionHelper>();
            encryptionHelper
                .Setup(x => x.EncryptString("test", "JellyTank"))
                .Verifiable();
            
            var controller = new ProfileController(userLogic.Object, null, encryptionHelper.Object)
            {
                ControllerContext = controllerContext.Object
            };

            //ACT
            var result = controller.CreateProfile(model) as ViewResult;

            //ASSERT
            userLogic.Verify();
            encryptionHelper.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("CreateProfile", result.ViewName);
        }

        #endregion

        #region create profile

        [Test]
        public void ChangeProfileShouldDisplayTheCorrectView()
        {
            //ARRANGE
            const string userName = "jburton";
            var user = new User{Answer = "test"};

            var userMock = new Mock<IPrincipal>();
            userMock
                .Setup(p => p.Identity.Name)
                .Returns(userName);

            var context = new Mock<HttpContextBase>();
            context
                .Setup(ctx => ctx.User)
                .Returns(userMock.Object);

            var controllerContext = new Mock<ControllerContext>();
            controllerContext
                .Setup(con => con.HttpContext)
                .Returns(context.Object);

            var userLogic = new Mock<IUserLogic>();
            userLogic
                .Setup(x => x.FindByName(userName))
                .Returns(user)
                .Verifiable("Should get the user.");

            var secretQuestionLogic = new Mock<ISecretQuestionLogic>();
            secretQuestionLogic
                .Setup(x=>x.GetAll())
                .Returns(new List<SecretQuestion>())
                .Verifiable("Should get secret questions to display.");

            var encryptionHelper = new Mock<IEncryptionHelper>();
            encryptionHelper
                .Setup(x=>x.DecryptString("test", "JellyTank"))
                .Verifiable();

            var controller = new ProfileController(userLogic.Object, secretQuestionLogic.Object, encryptionHelper.Object)
            {
                ControllerContext = controllerContext.Object
            };

            //ACT
            var result = controller.ChangeProfile() as ViewResult;

            //ASSERT
            userLogic.Verify();
            secretQuestionLogic.Verify();
            encryptionHelper.Verify();

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("ChangeProfile"));
        }

        [Test]
        public void ChangeProfileShouldCreateTheProfile()
        {
            //ARRANGE
            var id = Guid.NewGuid();
            var model = new ChangeProfile { Day = 13, Month = 09, Year = 1984, SecretQuestionId = id, Answer = "test"};
            const string userName = "jburton";
            var user = new User{Id = "9fa22176-d0d4-4746-bc3d-28dd01ffae29" };

            var userMock = new Mock<IPrincipal>();
            userMock
                .Setup(p => p.Identity.Name)
                .Returns(userName);

            var context = new Mock<HttpContextBase>();
            context
                .Setup(ctx => ctx.User)
                .Returns(userMock.Object);

            var controllerContext = new Mock<ControllerContext>();
            controllerContext
                .Setup(con => con.HttpContext)
                .Returns(context.Object);

            var userLogic = new Mock<IUserLogic>();
            userLogic
                .Setup(x => x.FindByName(userName))
                .Returns(user)
                .Verifiable("Should get the user.");

            userLogic
                .Setup(x => x.Update(user))
                .Returns(true)
                .Verifiable("Should create the user");

            var secretQuestionLogic = new Mock<ISecretQuestionLogic>();
            secretQuestionLogic
                .Setup(x=>x.SaveSecretQuestionForUser(user.Id, id))
                .Verifiable("Should save secret question choice.");

            var encryptionHelper = new Mock<IEncryptionHelper>();
            encryptionHelper
                .Setup(x => x.EncryptString("test", "JellyTank"))
                .Verifiable();

            var controller = new ProfileController(userLogic.Object, secretQuestionLogic.Object, encryptionHelper.Object)
            {
                ControllerContext = controllerContext.Object
            };

            //ACT
            var result = controller.ChangeProfile(model) as RedirectToRouteResult;

            //ASSERT
            userLogic.Verify();
            secretQuestionLogic.Verify();
            encryptionHelper.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("Home", result.RouteValues["Controller"]);
        }

        [Test]
        public void ChangeProfileShouldRedirectCorrectlyWhenErroring()
        {
            //ARRANGE
            var model = new ChangeProfile { Day = 13, Month = 09, Year = 1984, Answer = "test"};
            const string userName = "jburton";
            var user = new User();

            var userMock = new Mock<IPrincipal>();
            userMock
                .Setup(p => p.Identity.Name)
                .Returns(userName);

            var context = new Mock<HttpContextBase>();
            context
                .Setup(ctx => ctx.User)
                .Returns(userMock.Object);

            var controllerContext = new Mock<ControllerContext>();
            controllerContext
                .Setup(con => con.HttpContext)
                .Returns(context.Object);

            var userLogic = new Mock<IUserLogic>();
            userLogic
                .Setup(x => x.FindByName(userName))
                .Returns(user)
                .Verifiable("Should get the user.");

            userLogic
                .Setup(x => x.Update(user))
                .Returns(false)
                .Verifiable("Should create the user");

            var encryptionHelper = new Mock<IEncryptionHelper>();
            encryptionHelper
                .Setup(x => x.EncryptString("test", "JellyTank"))
                .Verifiable();

            var controller = new ProfileController(userLogic.Object, null, encryptionHelper.Object)
            {
                ControllerContext = controllerContext.Object
            };

            //ACT
            var result = controller.ChangeProfile(model) as ViewResult;

            //ASSERT
            userLogic.Verify();
            encryptionHelper.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("ChangeProfile", result.ViewName);
        }

        #endregion
    }
}
