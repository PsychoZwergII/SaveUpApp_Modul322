using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SaveUpAppBackend.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]  // Stelle sicher, dass MongoDB die ID als Int32 behandelt
        public int Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public double Price { get; set; }

        public DateTime Date { get; set; }
    }
}
