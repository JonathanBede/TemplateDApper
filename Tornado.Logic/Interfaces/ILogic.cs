using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Tornado.Logic.Interfaces
{
    public interface ILogic<T>
    {
        List<T> GetAll();

        int GetTotal();

        T Get(Guid id);

        void Create(T entity);

        T Update(T entity);

        List<T> Where(Expression<Func<T, bool>> match);

        void Delete(Guid id);
    }
}
