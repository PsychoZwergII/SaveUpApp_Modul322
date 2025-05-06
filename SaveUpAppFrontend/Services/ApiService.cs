// Services/ApiService.cs
using System.Net.Http.Json;
using SaveUpAppFrontend.Models;

namespace SaveUpAppFrontend.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7137/api/") // ggf. anpassen
            };
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
