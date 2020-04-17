using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MongoDB.Driver.Core;

namespace ItLabs.MultiTenant.Core.MongoDb
{
    public class MongoDbContext<T> : IMongoDbContext<T>  where T : IEntity
    {
        private readonly IMongoCollection<T> entities;
        private readonly TenantService<Tenant> _tenantService;

        public MongoDbContext(TenantService<Tenant> tenantService)
        {
            _tenantService = tenantService;
            var tenant = _tenantService.GetTenantAsync().GetAwaiter().GetResult();
            MongoClient client = new MongoClient(tenant.ConnectionString);
            IMongoDatabase database = client.GetDatabase(tenant.Database);
            entities = database.GetCollection<T>(typeof(T).Name);
        }

        public List<T> Get()
        {
            return entities.Find(e => true).ToList();
        }

        public T Get(string id)
        {
            return entities.Find(e => e.Id == id).FirstOrDefault();
        }

        public T Create(T entity)
        {
            entities.InsertOne(entity);
            return entity;
        }

        public void Update(string id, T entity)
        {
            entities.ReplaceOne(e => e.Id == id, entity);
        }

        public void Remove(T entity)
        {
            entities.DeleteOne(e => e.Id == entity.Id);
        }

        public void Remove(string id)
        {
            entities.DeleteOne(e => e.Id == id);
        }
        
       public T Find(Expression<Func<T, bool>> filter)
        {
            return this.entities.AsQueryable<T>().FirstOrDefault<T>(filter);
        }
    }
}