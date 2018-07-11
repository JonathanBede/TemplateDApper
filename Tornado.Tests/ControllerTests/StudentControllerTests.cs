using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities;
using Tornado.Logic.Interfaces;
using Tornado.Website.Controllers.Students;
using Tornado.Website.Models.Student;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class StudentControllerTests
    {
        [Test]
        public void IndexShouldReturnView()
        {
            //Arrange
            var controller = new StudentController(null, null, null, null, null, null);

            //ACT
            var result = controller.Index() as ViewResult;

            //ASSERT
            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Index"));
        }

        [Test]
        public void ManageStudentsShouldDisplayStudents()
        {
            //Arrange
            var role = new Role { Name = "Student" };

            var roleLogic = new Mock<IRoleLogic>();
            roleLogic
                .Setup(x => x.GetByName("Student"))
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

            var controller = new StudentController(logic.Object, roleLogic.Object, null, null, null, null);

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
        public void CreateStudentShouldDisplayTheCorrectView()
        {
            //ARRANGE
            var logic = new Mock<ISchoolLogic>();
            logic
                .Setup(x => x.GetAll())
                .Returns(new List<School>())
                .Verifiable("Should get schools to select");

            var controller = new StudentController(null, null, logic.Object, null, null, null);

            //ACT
            var result = controller.Create() as ViewResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.NotNull(result.Model);
            Assert.That(result.ViewName, Is.EqualTo("Create"));

        }

        [Test]
        public void CreateStudentShouldSaveTheStudent()
        {
            //ARRANGE   
            var classId = Guid.NewGuid();
            var classEntity = new ClassEntity();
            var model = new CreateStudentViewModel { Class = new ClassEntity { Id = classId }, UserName = "JBurton" };

            var classLogic = new Mock<IClassLogic>();
            classLogic
                .Setup(x => x.Get(classId))
                .Returns(classEntity)
                .Verifiable("Should get the class to add the student to.");
            classLogic
                .Setup(x => x.Update(It.IsAny<ClassEntity>()))
                .Returns(classEntity)
                .Verifiable("Should add user to class");

            var userLogic = new Mock<IUserLogic>();
            userLogic
                .Setup(x => x.AddToRole(It.IsAny<string>(), "Student"))
                .Verifiable("Should create user");

            var controller = new StudentController(userLogic.Object, null, null, classLogic.Object, null, null);

            //ACT
            var result = controller.Create(model) as RedirectToRouteResult;

            //ASSERT
            classLogic.Verify();
            userLogic.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("Manage", result.RouteValues["Action"]);
        }

    }
}
