using NUnit.Framework;
using Tornado.DataAccess.Repositories;
using Tornado.Domain.Entities;
using Tornado.Logic.Logics;

namespace Tornado.Tests.LogicTests
{
    

    [TestFixture]
    public class ClassLogicTests
    {

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void CreateShouldSaveAClass()
        {
            //ARRANGE
            var repository = new ClassRepository();
            var logic = new ClassLogic(repository);
            var classToCreate = new ClassEntity {Name = "ClassOne"};

            //ACT
            logic.Create(classToCreate);


            //ASSERT
            var classEntities = logic.GetAll();
            Assert.That(classEntities.Count, Is.EqualTo(1));
            Assert.That(classEntities[0].Name, Is.EqualTo("ClassOne"));
        }

    }
}
