using MongoDB.Driver;
using SaveUpAppBackend.Models;
using SaveUpAppBackend.Data;
using MongoDB.Bson;

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

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _products.Find(_ => true).ToListAsync();  // Keine Änderungen erforderlich
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            // Generiere eine eindeutige ID, wenn sie nicht gesetzt ist
            if (product.Id == 0)
            {
                var lastProduct = await _products.Find(_ => true).SortByDescending(p => p.Id).FirstOrDefaultAsync();
                product.Id = (lastProduct?.Id ?? 0) + 1; // Auto-Inkrement
            }

            await _products.InsertOneAsync(product);
            return product;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var result = await _products.DeleteOneAsync(p => p.Id == id);
            return result.DeletedCount > 0;
        }

        public async Task DeleteAllAsync()
        {
            await _products.DeleteManyAsync(_ => true);
        }
    }
}
