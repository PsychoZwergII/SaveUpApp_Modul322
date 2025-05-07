using SaveUpAppFrontend.Models;

namespace SaveUpAppFrontend.Services
{
    public class ProductService
    {
        private readonly JsonStorageService<Product> _jsonStorage;

        public ProductService()
        {
            // JSON-Datei für Produkte
            _jsonStorage = new JsonStorageService<Product>("products.json");
        }

        // Lädt Produkte (mit Fallback-Logik)
        public async Task<List<Product>> GetProductsAsync(bool databaseAvailable)
        {
            if (databaseAvailable)
            {
                // Hier würdest du Produkte aus der Datenbank laden
                // return await LoadFromDatabaseAsync();
                return new List<Product>(); // Platzhalter
            }
            else
            {
                // Produkte aus der JSON-Datei laden
                return await _jsonStorage.LoadFromFileAsync();
            }
        }

        // Speichert Produkte (mit Fallback-Logik)
        public async Task SaveProductsAsync(List<Product> products, bool databaseAvailable)
        {
            if (databaseAvailable)
            {
                // Hier würdest du Produkte in der Datenbank speichern
                // await SaveToDatabaseAsync(products);
            }
            else
            {
                // Produkte in der JSON-Datei speichern
                await _jsonStorage.SaveToFileAsync(products);
            }
        }
    }
}