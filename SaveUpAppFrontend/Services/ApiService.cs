// Services/ApiService.cs
using System.Net.Http.Json;
using System.Text.Json;
using SaveUpAppFrontend.Models;

namespace SaveUpAppFrontend.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _localFilePath;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7137/api/") // ggf. anpassen
            };
            // Lokaler Dateipfad
            _localFilePath = Path.Combine(FileSystem.AppDataDirectory, "products.json");
        }

        public async Task SaveToLocalFileAsync(IEnumerable<Product> products)
        {
            try
            {
                var json = JsonSerializer.Serialize(products, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(_localFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to local file: {ex.Message}");
            }
        }

        public async Task<List<Product>> LoadFromLocalFileAsync()
        {
            try
            {
                if (File.Exists(_localFilePath))
                {
                    var json = await File.ReadAllTextAsync(_localFilePath);
                    return JsonSerializer.Deserialize<List<Product>>(json) ?? new List<Product>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from local file: {ex.Message}");
            }

            return new List<Product>();
        }
               

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Product>>("products");
        }
        public async Task<Product> AddProductAsync(Product product)
        {
            var response = await _httpClient.PostAsJsonAsync("products", product);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Fehler beim Hinzufügen: {response.ReasonPhrase}");
            }

            return await response.Content.ReadFromJsonAsync<Product>();
        }

        public async Task DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"products/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to delete product with ID {id}. Status Code: {response.StatusCode}");
            }
        }

        public async Task DeleteAllAsync()
        {
            await _httpClient.DeleteAsync("products/clear");
        }
    }
}
