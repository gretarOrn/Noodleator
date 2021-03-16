using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Noodleator.Models
{
    public class Nickname
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Noodle { get; set; }

        public string Nick { get; set; }

        public string About { get; set; }
        public string Author { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}