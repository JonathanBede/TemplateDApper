using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities;
using Tornado.Logic.Interfaces;
using Tornado.Website.Controllers.Teachers;
using Tornado.Website.Models.Teacher;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class TeacherControllerTests
    {
        [Test]
        public void IndexShouldReturnView()
        {
            //Arrange
            var controller = new TeacherController(null, null, null, null, null, null, null);

            //ACT
            var result = controller.Index() as ViewResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Index"));
        }

        [Test]
        public void ManageTeachersShouldDisplayTeachers()
        {
            //Arrange
            var role = new Role { Name = "Teacher" };

            var roleLogic = new Mock<IRoleLogic>();
            roleLogic
                .Setup(x => x.GetByName("Teacher"))
                .Returns(role)
                .Verifiable("Should get teacher role");

            var logic = new Mock<IUserLogic>();
            logic
                .Setup(x => x.GetNumberOfUsersInRole(role))
                .Returns(200)
                .Verifiable("Should get number of teachers to work out number of pages");

            logic
                .Setup(x => x.GetUsersInRoleByPage(role, 0, 20))
                .Returns(new List<User>())
                .Verifiable("Should get 1st 20 users.");

            var controller = new TeacherController(logic.Object, roleLogic.Object, null, null, null, null, null);

            //ACT
            var result = controller.Manage() as ViewResult;

            //ASSERT
            logic.Verify();
            roleLogic.Verify();

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("Manage"));
        }

        [Test]
        public void CreateTeacherShouldDisplayTheView()
        {
            //ARRANGE
            var controller = new TeacherController(null, null, null, null, null, null, null);

            var result = controller.Create() as ViewResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("Create"));
        }

        [Test]
        public void CreateTeacherShouldSaveTheTeacher()
        {
            //ARRANGE
            const string userName = "Jburton";
            const string password = "test123";
            var model = new CreateTeacherViewModel { UserName = userName, Password = password };

            var userLogic = new Mock<IUserLogic>();
            userLogic
                .Setup(x => x.CreateUser(It.IsAny<User>()))
                .Returns(true)
                .Verifiable("Should store user");


            userLogic
                .Setup(x => x.ResetPassword(It.IsAny<string>(), password))
                .Returns(true)
                .Verifiable("Should set the teachers password");

            userLogic
                .Setup(x => x.AddToRole(It.IsAny<string>(), "Teacher"))
                .Verifiable("Should add to role.");

            var controller = new TeacherController(userLogic.Object, null, null, null, null, null, null);

            //ACT
            var result = controller.Create(model) as RedirectToRouteResult;

            //ASSERT
            userLogic.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("Manage", result.RouteValues["Action"]);
        }

        [Test]
        public void AddToClassShouldDisplayTheCorretView()
        {
            //ARRANGE
            var teacherId = "51f213d1-17a7-4e72-8aac-035d197a4f9f";
            var logic = new Mock<IClassLogic>();
            logic
                .Setup(x=>x.GetAll())
                .Returns(new List<ClassEntity>())
                .Verifiable("Should get list of classes to display");

            var controller = new TeacherController(null, null, logic.Object, null, null, null, null);

            //ACT
            var result = controller.AddClass(teacherId) as ViewResult;


            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("AddClass"));
        }

        [Test]
        public void AddToClassShouldSaveChange()
        {
            //ARRANGE
            var classId = Guid.NewGuid();
            const string teacherId = "51f213d1-17a7-4e72-8aac-035d197a4f9f";

            var model = new AddTeacherToClassViewModel{TeacherId = teacherId, Class = new ClassEntity{Id = classId}};

            var logic = new Mock<IClassLogic>();
            logic
                .Setup(x => x.AddTeacherToClass(teacherId, classId))
                .Verifiable("Should add teacher to class");

            var controller = new TeacherController(null, null, logic.Object, null, null, null, null);

            //ACT
            var result = controller.AddClass(model) as RedirectToRouteResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("Manage", result.RouteValues["Action"]);
        }

    }
}
