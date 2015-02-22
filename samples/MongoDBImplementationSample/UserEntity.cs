using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDBImplementationSample
{
    public class UserEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string Name { get; set; }
        public int Rating { get; set; }
    }
}