using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using Tornado.DataAccess.Interfaces;
using Tornado.Domain.Interfaces;

namespace Tornado.DataAccess.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class, IEntity, new() 
    {

        private readonly string _tableName;

        protected BaseRepository(string tableName)
        {
            _tableName = tableName;
        }

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection(ConfigurationManager.ConnectionStrings["Main"].ConnectionString);
            }
        }

        public T Create(T entity)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll()
        {
            IEnumerable<T> items;

            using (IDbConnection cn = Connection)
            {
                cn.Open();
                items = cn.Query<T>("SELECT * FROM " + _tableName);
            }

            return items;
        }

        public int GetTotal()
        {
            throw new NotImplementedException();
        }
        
        public T Get(Guid id)
        {
            T item;

            using (IDbConnection dbConnection = Connection)
            {
                dbConnection.Open();
                item = dbConnection.Query<T>("SELECT * FROM " + _tableName + " WHERE ID=@ID", new { id }).SingleOrDefault();
            }

            return item;
        }

        public ICollection<T> Where(Expression<Func<T, bool>> match)
        {
            throw new NotImplementedException();
        }

        public T Update(T entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Guid id)
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();
                cn.Execute("DELETE FROM " + _tableName + " WHERE ID=@ID", new { id });
            }
        }

        public void Delete(T entity)
        {
            using (IDbConnection cn = Connection)
            {
                cn.Open();
                cn.Execute("DELETE FROM " + _tableName + " WHERE ID=@ID", new { entity.Id });
            }
        }

        public void DeleteAll()
        {
            throw new NotImplementedException();
        }
    }
}
