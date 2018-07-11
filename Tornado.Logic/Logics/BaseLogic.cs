using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Tornado.DataAccess.Interfaces;
using Tornado.Domain.Interfaces;
using Tornado.Logic.Interfaces;

namespace Tornado.Logic.Logics
{
    public class BaseLogic<T> : ILogic<T> where T : class, IEntity, new()
    {
        private readonly IRepository<T> _repository;

        public BaseLogic(IRepository<T> repository)
        {
            _repository = repository;
        }

        public void Create(T entity)
        {
            _repository.Create(entity);
        }

        public T Update(T entity)
        {
            return _repository.Update(entity);
        }

        public List<T> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public int GetTotal()
        {
            return _repository.GetTotal();
        }

        public T Get(Guid id)
        {
            return _repository.Get(id);
        }

        public List<T> Where(Expression<Func<T, bool>> match)
        {
            return _repository.Where(match).ToList();
        }

        public void Delete(Guid id)
        {
            _repository.Delete(id);
        }

        public void DeleteAll()
        {
            _repository.DeleteAll();
        }
    }
}
