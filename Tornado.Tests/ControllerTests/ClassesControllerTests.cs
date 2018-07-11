using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using Tornado.Domain.Entities;
using Tornado.Logic.Interfaces;
using Tornado.Website.Controllers.Classes;
using Tornado.Website.Models.Classes;

namespace Tornado.Tests.ControllerTests
{
    [TestFixture]
    public class ClassesControllerTests
    {
        [Test]
        public void IndexShouldDisplayTheCorrectView()
        {
            //ARRANGE
            var model = new List<ClassEntity>();

            var logic = new Mock<IClassLogic>();
            logic
                .Setup(x => x.GetAll())
                .Returns(model)
                .Verifiable("should get classes to display");

            var controller = new ClassesController(logic.Object, null);

            //ACT
            var result = controller.Index() as ViewResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Index"));
        }

        [Test]
        public void CreateShouldDisplayTheCorrectView()
        {
            //ARRANGE
            var model = new List<School>();

            var logic = new Mock<ISchoolLogic>();
            logic
                .Setup(x => x.GetAll())
                .Returns(model)
                .Verifiable("should get schools to display");

            var controller = new ClassesController(null, logic.Object);

            //ACT
            var result = controller.Create() as ViewResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Create"));
        }

        [Test]
        public void CreateShouldSaveAndDisplayCorrectView()
        {
            //ARRANGE
            var model = new ClassViewModel{Name= "Class One", School = new School{Id = Guid.NewGuid() } };

            var logic = new Mock<IClassLogic>();
            logic
                .Setup(x=>x.Create(It.IsAny<ClassEntity>()))
                .Verifiable("Should create class");

            var controller = new ClassesController(logic.Object, null);

            //ACT
            var result = controller.Create(model) as RedirectToRouteResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["Action"]);
        }

        [Test]
        [Ignore("work in progress")]
        public void CreateShouldValidateModel()
        {
            //ARRANGE
            var model = new ClassViewModel { Name = "Class One"};

            var logic = new Mock<ISchoolLogic>();
            logic
                .Setup(x => x.GetAll())
                .Returns(new List<School>())
                .Verifiable("Should re get the schools to display.");

            var controller = new ClassesController(null, logic.Object);

            //ACT
            var result = controller.Create(model) as ViewResult;

            //ASSERT
            logic.Verify();

            Assert.NotNull(result);
            Assert.That(result.ViewName, Is.EqualTo("Create"));

            Assert.That(result.ViewData.ModelState.Count, Is.EqualTo(1));
        }
    }
}
