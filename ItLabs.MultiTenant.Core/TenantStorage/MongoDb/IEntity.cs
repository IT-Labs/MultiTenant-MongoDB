using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ItLabs.MultiTenant.Core.MongoDb
{
    public interface IEntity
    {
        public string Id {get; set;}
    }
}