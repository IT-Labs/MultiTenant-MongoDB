using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Bson;

namespace ItLabs.MultiTenant.Core.MongoDb
{
    public interface IMongoDbContext<T> where T : IEntity
    {
        public List<T> Get();
        public T Get(BsonObjectId id);
        public T Create(T entity);
        public void Update(BsonObjectId id, T entity);
        public void Remove(T entity);
        public void Remove(BsonObjectId id);
        public T Find(Expression<Func<T, bool>> filter);
    }
}