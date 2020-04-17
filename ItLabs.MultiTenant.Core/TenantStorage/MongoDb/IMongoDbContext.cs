using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ItLabs.MultiTenant.Core.MongoDb
{
    public interface IMongoDbContext<T> where T : IEntity
    {
        public List<T> Get();
        public T Get(string id);
        public T Create(T entity);
        public void Update(string id, T entity);
        public void Remove(T entity);
        public void Remove(string id);
        public T Find(Expression<Func<T, bool>> filter);
    }
}