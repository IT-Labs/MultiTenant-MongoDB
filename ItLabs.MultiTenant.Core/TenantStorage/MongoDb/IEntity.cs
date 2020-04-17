using MongoDB.Bson;

namespace ItLabs.MultiTenant.Core.MongoDb
{
    public interface IEntity
    {
        public BsonObjectId Id {get; set;}
    }
}