using SaveUpAppFrontend.Services;
using SaveUpAppFrontend.Models;

namespace SaveUpAppFrontend;

public partial class App : Application
{
    public App(JsonStorageService<Product> jsonStorage)
    {
        InitializeComponent();

        // Asynchronen Initialisierungsprozess starten
        Task.Run(async () => await InitializeData(jsonStorage));

        MainPage = new AppShell();
    }

    private async Task InitializeData(JsonStorageService<Product> jsonStorage)
    {
        // Beispiel: Produktliste speichern
        var products = new List<Product>
        {
            new Product { Id = 1, Description = "Produkt A", Price = 9.99, Date = DateTime.Now },
            new Product { Id = 2, Description = "Produkt B", Price = 19.99, Date = DateTime.Now }
        };

        await jsonStorage.SaveToFileAsync(products);

        // Beispiel: Produktliste laden
        var loadedProducts = await jsonStorage.LoadFromFileAsync();
        foreach (var product in loadedProducts)
        {
            Console.WriteLine($"{product.Id}: {product.Description} - {product.Price:C} am {product.Date}");
        }
    }
}