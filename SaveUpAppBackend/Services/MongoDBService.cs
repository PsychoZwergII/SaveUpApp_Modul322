using MongoDB.Driver;
using SaveUpAppBackend.Models;
using SaveUpAppBackend.Data;

namespace SaveUpAppBackend.Services
{
    public class MongoDBService
    {
        private readonly IMongoCollection<Product> _products;

        public MongoDBService(IConfiguration config)
        {
            var settings = config.GetSection("MongoDbSettings").Get<MongoDbSettings>();
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _products = database.GetCollection<Product>(settings.ProductsCollectionName);
        }

        public async Task<List<Product>> GetProductsAsync() =>
            await _products.Find(_ => true).ToListAsync();

        public async Task<Product> CreateProductAsync(Product product)
        {
            // Setze die Id manuell oder lasse MongoDB diese selbst generieren
            await _products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)  // ID als int
        {
            var result = await _products.DeleteOneAsync(p => p.Id == id);  // Suche nach int ID
            return result.DeletedCount > 0;
        }

        public async Task DeleteAllAsync() =>
            await _products.DeleteManyAsync(_ => true);
    }
}
