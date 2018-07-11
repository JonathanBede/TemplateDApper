using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities;
using Tornado.Logic.Interfaces;
using Tornado.Website.Controllers.Account;
using Tornado.Website.Helpers.Interfaces;
using Tornado.Website.Models.Account;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class AccountControllerTests
    {
        [Test]
        public void LoginShouldReturnTheCorrectView()
        {
            //ARRANGE
            var controller = new AccountController(null, null, null);

            //ACT
            var result = controller.Login("") as ViewResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Login"));
        }

        //todo:login test

        [Test]
        public void RegisterShouldDisplayTheCorrectView()
        {
            //ARRANGE
            var schoolLogic = new Mock<ISchoolLogic>();
            schoolLogic
                .Setup(x=>x.GetAll())
                .Returns(new List<School>())
                .Verifiable("Should get schools to choose from.");

            var controller = new AccountController(null, schoolLogic.Object, null);

            //ACT
            var result = controller.Register() as ViewResult;

            //ASSERT
            schoolLogic.Verify();

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("Register"));
        }

        //todo: register tests

        [Test]
        public void ChangePasswordShouldDisplayTheCorrectView()
        {
            //ARRANGE
            var controller = new AccountController(null, null, null);

            var result = controller.ChangePassword() as ViewResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("ChangePassword"));
        }

        [Test]
        public void ChangePasswordShouldChangeThePassword()
        {
            //ARRANGE
            var model = new ChangePassword
            {
                OldPassword = "test123",
                NewPassword = "test12345",
                ConfirmNewPassword = "test12345"
            };

            var user = new User
            {
                Id = "8285cc79-4561-499c-87cc-2e5997d62c20",
                UserName = "JBurton"

            };

            var controllerContext = new Mock<ControllerContext>();
            controllerContext
                .Setup(d => d.HttpContext.User.Identity.Name)
                .Returns("JBurton")
                .Verifiable("Should check for user");

            var userLogic = new Mock<IUserLogic>();
            userLogic
                .Setup(x => x.FindByName("JBurton"))
                .Returns(user);

            userLogic
                .Setup(x=>x.ChangePassword(user.Id, model.OldPassword, model.NewPassword))
                .Returns(true)
                .Verifiable("should change password");

            var controller = new AccountController(userLogic.Object, null, null)
            {
                ControllerContext = controllerContext.Object
            };

            var result = controller.ChangePassword(model) as RedirectToRouteResult;

            //ASSERT
            userLogic.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
            Assert.AreEqual("Home", result.RouteValues["Controller"]);
        }

        [Test]
        public void ForgottenPasswordShouldDisplayTheCorrectView()
        {
            //ARRANGE
            var controller = new AccountController(null, null, null);

            var result = controller.ForgottenPassword() as ViewResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("ForgottenPassword"));
        }

        [Test]
        public void ForgottenPasswordCheckUserAndShowAnswerView()
        {
            //ARRANGE
            const string userName = "JBurton";
            var model = new ForgottenPassword{UserName = userName };

            var userLogic = new Mock<IUserLogic>();
            userLogic.Setup(x=>x.FindByName(userName))
                .Returns(new User())
                .Verifiable("shoud get teh user to populate the model");

            var controller = new AccountController(userLogic.Object, null, null);

            var result = controller.ForgottenPassword(model) as ViewResult;

            //ASSERT
            userLogic.Verify();

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("AnswerQuestion"));
        }

        [Test]
        public void AnswerQuestionShouldCheckAnswerAndDisplayResetView()
        {
            //ARRANGE
            var model = new AnswerSecretQuestion
            {
                UserId = "8285cc79-4561-499c-87cc-2e5997d62c20",
                Answer = "secret answer"
            };

            var user = new User
            {
                Id = "8285cc79-4561-499c-87cc-2e5997d62c20",
                UserName = "JBurton",
                Answer = "Encrypted answer"
            };

            var userLogic = new Mock<IUserLogic>();
            userLogic.Setup(x => x.GetById(user.Id))
                .Returns(user)
                .Verifiable("shoud get the user to check the answer");

            var encryptionHelper = new Mock<IEncryptionHelper>();
            encryptionHelper
                .Setup(x=>x.DecryptString(user.Answer, "JellyTank"))
                .Returns("secret answer")
                .Verifiable("Should get answer");

            var controller = new AccountController(userLogic.Object, null, encryptionHelper.Object);

            //ACT
            var result = controller.AnswerQuestion(model) as ViewResult;

            //ASSERT
            userLogic.Verify();
            encryptionHelper.Verify();

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("ResetPassword"));
        }

        [Test]
        public void ResetPasswordShouldSaveTheChangeAndShowTheConfirmation()
        {
            //ARRANGE
            var model = new ResetPassword{UserId = "8285cc79-4561-499c-87cc-2e5997d62c20", NewPassword = "New Password"};

            var userLogic = new Mock<IUserLogic>();
            userLogic.Setup(x => x.ResetPassword(model.UserId, model.NewPassword))
                .Returns(true)
                .Verifiable("shoud save the new password");

            var controller = new AccountController(userLogic.Object, null, null);

            //ACT
            var result = controller.ResetPassword(model) as ViewResult;

            //ASSERT
            userLogic.Verify();

            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("PasswordResetConfirmation"));
        }

        [Test]
        [Ignore("need to work out how to mock getOwinContext")]
        public void LogOutShouldLogTheUserOut()
        {
            //ARRANGE
            //HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
           var controllerContext = new Mock<ControllerContext>();
            controllerContext
                .Setup(d => d.HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie))
                .Verifiable("Should check for user");

            var controller = new AccountController(null, null, null)
            {
                ControllerContext = controllerContext.Object,
                
            };

            //ACT
            var result = controller.LogOut() as RedirectToRouteResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.AreEqual("Login", result.RouteValues["Action"]);
            Assert.AreEqual("Account", result.RouteValues["Controller"]);
        }
    }
}
