using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Tornado.DataAccess.Interfaces.Api;
using Tornado.DataAccess.Repositories.Api;
using Tornado.Logic.Interfaces.Api;
using Tornado.Logic.Logics.Api;

namespace Tornado.API.Windsor
{
    public class DependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                .Pick()
                .WithServiceDefaultInterfaces()
                .Configure(c => c.LifestyleTransient()));

            //register logic 
            container.Register(Component.For<IGameLogic>()
                                        .ImplementedBy<GameLogic>()
                                        .LifestylePerThread());

            //register repositories
            container.Register(Component.For<IGameRepository>()
                                        .ImplementedBy<GameRepository>()
                                        .LifestylePerThread());
        }
    }
}