using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace realtorAPI.Models
{
    public class Owner
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string IdOwner { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string? Photo { get; set; }

        public DateTime? Birthday { get; set; }
    }
}

