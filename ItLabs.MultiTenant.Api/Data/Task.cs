﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using ItLabs.MultiTenant.Core.MongoDb;

namespace ItLabs.MultiTenant.Api
{
    /// <summary>
    /// Task Entity
    /// </summary>
    [BsonDiscriminator("myclass")]
    public class Task : IEntity
    {
        /// <summary>
        /// The task Id (identifier)
        /// </summary>
        [BsonId]
        public BsonObjectId Id { get; set; }


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
        
        [BsonRepresentation(BsonType.Boolean)]
        public bool IsDone { get; set; }
    }
}
