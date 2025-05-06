using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SaveUpAppBackend.Models
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.Int32)]  // Stelle sicher, dass die ID als Int32 behandelt wird
        public int Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public double Price { get; set; }

        public DateTime Date { get; set; }
    }
}