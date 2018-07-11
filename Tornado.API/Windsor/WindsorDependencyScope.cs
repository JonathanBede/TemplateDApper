using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;
using Castle.Windsor;

namespace Tornado.API.Windsor
{
    public class WindsorDependencyScope : IDependencyScope
    {

        protected readonly IWindsorContainer Container;
        private ConcurrentBag<object> _toBeReleased = new ConcurrentBag<object>();

        public WindsorDependencyScope(IWindsorContainer container)
        {
            Container = container;
        }

        public void Dispose()
        {
            if (_toBeReleased != null)
            {
                foreach (var o in _toBeReleased)
                {
                    Container.Release(o);
                }
            }
            _toBeReleased = null;
        }

        public object GetService(Type serviceType)
        {
            if (!Container.Kernel.HasComponent(serviceType)) return null;

            var resolved = Container.Resolve(serviceType);
            if (resolved != null) _toBeReleased.Add(resolved);
            return resolved;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (!Container.Kernel.HasComponent(serviceType)) return new object[0];


            var allResolved = Container.ResolveAll(serviceType).Cast<object>();
            if (allResolved != null)
            {
                allResolved.ToList()
                    .ForEach(x => _toBeReleased.Add(x));
            }
            return allResolved;

        }
    }
}