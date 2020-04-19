using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ItLabs.MultiTenant.Core.MongoDb;

namespace ItLabs.MultiTenant.Api
{
    /// <summary>
    /// Task Entity
    /// </summary>
    public class Task : IEntity
    {
        /// <summary>
        /// The task Id (identifier)
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }


        /// <summary>
        /// The task name
        /// </summary>
        public string Name { get; set; }

        
        /// <summary>
        /// The task description
        /// </summary>
        public string Description { get; set; }


        /// <summary>
        /// The task flag for marking it done
        /// </summary>
        public bool IsDone { get; set; }
    }
}
